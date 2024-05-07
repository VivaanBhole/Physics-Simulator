using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Vector3 _acceleration = Vector3.zero,
        _velocity = Vector3.zero;

    private float _verticalRotation = 0, _pDistance = 10;

    public enum PlacingMode{
        Place, Delete, Rotate, None
    }

    public PlacingMode Mode;

    [SerializeField] private float _maxSpeed, _accelerationMagnitude, _maxVerticalAngle, _floorY;
    [SerializeField] private float _pUpdateTime, _pAdjustmentSpeed;
    public bool CanMove, CanLook, CanPlace;
    public float MouseSensitivity;

    public GameObject selection;


    void Start()
    {
        toggleLockedCursor();
        Mode = PlacingMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CanLook = !CanLook;

        if(CanLook)
            handleMouseMovement();

        if(CanMove)
            handleMotionInputs();

        if (Mode == PlacingMode.Place)
            handlePlacementInputs();

        if (Input.GetKeyDown(KeyCode.P) && CanPlace && Mode == PlacingMode.None)
        {
            Mode = PlacingMode.Place;
            StartCoroutine(placeBlock(Instantiate(selection)));
        } else if (Input.GetKeyDown(KeyCode.P) && Mode == PlacingMode.Place)
            Mode = PlacingMode.None;

        if(Input.GetKeyDown(KeyCode.R) && Mode == PlacingMode.Place)
        {
            Mode = PlacingMode.Rotate;
        }

        if(Input.GetKeyDown(KeyCode.Backspace) && Mode == PlacingMode.None)
        {
            Mode = PlacingMode.Delete;
        }
        if(Input.GetKeyDown(KeyCode.Backspace) && Mode == PlacingMode.Delete)
        {
            delete();
        }

        

    }

    private void FixedUpdate()
    {
        updatePosition();
    }

    public void delete()
    {
        //TODO
    }

    public IEnumerator placeBlock(GameObject element)
    {
        Collider collider = element.GetComponentInChildren<Collider>();
        Transform elementTransform = element.transform;
        Rigidbody elementRB = elementTransform.GetComponentInChildren<Rigidbody>();

        PlacementDetector placementDetector = elementRB.gameObject.AddComponent<PlacementDetector>();

        collider.isTrigger = true;

        CanPlace = false;
        elementRB.isKinematic = true;


        while (!Input.GetMouseButton(1) || !placementDetector.isValid)
        {
            if(Mode == PlacingMode.Place)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                elementTransform.position = ray.GetPoint(_pDistance);
            }
            if(Mode == PlacingMode.Rotate)
            {
                //TODO
            }
            if (Mode == PlacingMode.None)
            {
                Destroy(element);
                yield break;
            }
            yield return new WaitForSeconds(_pUpdateTime);
        }

        Destroy(placementDetector);

        CanPlace = true;

        elementRB.isKinematic = false;
        collider.isTrigger = false;

    }

    private void setLayerRecursive(GameObject go, int layer)
    {
        go.layer = layer;
        foreach(Transform child in go.transform)
        {
            child.gameObject.layer = layer;
            if(child.childCount > 0)
               setLayerRecursive(child.gameObject, layer);
        }
    }



    public void handleMouseMovement()
    {
        //point camera to mouse
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity,
            mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        _verticalRotation = Mathf.Clamp(_verticalRotation - mouseY, -_maxVerticalAngle, _maxVerticalAngle);

        transform.localEulerAngles = Vector3.right * _verticalRotation + Vector3.up * (transform.localEulerAngles.y + mouseX);

    }

    private void handleMotionInputs()
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

    private void handlePlacementInputs()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _pDistance -= _pAdjustmentSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _pDistance += _pAdjustmentSpeed * Time.deltaTime;
        }
    }

    public void updatePosition()
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

    public void toggleLockedCursor()
    {
        UnityEngine.Cursor.visible = !UnityEngine.Cursor.visible;
        UnityEngine.Cursor.lockState = 1 - UnityEngine.Cursor.lockState;
    }
}
