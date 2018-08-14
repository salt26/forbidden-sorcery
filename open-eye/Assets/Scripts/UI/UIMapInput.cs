using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMapInput : MonoBehaviour
{
    [SerializeField]
    CameraController cameraController;

    [SerializeField]
    int dragLength;

    private Vector2 previousInput;
    private Vector2 inititalInput;
    private bool isDragging = false;
    
    public void OnMouseDown()
    {
        if (!Node.nodeBeingClicked) {
            isDragging = true;
        }
        previousInput = Input.mousePosition;
        inititalInput = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        isDragging = false;
        if ( (previousInput - inititalInput).magnitude < dragLength )
        {
            GameManager.instance.UnitListDown();
        }
        Node.nodeBeingClicked = false;
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
