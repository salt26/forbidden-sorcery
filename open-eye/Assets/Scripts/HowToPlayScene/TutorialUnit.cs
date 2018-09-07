using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUnit : MonoBehaviour
{
    public GameObject nearNodeLeft, nearNodeCentral, nearNodeRight;

    Vector3 nearNodeLeftPosition, nearNodeCentralPosition, nearNodeRightPosition;

    void Awake()
    {
        nearNodeLeftPosition = nearNodeLeft.GetComponent<Transform>().position;
        nearNodeCentralPosition = nearNodeCentral.GetComponent<Transform>().position;
        nearNodeRightPosition = nearNodeRight.GetComponent<Transform>().position;
    }

    IEnumerator Move(Vector3 destination, float time)
    {
        float deltaTime = 0;
        float rate = deltaTime / time;
        var initialPosition = this.transform.position;
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / time;
            transform.position = Vector3.Lerp(initialPosition, destination, rate);
            yield return null;
        }
        transform.position = destination;

        
    }
}
