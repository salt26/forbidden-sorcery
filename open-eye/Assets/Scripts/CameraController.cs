using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 0.25f;

    [SerializeField]
    private float scrollSpeed = 5f;

    [SerializeField]
    private float maxCameraSize = 10f;

    [SerializeField]
    private float minCameraSize = 3f;
    
    private Camera targetCamera;
    private float currentCameraSize = 5f;

    private Vector3 cameraDestination;

    private Vector2 previousInput;
    private bool isDragging = false;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = GetComponent<Camera>();
            currentCameraSize = targetCamera.orthographicSize;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                Vector3 beforeWorld = targetCamera.ScreenToWorldPoint(previousInput);
                Vector3 currentWorld = targetCamera.ScreenToWorldPoint(Input.mousePosition);

                cameraDestination += beforeWorld - currentWorld;
            }
            isDragging = true;
            previousInput = Input.mousePosition;
        }
        else
        {
            isDragging = false;
        }
        
        currentCameraSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        currentCameraSize = Mathf.Clamp(currentCameraSize, minCameraSize, maxCameraSize);

        targetCamera.orthographicSize = currentCameraSize;
        
        float moveRatio = 0.5f;
        Vector3 newPosition = transform.localPosition * (1.0f - moveRatio) + cameraDestination * moveRatio;
        
        transform.localPosition = new Vector3(newPosition.x, newPosition.y, transform.localPosition.z);
    }
}
