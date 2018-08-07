using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMapInput : MonoBehaviour
{
    [SerializeField]
    CameraController cameraController;
    
    private Vector2 previousInput;
    private bool isDragging = false;
    
    public void OnMouseDown()
    {
        isDragging = true;
        previousInput = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        isDragging = false;
    }

    public void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 beforeWorld = cameraController.targetCamera.ScreenToWorldPoint(previousInput);
            Vector3 currentWorld = cameraController.targetCamera.ScreenToWorldPoint(Input.mousePosition);

            cameraController.SetDestination(cameraController.cameraDestination + beforeWorld - currentWorld);

            previousInput = Input.mousePosition;
        }
    }
}
