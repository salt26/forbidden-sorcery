using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public enum NodeType
    {
        Normal,
        Castle,
        EnemySpawner,
    }

    [SerializeField]
    private NodeType type;

    [SerializeField]
    public int manaValue;

    [SerializeField]
    public List<Node> edges;

    public bool IsCastle
    {
        get
        {
            return isCastle;
        }
    }


    [HideInInspector]
    public List<Unit> units = new List<Unit>();
    public List<Unit> allies
    {
        get
        {
            return units.FindAll((unit) => unit.isAlly);
        }
    }
    public List<Unit> enemies
    {
        get
        {
            return units.FindAll((unit) => !unit.isAlly);
        }
    }
    [HideInInspector]
    public List<Unit> destroyedEnemies = new List<Unit>();

    [HideInInspector]
    public bool isPlayerTerritory;
    [HideInInspector]
    public int distance = int.MaxValue;
    
    private bool isCastle
    {
        get
        {
            return type == NodeType.Castle;
        }
    }
    
    public bool isEnemySpawner
    {
        get
        {
            return type == NodeType.EnemySpawner;
        }
    }

    public bool isFighting
    {
        get
        {
            return allies.Count > 0 && enemies.Count > 0;
        }
    }

    bool isRed;

    void Awake()
    {
        isRed = false;
        if (isCastle)
        {
            isPlayerTerritory = true;
        }
        foreach (var edge in edges)
        {
            if (!edge.edges.Contains(this))
            {
                edge.edges.Add(this);
            }
        }
    }

    void Start()
    {
        if (isCastle)
        {
            GameManager.instance.SetCastle(this);
            isPlayerTerritory = true;
        }
        if (isEnemySpawner)
        {
            isPlayerTerritory = false;
        }
        if (isPlayerTerritory)
        {
            if (!isCastle)
                this.GetComponent<SpriteRenderer>().color = Color.green;
        }
        GameManager.instance.allNodes.Add(this);
    }

    public void OnClick()
    {

    }

    void OnMouseUpAsButton()
    {
        GameManager.instance.SetNode(this);
    }

    public void RedLight()
    {
        if (isRed == false)
            StartCoroutine(RedLightAnimation());
    }
    
    public Unit Spawn(UnitData unitData, bool isAlly)
    {
        var unitObject = Instantiate(AssetManager.Instance.GetPrefab("Unit"));
        var unit = unitObject.GetComponent<Unit>();
        unit.SetUnit(unitData);
        unit.isAlly = isAlly;

        unit.transform.localPosition = this.transform.localPosition;
        unit.position = this;

        units.Add(unit);

        return unit;
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

    public void SetDIstance(int distance)
    {
        this.distance = distance;
        foreach (var node in edges)
        {
            if (node.distance > distance + 1)
            {
                node.SetDIstance(distance + 1);
            }
        }
    }
}
