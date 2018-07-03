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
    private int notoriety;
    private int spell;
    private int karma;
    bool isStateReady = false;
    public Node kingTower;
    //public int isAllMoved = 0;
    //public int haveToMove = 0;
    bool isNodeClickable;
    bool isEnemyMoveDone = false;
    public List<Node> enemySpawners = new List<Node>();
    List<Unit> allies = new List<Unit>();
    List<Unit> enemies = new List<Unit>();
    public Button endTurnButton;

    private void Awake()
    {
        state = 0;
        karma = 0;
        notoriety = 1;
        manager = this;
    }

    private void Start()
    {
        allies.Add(kingTower.SpawnAlly());
        Ready();
        state = 1;
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

    }

    public void OnClick()
    {
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
                bool isAllAlliesMoved = true;
                to = n;
                foreach (var unit in from.allies)
                {
                    unit.Move(from, to);
                    if (!unit.IsMove)
                        isAllAlliesMoved = false;
                }

                if (isAllAlliesMoved)
                {
                    from.GetComponent<SpriteRenderer>().color = originColor;
                    from = null;
                    to = null;
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
        foreach(Unit enemy in enemies)
        {
            enemy.movableLength = 2;
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
                //haveToMove += enemy.movableLength;
                for (;0 < enemy.movableLength;)
                {
                    Debug.Log(enemy.Position);
                    enemy.Move(enemy.Position, enemy.Position.edges[Random.Range(0, enemy.Position.edges.Count)]);
                }
            }
            else
                enemy.isOldOne = true;
        }
        /*
        if (haveToMove == 0)
        {
            isAllMoved = 0;
            haveToMove = 0;
            PlayerAction();
        }
        */
    }
    public void PlayerAction()
    {
        endTurnButton.interactable = true;
        isNodeClickable = true;

        isStateReady = true;
    }
    public void Fight()
    {

        isStateReady = false;
        endTurnButton.interactable = false;


        EndPhase();
    }
    public void EndPhase()
    {
        karma += notoriety;


        endTurnButton.interactable = true;
        isStateReady = true;
    }
}
