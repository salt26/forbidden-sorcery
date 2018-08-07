using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 5f;

    [SerializeField]
    private float maxCameraSize = 10f;

    [SerializeField]
    private float minCameraSize = 3f;
    
    public Camera targetCamera { get; private set; }
    private float currentCameraSize = 5f;

    public Vector3 cameraDestination { get; private set; }
    
    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = GetComponent<Camera>();

            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            currentCameraSize = targetCamera.orthographicSize;
        }
    }

    private void Update()
    {
        if (GameManager.instance.isMouseInMap)
        {
            currentCameraSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            currentCameraSize = Mathf.Clamp(currentCameraSize, minCameraSize, maxCameraSize);

            targetCamera.orthographicSize = currentCameraSize;
        }
        
        float moveRatio = 0.5f;
        Vector3 newPosition = transform.localPosition * (1.0f - moveRatio) + cameraDestination * moveRatio;
        
        transform.localPosition = new Vector3(newPosition.x, newPosition.y, transform.localPosition.z);
    }

    public void SetDestination(Vector3 destination)
    {
        cameraDestination = destination;
    }
}
