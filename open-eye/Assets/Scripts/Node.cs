using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private bool isKing;
    public List<Node> edges;
    [HideInInspector]
    public List<Unit> allies;
    [HideInInspector]
    public List<Unit> enemies;
    bool isRed;

    void Awake()
    {
        isRed = false;
        foreach (var edge in edges)
        {
            if (!edge.edges.Contains(this))
            {
                edge.edges.Add(this);
            }
        }
    }

    public void OnClick()
    {

    }

    void OnMouseUpAsButton()
    {
        Manager.manager.SetNode(this);
    }

    public void RedLight()
    {
        if (isRed == false)
            StartCoroutine(RedLightAnimation());
    }

    IEnumerator RedLightAnimation()
    {
        isRed = true;
        float duration = 0.5f;
        float deltaTime = 0;
        float rate = deltaTime / duration;
        var init = this.GetComponent<SpriteRenderer>().color;
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            this.GetComponent<SpriteRenderer>().color = Color.Lerp(init, Color.red, rate);
            yield return null;
        }

        rate = 0;
        deltaTime = 0;
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            this.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, init, rate);
            yield return null;
        }
        isRed = false;
    }
}
