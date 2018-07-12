using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager manager;
    Node from = null;
    Node to = null;
    Color originColor = Color.white;
    private int state;
    private int notoriety;//delta karma
    private int spell;
    private int karma;//accumulated notoriety
    bool isStateReady = false;
    public Node kingTower;
    bool isNodeClickable;
    bool isEnemyMoveDone = false;
    public List<Node> enemySpawners = new List<Node>();
    [HideInInspector]
    public List<Unit> allies = new List<Unit>();
    List<Unit> enemies = new List<Unit>();
    [HideInInspector]
    public List<Node> territories = new List<Node>();
    [HideInInspector]
    public List<Node> allNodes = new List<Node>();
    public Button endTurnButton;
    public GameObject scrollview;
    public List<Unit> buttonUnitList = new List<Unit>();
    public List<Unit> spawnableUnitList = new List<Unit>();


    public int i = 0;

    private void Awake()
    {
        spell = 3;
        state = 0;
        karma = 0;
        notoriety = 1;
        manager = this;
        scrollview.SetActive(false);
    }

    private void Start()
    {
        allies.Add(kingTower.SpawnAlly());
        Ready();
        state = 1;
        kingTower.SetDIstance(0);
    }

    private void FixedUpdate()
    {
        if (!isEnemyMoveDone)
        {
            bool isAllEnemyMoved = true;
            foreach (Unit enemy in enemies)
            {
                if (enemy.moveQueue.Count != 0 || !enemy.IsMoved)
                    isAllEnemyMoved = false;
            }
            if (isAllEnemyMoved)
            {
                isEnemyMoveDone = true;
                PlayerAction();
            }
        }
        
        if (spawnableUnitList.Count != 0)
        {
            Unit unit = kingTower.SpawnAlly();
            allies.Add(unit);
            ScrollViewContent.manager.AddComponent(unit, true);
            spawnableUnitList.Clear();
        }
    }

    public void OnClick()
    {
        buttonUnitList.Clear();
        scrollview.SetActive(false);
        ScrollViewContent.manager.Reset();
        if (from != null && from.GetComponent<SpriteRenderer>().color != originColor)
            from.GetComponent<SpriteRenderer>().color = originColor;
        if (state == 0 && isStateReady)
        {
            Ready();
            state = 1;
        }
        else if (isStateReady)
        {
            isNodeClickable = false;
            Fight();
            state = 0;
        }
    }



    public void SetNode(Node n)
    {

        if (isNodeClickable)
        {
            /*
            if (selected == n)
            {
                buttonUnitList.Clear();
                scrollview.SetActive(false);
                ScrollViewContent.manager.Reset();
                selected = null;
            }
            else if (from == null && n.allies.Count != 0)
            {
                from = n;
                var spriteRenderer = from.GetComponent<SpriteRenderer>();
                originColor = spriteRenderer.color;
                spriteRenderer.color = Color.black;

                buttonUnitList.Clear();
                scrollview.SetActive(true);
                ScrollViewContent.manager.Reset();
                foreach (Unit u in n.allies)
                {
                    ScrollViewContent.manager.AddComponent(u);
                }
            }
            else if (from == null && n.enemies.Count != 0)
            {
                buttonUnitList.Clear();
                ScrollViewContent.manager.Reset();
                scrollview.SetActive(true);
                foreach (Unit u in n.enemies)
                {
                    ScrollViewContent.manager.AddComponent(u);
                }
                selected = n;
            }
            */

            if(from == null && (n.IsKing || n.allies.Count != 0 || n.enemies.Count != 0))
            {
                from = n;
                var spriteRenderer = from.GetComponent<SpriteRenderer>();
                originColor = spriteRenderer.color;
                spriteRenderer.color = Color.black;

                buttonUnitList.Clear();
                scrollview.SetActive(true);
                ScrollViewContent.manager.Reset();
                if (n.IsKing)
                {
                    ScrollViewContent.manager.AddComponent(n.allyPrefab.GetComponent<Unit>(), false);
                }
                foreach (Unit u in n.allies)
                {
                    ScrollViewContent.manager.AddComponent(u, true);
                }
                foreach (Unit u in n.enemies)
                {
                    ScrollViewContent.manager.AddComponent(u, true);
                }
            }

            else if (from == null)
            {
                n.RedLight();
            }
            else if (from == n)
            {
                from.GetComponent<SpriteRenderer>().color = originColor;
                from = null;

                buttonUnitList.Clear();
                scrollview.SetActive(false);
                ScrollViewContent.manager.Reset();
            }
            else if (!from.edges.Contains(n))
            {
                n.RedLight();
            }
            else if (to == null)
            {
                bool isAllAlliesMoved = true;
                to = n;
                foreach (Unit unit in buttonUnitList)
                {
                    unit.Move(from, to);
                    unit.movableLength--;
                    if (!unit.IsMove)
                        isAllAlliesMoved = false;
                }

                if (isAllAlliesMoved)
                {
                    from.GetComponent<SpriteRenderer>().color = originColor;
                    from = null;
                    to = null;
                    buttonUnitList.Clear();
                    scrollview.SetActive(false);
                    ScrollViewContent.manager.Reset();
                }
                else
                {
                    to.RedLight();
                    to = null;
                }
            }
        }
    }

    public void Ready()
    {
        isStateReady = false;
        endTurnButton.interactable = false;
        if (karma > 2)
        {
            foreach (Node node in enemySpawners)
            {
                enemies.Add(node.SpawnEnemy());
            }
        }

        EnemyMove();
    }

    public void EnemyMove()
    {
        isEnemyMoveDone = false;
        foreach (Unit enemy in enemies)
        {

            if (enemy.isOldOne)
            {
                for (int i = 0; i < enemy.movableLength;i++)
                {
                    Node nextNode = enemy.Position.edges[0];
                    foreach (Node n in enemy.Position.edges)
                    {
                        if (n.distance < nextNode.distance)
                        {
                            nextNode = n;
                        }
                        else if (nextNode.distance == n.distance)
                        {
                            int r = Random.Range(0, 1);
                            if (r == 0)
                                nextNode = n;
                        }
                    }
                    enemy.Move(enemy.Position, nextNode);
                }
            }
            else
                enemy.isOldOne = true;
        }
    }
    public void PlayerAction()
    {
        foreach(Unit n in allies)
        {
            n.movableLength = 2;
        }
        endTurnButton.interactable = true;
        isNodeClickable = true;

        isStateReady = true;
    }
    public void Fight()
    {
        foreach (Node n in allNodes)
        {
            if (n.allies.Count > 0 && n.enemies.Count > 0)
            {
                int allyAttack = 0;
                int enemyAttack = 0;
                List<Unit> destroyAlly = new List<Unit>();
                List<Unit> destroyEnemy = new List<Unit>();
                foreach (Unit ally in n.allies)
                {
                    allyAttack += ally.attck;
                }

                foreach (Unit enemy in n.enemies)
                {
                    enemyAttack += enemy.attck;
                    if (allyAttack < enemy.health)
                    {
                        enemy.health -= allyAttack;
                        allyAttack = 0;
                    }
                    else
                    {
                        allyAttack -= enemy.health;
                        destroyEnemy.Add(enemy);
                    }
                }
                foreach(Unit enemy in destroyEnemy)
                {
                    n.enemies.Remove(enemy);
                    Destroy(enemy.gameObject);
                }

                foreach(Unit ally in n.allies)
                {
                    if(enemyAttack < ally.health)
                    {
                        ally.health -= enemyAttack;
                        enemyAttack = 0;
                    }
                    else
                    {
                        enemyAttack -= ally.health;
                        destroyAlly.Add(ally);
                    }
                }
                foreach(Unit ally in destroyAlly)
                {
                    n.allies.Remove(ally);
                    Destroy(ally.gameObject);
                }
            }
            if (n.allies.Count > 0 && n.enemies.Count > 0)
            {
                n.isTerritory = 0;
                foreach(Unit enemy in n.enemies)
                {
                    enemy.movableLength = 0;
                }
            }
        }
        isStateReady = false;
        endTurnButton.interactable = false;


        EndPhase();
    }
    public void EndPhase()
    {
        karma += notoriety;
        spell += territories.Count;

        foreach (Node n in allNodes)
        {
            if (n.allies.Count > 0 && n.enemies.Count == 0)
            {
                n.isTerritory = 1;
                territories.Add(n);
            }
            else if (n.allies.Count == 0 && n.enemies.Count > 0)
            {
                n.isTerritory = -1;
                if (territories.Contains(n))
                    territories.Remove(n);
                foreach (Unit enemy in n.enemies)
                    enemy.movableLength = 2;
            }
            else if (n.allies.Count == 0 && n.enemies.Count == 0)
                n.isTerritory = 0;
        }

        endTurnButton.interactable = true;
        isStateReady = true;
    }
}
