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
    private bool dragged = false;
    
    public void OnMouseDown()
    {
        dragged = false;
        if (!Node.nodeBeingClicked) {
            isDragging = true;
        }
        previousInput = Input.mousePosition;
        inititalInput = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        isDragging = false;
        if ( !dragged && !Node.nodeBeingClicked )
        {
            GameManager.instance.UnitListShow(false);
            //selectedNode = null; 코드는 private이라 UnitListShow 함수에 적었습니다
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

            if ( !dragged && (previousInput - inititalInput).magnitude < dragLength )
            {
                dragged = true;
            }

        }
    }
}
