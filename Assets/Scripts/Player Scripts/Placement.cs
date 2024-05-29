using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    protected float _pDistance = 10;

    protected Vector3 _pRotation;

    protected Transform _outlinedElement;
    protected Outline _outline;

    [Header("Placement Settings")]
    [SerializeField] protected float _pUpdateTime;
    [SerializeField] protected float _pAdjustmentSpeed;
    [SerializeField] protected float _pRotationSpeed;
    [SerializeField] protected float _pRotationRes;
    [SerializeField] protected Hotbar _hotbar;
    //[NonSerialized]
    [SerializeField] private ElementData _selection;

    public bool CanInteract;

    public PlayerMovement movement;

    private bool inElementMenu, inSidebar;



    public enum PlacingMode
    {
        Place, Delete, None
    }

    [NonSerialized] public PlacingMode Mode;
    void Start()
    {
        Mode = PlacingMode.None;
    }

    private void OnEnable()
    {
        _hotbar.updateSelection += SetSelection;
    }

    private void OnDisable()
    {
        _hotbar.updateSelection -= SetSelection;
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

        //UI

        if (Input.GetMouseButtonDown(0) && _outlinedElement && CanInteract)
        {
            if (Mode == PlacingMode.Delete)
                Delete();
            if (Mode == PlacingMode.None)
                OpenElementUI(_outlinedElement);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inElementMenu) CloseElementUI();
            if (inSidebar) CloseSidebar();
        }

        if (Input.GetKeyDown(KeyCode.C) && !inSidebar)
        {
            OpenSidebar();
        }

        if (inSidebar || inElementMenu)
            return;

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
                        StartCoroutine(PlaceElement(Instantiate(_selection.GetDefaultObject())));
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



    }

    private void OpenSidebar()
    {
        inSidebar = true;
        Config.Instance.transform.localScale = Vector3.one;
        EnterMenu();
    }

    private void CloseSidebar()
    {
        inSidebar = false;
        Config.Instance.transform.localScale = Vector3.zero;
        ExitMenu();
    }


    public void OutlineObject()
    {
        Transform go = GetObjectInFront();
        
        Element e;
        if (!go || !(e = go.GetComponentInChildren<Element>()) && !(e = go.GetComponentInParent<Element>()))
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

            _outline = e.gameObject.GetComponentInChildren<Renderer>().gameObject.AddComponent<Outline>();
            if (Mode == PlacingMode.Delete)
                _outline.color = 1;
        }
    }

    public void Delete()
    {
        Transform go = GetObjectInFront();
        if (go.GetComponentInParent<Element>())
            Destroy(go.parent.gameObject);
        else
            Destroy(go.gameObject);
    }

    public virtual IEnumerator PlaceElement(GameObject element)
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
        Element e = t.GetComponentInChildren<Element>();
        if (!e) e = t.GetComponentInParent<Element>();
        ModifyMenu.Instance.Open(e);
        EnterMenu();
        inElementMenu = true;
    }

    private void CloseElementUI()
    {
        ModifyMenu.Instance.Close();
        ExitMenu();
        inElementMenu = false;
    }

    private void HandlePlacementInputs()
    {
        _pDistance += Input.mouseScrollDelta.y * _pAdjustmentSpeed * Time.deltaTime;
        _pDistance = Mathf.Max(0, _pDistance);
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

    protected Vector3 RoundDown(Vector3 vector, float round)
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

        if (!rb)
            rb = go.GetComponentInChildren<Element>().gameObject.AddComponent<Rigidbody>();

        return rb;

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

    private void SetSelection(ElementData s)
    {
        _selection = s;
    }

    private void EnterMenu()
    {
        CanInteract = false;
        movement.CanLook = false;
        movement.CanMove = false;
        movement.ToggleLockedCursor();
    }

    private void ExitMenu()
    {
        CanInteract = true;
        movement.CanLook = true;
        movement.CanMove = true;
        movement.ToggleLockedCursor();
    }
}
