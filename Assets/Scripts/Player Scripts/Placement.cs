using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    private float _pDistance = 10;

    private Vector3 _pRotation;

    private Transform _outlinedElement;
    private Outline _outline;

    [Header("Placement Settings")]
    [SerializeField] private float _pUpdateTime;
    [SerializeField] private float _pAdjustmentSpeed;
    [SerializeField] private float _pRotationSpeed;
    [SerializeField] private float _pRotationRes;
    //[NonSerialized]
    public GameObject selection;

    public bool CanInteract;



    public enum PlacingMode
    {
        Place, Delete, None
    }

    [NonSerialized] public PlacingMode Mode;
    void Start()
    {
        Mode = PlacingMode.None;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Mode = PlacingMode.None;

        if (CanInteract)
            OutlineObject();

        if (Mode == PlacingMode.Place)
        {
            HandlePlacementInputs();
            HandleRotationInputs();
        }

        //placing elements modes

        if (Input.GetKeyDown(KeyCode.P))
            switch (Mode)
            {
                case PlacingMode.Place:
                    Mode = PlacingMode.None; break;
                case PlacingMode.Delete:
                    if (_outline) _outline.color = 0;
                    goto case PlacingMode.Place;
                case PlacingMode.None:
                    if (CanInteract)
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

        if (Input.GetMouseButtonDown(0) && _outlinedElement && CanInteract)
        {
            if (Mode == PlacingMode.Delete)
                Destroy(_outlinedElement.gameObject);
            if (Mode == PlacingMode.None)
                OpenElementUI(_outlinedElement);
        }

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
        element.name = Time.frameCount + "";
        Collider collider = element.GetComponentInChildren<Collider>();
        Transform elementTransform = element.transform;
        Rigidbody elementRB = GetOrAddRigidbody(element);

        PlacementDetector placementDetector = elementRB.gameObject.AddComponent<PlacementDetector>();

        collider.isTrigger = true;

        CanInteract = false;
        elementRB.isKinematic = true;
        Mode = PlacingMode.Place;
        ConnectionPoint.isVisible = true;



        while ((!Input.GetMouseButton(1) || !placementDetector.isValid) && Mode == PlacingMode.Place)
        {
            elementTransform.localEulerAngles = RoundDown(_pRotation, _pRotationRes);

            elementTransform.position = transform.position + transform.forward * _pDistance;

            yield return new WaitForSeconds(_pUpdateTime);
        }

        if (Mode != PlacingMode.Place)
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


        CanInteract = true;
        ConnectionPoint.isVisible = false;

    }

    private void OpenElementUI(Transform t)
    {
        //TODO
    }

    private void HandlePlacementInputs()
    {
        _pDistance += Input.mouseScrollDelta.y * _pAdjustmentSpeed * Time.deltaTime;
        _pDistance = Mathf.Min(0, _pDistance);
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

        return go.GetComponentInChildren<Element>().gameObject.AddComponent<Rigidbody>();

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
}
