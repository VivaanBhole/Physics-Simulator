using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Vector3 _acceleration = Vector3.zero,
        _velocity = Vector3.zero;

    private float _verticalRotation = 0, _pDistance = 10;

    private Vector3 _pRotation;

    private Transform _outlinedElement;
    private Outline _outline;

    public enum PlacingMode{
        Place, Delete, None
    }

    [NonSerialized] public PlacingMode Mode;

    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _accelerationMagnitude;
    [SerializeField] private float _floorY;

    [Header("Looking Settings")]
    [SerializeField] private float _maxVerticalAngle;
    public float MouseSensitivity;

    [Header("Placement Settings")] 
    [SerializeField] private float _pUpdateTime;
    [SerializeField] private float _pAdjustmentSpeed;
    [SerializeField] private float _pRotationSpeed;
    [SerializeField] private float _pRotationRes;
    //[NonSerialized]
    public GameObject selection;


    [Header("Ability Toggles")] 
    public bool CanMove;
    public bool CanLook;
    public bool CanInteract;

    void Start()
    {
        ToggleLockedCursor();
        Mode = PlacingMode.None;
    }

    void Update()
    {

        //for testing only
        if (Input.GetKeyDown(KeyCode.Escape)) 
            CanLook = !CanLook;
        if (Input.GetKeyDown(KeyCode.E))
            Mode = PlacingMode.None;



        if (CanLook)
            HandleMouseMovement();

        if(CanMove)
            HandleMotionInputs();

        if (CanInteract)
            OutlineObject();

        if (Mode == PlacingMode.Place)
        {
            HandlePlacementInputs();
            HandleRotationInputs();
        }

        //placing elements modes

        if(Input.GetKeyDown(KeyCode.P))
            switch (Mode)
            {
                case PlacingMode.Place:
                    Mode = PlacingMode.None; break;
                case PlacingMode.Delete:
                    if(_outline) _outline.color = 0;
                    goto case PlacingMode.Place;
                case PlacingMode.None:
                    if(CanInteract)
                        StartCoroutine(PlaceElement(Instantiate(selection)));
                    break;
                }

        //delete mode

        if (Input.GetKeyDown(KeyCode.Backspace))
            switch (Mode)
            {
                case PlacingMode.Delete:
                    Mode = PlacingMode.None;
                    if (_outline) _outline.color = 0;
                    break;
                case PlacingMode.None:
                    if (_outline) _outline.color = 1;
                    goto default;
                default:
                    Mode = PlacingMode.Delete; break;
            }
        if(Input.GetMouseButtonDown(0) && _outlinedElement && CanInteract)
        {
            if(Mode == PlacingMode.Delete)
                Destroy(_outlinedElement.gameObject);
            if (Mode == PlacingMode.None)
                OpenElementUI(_outlinedElement);
        }
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    public void OutlineObject()
    {
        Transform go = GetObjectInFront();
        Element e;
        if (!go || !(e = go.GetComponentInChildren<Element>()))
        {
            _outlinedElement = null;
            Destroy(_outline);
            return;
        }

        if (_outlinedElement == null || !go.Equals(_outlinedElement))
        {
            _outlinedElement = go;

            if (_outline)
                Destroy(_outline);

            _outline = e.gameObject.AddComponent<Outline>();
            if (Mode == PlacingMode.Delete)
                _outline.color = 1;
        }
    }

    public IEnumerator PlaceElement(GameObject element)
    {
        element.name = Time.frameCount+"";
        Collider collider = element.GetComponentInChildren<Collider>();
        Transform elementTransform = element.transform;
        Rigidbody elementRB = GetOrAddRigidbody(element);

        PlacementDetector placementDetector = elementRB.gameObject.AddComponent<PlacementDetector>();

        collider.isTrigger = true;

        CanInteract = false;
        elementRB.isKinematic = true;
        Mode = PlacingMode.Place;
        ConnectionPoint.isVisible = true;

        CircuitElement circuitElement = element.GetComponentInChildren<CircuitElement>();


        while ((!Input.GetMouseButton(1) || !placementDetector.isValid) && Mode == PlacingMode.Place)
        {
            elementTransform.localEulerAngles = RoundDown(_pRotation, _pRotationRes);

            if (circuitElement)
                elementTransform.position = circuitElement.GetDesiredPos(transform.position + transform.forward * _pDistance);
            else
                elementTransform.position = transform.position + transform.forward * _pDistance;

            yield return new WaitForSeconds(_pUpdateTime);
        }

        if(Mode != PlacingMode.Place)
        {
            Destroy(element);
        }
        else
        {
            Destroy(placementDetector);
            Mode = PlacingMode.None;
            elementRB.isKinematic = false;
            collider.isTrigger = false;
        }

        if (circuitElement)
        {
            elementRB.useGravity = false;
            circuitElement.AddToCircuit();
        }

        CanInteract = true;
        ConnectionPoint.isVisible = false;

    }

    private void OpenElementUI(Transform t)
    {
        //TODO
    }

    public void HandleMouseMovement()
    {
        //point camera to mouse
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity,
            mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        _verticalRotation = Mathf.Clamp(_verticalRotation - mouseY, -_maxVerticalAngle, _maxVerticalAngle);

        transform.localEulerAngles = Vector3.right * _verticalRotation + Vector3.up * (transform.localEulerAngles.y + mouseX);

    }

    private void HandleMotionInputs()
    {
        _acceleration = Vector3.zero;

        //horizontal movement
        if (Input.GetKey(KeyCode.A))
        {
            _acceleration += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _acceleration += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _acceleration += Vector3.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _acceleration += Vector3.forward;
        }

        //vertical movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _acceleration += Vector3.down;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _acceleration += Vector3.up;
        }

    }

    private void HandlePlacementInputs()
    {
        _pDistance += Input.mouseScrollDelta.y * _pAdjustmentSpeed * Time.deltaTime;
    }

    private void HandleRotationInputs()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _pRotation += _pRotationSpeed * Time.deltaTime * Vector3.back;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _pRotation += _pRotationSpeed * Time.deltaTime * Vector3.forward;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _pRotation += _pRotationSpeed * Time.deltaTime * Vector3.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _pRotation += _pRotationSpeed * Time.deltaTime * Vector3.up;
        }
    }

    public void UpdatePosition()
    {
        _acceleration *= _accelerationMagnitude;


        // if an axis has no acceleration slow down

        if (_acceleration.x == 0 && _velocity.x != 0)
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, _accelerationMagnitude);

        if (_acceleration.y == 0 && _velocity.y != 0)
            _velocity.y = Mathf.MoveTowards(_velocity.y, 0, _accelerationMagnitude);

        if (_acceleration.z == 0 && _velocity.z != 0)
            _velocity.z = Mathf.MoveTowards(_velocity.z, 0, _accelerationMagnitude);

        //clamps velocity, adds to position, clamps position
        _velocity = Vector3.ClampMagnitude(_velocity + _acceleration, _maxSpeed);

        transform.position += _velocity.x * transform.right
            + _velocity.z * new Vector3(transform.forward.x, 0, transform.forward.z).normalized
            + _velocity.y * Vector3.up;

        if (transform.position.y <= _floorY && _velocity.y < 0)
        {
            _velocity.y = 0;
            transform.position = new Vector3(transform.position.x, _floorY, transform.position.z);
        }
    }

    private Vector3 RoundDown(Vector3 vector, float round)
    {
        
        return new Vector3(
            ((int)(vector.x / round)) * round,
            ((int)(vector.y / round)) * round,
            ((int)(vector.z / round)) * round);
    }

    public Transform GetObjectInFront()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 200))
        {
            return hit.collider.transform;
        }
        return null;
    }

    private Rigidbody GetOrAddRigidbody(GameObject go)
    {
        Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
        if (rb)
            return rb;

        return go.GetComponentInChildren<Collider>().gameObject.AddComponent<Rigidbody>();

    }

    private void SetLayerRecursive(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
        {
            child.gameObject.layer = layer;
            if (child.childCount > 0)
                SetLayerRecursive(child.gameObject, layer);
        }
    }

    public void ToggleLockedCursor()
    {
        UnityEngine.Cursor.visible = !UnityEngine.Cursor.visible;
        UnityEngine.Cursor.lockState = 1 - UnityEngine.Cursor.lockState;
    }

    public void EnterMenu()
    {
        CanInteract = false;
        CanLook = false;
        CanMove = false;
    }

    public void ExitMenu()
    {
        CanInteract = true;
        CanLook = true;
        CanMove = true;
    }
}
