using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private bool isKing;
    public bool isEnemySpawner;
    public List<Node> edges = new List<Node>();
    [HideInInspector]
    public List<Unit> allies = new List<Unit>();
    [HideInInspector]
    public List<Unit> enemies = new List<Unit>();
    bool isRed;
    public GameObject allyPrefab;
    public GameObject enemyPrefab;
    public int isTerritory;//negative(-1) is enemy territory, 0 is neutral territory, possitive(1) is ally territory. if positive, add value to spell when turn ends
    public int distance = int.MaxValue;

    public bool IsKing
    {
        get
        {
            return isKing;
        }
    }

    void Awake()
    {
        isRed = false;
        if (isKing)
        {
            isTerritory = 1;
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
        if (isKing)
        {
            Manager.manager.kingTower = this;
            isTerritory = 1;
        }
        //foreach (var edge in edges)
        //{
        if (isEnemySpawner)
        {
            Manager.manager.enemySpawners.Add(this);
            isTerritory = -1;
        }
        //}
        if (isTerritory > 0)
            Manager.manager.territories.Add(this);
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

    public Unit SpawnEnemy()
    {
        if (isEnemySpawner)
        {
            var unitClone = Instantiate(enemyPrefab);
            var unitComponent = unitClone.GetComponent<Unit>();
            unitComponent.transform.localPosition = this.transform.localPosition;
            unitComponent.IsAlly = false;
            unitComponent.Position = this;
            unitComponent.IsMove = false;
            unitComponent.movableLength = 2;
            unitComponent.Kind = "" + Manager.manager.i++;
            unitComponent.Initialize();
            enemies.Add(unitClone.GetComponent<Unit>());
            return unitClone.GetComponent<Unit>();
        }
        return null;
    }

    public Unit SpawnAlly()
    {
        if (isKing)
        {
            var unitClone = Instantiate(allyPrefab);
            var unitComponent = unitClone.GetComponent<Unit>();
            unitComponent.transform.localPosition = this.transform.localPosition;
            unitComponent.IsAlly = true;
            unitComponent.Position = this;
            unitComponent.IsMove = false;
            unitComponent.movableLength = 2;
            unitComponent.Kind = "" + Manager.manager.i++;
            unitComponent.Initialize();
            allies.Add(unitClone.GetComponent<Unit>());
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
        foreach(var node in edges)
        {
            if(node.distance > distance + 1)
            {
                node.SetDIstance(distance + 1);
            }
        }
    }
}
