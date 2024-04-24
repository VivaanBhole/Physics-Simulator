using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    Vector3 acceleration = Vector3.zero,
        velocity = Vector3.zero;

    float verticalRotation = 0;


    public float maxSpeed, accelerationMagnitude, maxVerticalAngle, floorY, mouseSensitivity;


    void Start()
    {
        toggleLockedCursor();
    }

    void Update()
    {
        //point camera to mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity,
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation = Mathf.Clamp(verticalRotation - mouseY, -maxVerticalAngle, maxVerticalAngle);

        transform.localEulerAngles = Vector3.right * verticalRotation + Vector3.up * (transform.localEulerAngles.y + mouseX);


        acceleration = Vector3.zero;

        //horizontal movement recording
        if (Input.GetKey(KeyCode.A))
        {
            acceleration += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            acceleration += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            acceleration += Vector3.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            acceleration += Vector3.forward;
        }

        //vertical movement recording
        if (Input.GetKey(KeyCode.LeftShift))
        {
            acceleration += Vector3.down;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            acceleration += Vector3.up;
        }

    }

    private void FixedUpdate()
    {

        acceleration *= accelerationMagnitude;


        // if an axis has no acceleration slow down

        if (acceleration.x == 0 && velocity.x != 0)
            velocity.x = Mathf.MoveTowards(velocity.x, 0, accelerationMagnitude);

        if (acceleration.y == 0 && velocity.y != 0)
            velocity.y = Mathf.MoveTowards(velocity.y, 0, accelerationMagnitude);

        if (acceleration.z == 0 && velocity.z != 0)
            velocity.z = Mathf.MoveTowards(velocity.z, 0, accelerationMagnitude);

        //clamps velocity, adds to position, clamps position
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);

        transform.position += velocity.x * transform.right
            + velocity.z * new Vector3(transform.forward.x, 0, transform.forward.z).normalized
            + velocity.y * Vector3.up;

        if (transform.position.y <= floorY && velocity.y < 0)
        {
            velocity.y = 0;
            transform.position = new Vector3(transform.position.x, floorY, transform.position.z);
        }
    }

    public void toggleLockedCursor()
    {
        UnityEngine.Cursor.visible = !UnityEngine.Cursor.visible;
        UnityEngine.Cursor.lockState = 1 - UnityEngine.Cursor.lockState;
    }
}
