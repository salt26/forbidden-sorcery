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
    public GameObject unitPrefab;

    void Awake()
    {
        isRed = false;
        if (isKing)
            isEnemySpawner = false;
        foreach (var edge in edges)
        {
            /*
            if (edge.isEnemySpawner)
            {
                Manager.manager.enemySpawners.Add(edge);
            }
            */
            if (!edge.edges.Contains(this))
            {
                edge.edges.Add(this);
            }
        }
    }

    void Start()
    {
        if (isKing)
            Manager.manager.kingTower = this;
        foreach (var edge in edges)
        {
            if (edge.isEnemySpawner)
            {
                Manager.manager.enemySpawners.Add(edge);
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

    public Unit SpawnEnemy()
    {
        if (isEnemySpawner)
        {
            var unitClone = Instantiate(unitPrefab);
            var unitComponent = unitClone.GetComponent<Unit>();
            unitComponent.transform.localPosition = this.transform.localPosition;
            unitComponent.IsAlly = false;
            unitComponent.Position = this;
            unitComponent.IsMove = false;
            unitComponent.movableLength = 2;
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
            var unitClone = Instantiate(unitPrefab);
            var unitComponent = unitClone.GetComponent<Unit>();
            unitComponent.transform.localPosition = this.transform.localPosition;
            unitComponent.IsAlly = true;
            unitComponent.Position = this;
            unitComponent.IsMove = false;
            unitComponent.movableLength = 2;
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
}
