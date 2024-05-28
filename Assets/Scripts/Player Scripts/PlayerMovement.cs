using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 _acceleration = Vector3.zero,
            _velocity = Vector3.zero;

    private float _verticalRotation = 0;

    public enum PlacingMode
    {
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

    [Header("Ability Toggles")]
    public bool CanMove;
    public bool CanLook;
    
    void Start()
    {
        ToggleLockedCursor();
    }

    // Update is called once per frame
    void Update()
    {
        //for testing only
        if (Input.GetKeyDown(KeyCode.Escape))
            CanLook = !CanLook;

        if (CanLook)
            HandleMouseMovement();

        if (CanMove)
            HandleMotionInputs();
    }

    private void FixedUpdate()
    {
        UpdatePosition();
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

    public void ToggleLockedCursor()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = 1 - Cursor.lockState;
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
}
