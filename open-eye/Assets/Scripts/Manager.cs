using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager manager;
    Node from = null;
    Node to = null;
    Color originColor = Color.white;
    private void Awake()
    {
        manager = this;
    }

    private void Start()
    {
        
    }

    public void SetNode(Node n)
    {
        if (from == null && n.allies.Count != 0)
        {
            from = n;
            var spriteRenderer = from.GetComponent<SpriteRenderer>();
            originColor = spriteRenderer.color;
            spriteRenderer.color = Color.black;
        }
        else if (from == null)
        {
            n.RedLight();
        }
        else if (from == n)
        {
            from.GetComponent<SpriteRenderer>().color = originColor;
            from = null;
        }
        else if (!from.edges.Contains(n))
        {
            n.RedLight();
        }
        else if (to == null)
        {
            to = n;
            from.GetComponent<SpriteRenderer>().color = originColor;
            foreach (var unit in from.allies)
            {
                unit.Move(from, to);
            }
            from.allies.Clear();
            from = null;
            to = null;
        }
    }
}
