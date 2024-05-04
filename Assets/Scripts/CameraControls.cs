using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

    [SerializeField] private Camera _camera;

    [SerializeField] private float moveSpeed = 30f;

    [Header("Bounds")]
    [SerializeField] private float cameraLeftBound = -8f;
    [SerializeField] private float cameraRightBound = 8f;
    [SerializeField] private float cameraTopBound = 8f;
    [SerializeField] private float cameraBottomBound = -8f;

    private Vector2 velocity = Vector2.zero;
    private float drag = 1f;

    private void Update() {
        if (Input.GetMouseButton(1)) {
            // right mouse button currently down
            HandleScroll();
        } else if (Input.GetMouseButtonUp(1)) {
            // right mouse button just released
            HandleFling();
        } else {
            // no input this frame
            HandleNoInput();
        }
    }

    /*
    For smooth velocity physics after fling
    */
    private void HandleNoInput()
    {
        if (velocity.magnitude > 0.01f)
        {
            velocity = Vector2.Lerp(velocity, Vector2.zero, drag * Time.deltaTime);
            Vector2 newCameraPos = _camera.transform.position + new Vector3(velocity.x, velocity.y, 0);
    
            CheckBounds(ref newCameraPos);
            _camera.transform.position = new Vector3(newCameraPos.x, newCameraPos.y, _camera.transform.position.z);
        }
    }

    /*
    When users release right click, they can fling the camera across the map according to the mouse movement in that frame
    */
    private void HandleFling()
    {
        float inputX = - Input.GetAxis("Mouse X");
        float inputY = - Input.GetAxis("Mouse Y");

        velocity = new Vector2(inputX, inputY) * 0.1f;
    }

    private void HandleScroll() {
        float inputX = - Input.GetAxis("Mouse X");
        float inputY = - Input.GetAxis("Mouse Y");
        Vector3 camPos = _camera.transform.position;
        Vector2 newPos = new Vector2(camPos.x, camPos.y);
        
        if(inputX != 0) {
            newPos.x = camPos.x + inputX * moveSpeed * Time.deltaTime;
        }

        if(inputY != 0) {
            newPos.y = camPos.y + inputY * moveSpeed * Time.deltaTime;
        }

        CheckBounds(ref newPos);

        Vector3 newCameraPos = new Vector3(newPos.x, newPos.y, camPos.z);
        _camera.transform.position = newCameraPos;
    }

    /*
    Given a vector2, ensure it stays within the defined bounds
    */
    private void CheckBounds(ref Vector2 position) {
        if (position.x < cameraLeftBound) {
            position.x = cameraLeftBound;
        }

        if (position.x > cameraRightBound) {
            position.x = cameraRightBound;
        }

        if (position.y > cameraTopBound) {
            position.y = cameraTopBound;
        }

        if (position.y < cameraBottomBound) {
            position.y = cameraBottomBound;
        }
    }

}
