using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private bool isCastle;
    public bool isEnemySpawner;
    public List<Node> edges;
    [HideInInspector]
    public List<Unit> allies = new List<Unit>();
    [HideInInspector]
    public List<Unit> enemies = new List<Unit>();
    [HideInInspector]
    public List<Unit> destroyedEnemies = new List<Unit>();
    bool isRed;
    [HideInInspector]
    public bool isPlayerTerritory;
    [HideInInspector]
    public int distance = int.MaxValue;

    public bool IsCastle
    {
        get
        {
            return isCastle;
        }
    }

    void Awake()
    {

        isRed = false;
        if (isCastle)
        {
            isPlayerTerritory = true;
            isEnemySpawner = false;
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
            Manager.manager.kingTower = this;
            isPlayerTerritory = true;
        }
        if (isEnemySpawner)
        {
            Manager.manager.enemySpawners.Add(this);
            isPlayerTerritory = false;
        }
        if (isPlayerTerritory)
        {
            Manager.manager.territories.Add(this);
            if (!isCastle)
                this.GetComponent<SpriteRenderer>().color = Color.green;
        }
        Manager.manager.allNodes.Add(this);
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
    
    public Unit Spawn(UnitData unitData)
    {
        if (isCastle && unitData.isAlly)
        {
            var unitClone = Instantiate(PrefabManager.Instance.GetPrefab("Unit"));
            var unitComponent = unitClone.GetComponent<Unit>();
            unitClone.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(unitData.kind);
            unitComponent.transform.localPosition = this.transform.localPosition;
            unitComponent.isAlly = true;
            unitComponent.position = this;
            unitComponent.isMove = false;
            unitComponent.staticMovableLength = unitData.movableLength;
            unitComponent.movableLength = unitData.movableLength;
            unitComponent.kind = unitData.kind;
            unitComponent.attck = unitData.attack;
            unitComponent.health = unitData.health;
            unitComponent.unitData = unitData;
            unitComponent.Initialize();
            allies.Add(unitClone.GetComponent<Unit>());
            return unitClone.GetComponent<Unit>();
        }
        else if (isEnemySpawner && !unitData.isAlly)
        {
            var unitClone = Instantiate(PrefabManager.Instance.GetPrefab("Unit"));
            var unitComponent = unitClone.GetComponent<Unit>();
            unitClone.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(unitData.kind);
            unitComponent.transform.localPosition = this.transform.localPosition;
            unitComponent.isAlly = false;
            unitComponent.position = this;
            unitComponent.isMove = false;
            unitComponent.staticMovableLength = unitData.movableLength;
            unitComponent.movableLength = unitData.movableLength;
            unitComponent.kind = unitData.kind;
            unitComponent.attck = unitData.attack;
            unitComponent.health = unitData.health;
            unitComponent.unitData = unitData;
            unitComponent.Initialize();
            enemies.Add(unitClone.GetComponent<Unit>());
            return unitClone.GetComponent<Unit>();
        }
        return null;
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
