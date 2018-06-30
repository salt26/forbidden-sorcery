using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    Node position;
    [SerializeField]
    bool isAlly;

    void Awake()
    {
        if (isAlly)
            position.allies.Add(this);
        else
        {
            position.enemies.Add(this);
            this.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    public bool IsAlly
    {
        get
        {
            return isAlly;
        }
        set
        {
            isAlly = value;
        }
    }

    public void Move(Node from, Node to)
    {
        position = null;
        StartCoroutine(MoveAnimation(from, to));
    }

    IEnumerator MoveAnimation(Node from, Node to)
    {
        float duration = 0.5f;
        float deltaTime = 0;
        float rate = deltaTime / duration;
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            transform.localPosition = Vector3.Lerp(from.transform.localPosition, to.transform.localPosition, rate);
            yield return null;
        }

        position = to;
        transform.localPosition = to.transform.localPosition;
        to.allies.Add(this);
    }
}
