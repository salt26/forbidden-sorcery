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
    public int notoriety;//delta karma
    public int spell;
    public int karma;//accumulated notoriety
    public int enemySpawnBound = 2;
    bool isStateReady = false;
    public Node kingTower;
    public bool isPlayerActionTurn = true;
    bool isEnemyMoveDone = false;
    public string whichTurn = "a";
    public List<Node> enemySpawners = new List<Node>();
    public StatusText AHText;
    [HideInInspector]
    public List<Unit> allies = new List<Unit>();
    [SerializeField]
    List<Unit> enemies = new List<Unit>();
    [HideInInspector]
    public List<Node> territories = new List<Node>();
    [HideInInspector]
    public List<Node> allNodes = new List<Node>();
    public Button endTurnButton;
    public GameObject scrollview;
    public List<Unit> buttonUnitList = new List<Unit>();
    public List<UnitData> spawnUnitList = new List<UnitData>();
    [SerializeField]
    List<GameObject> destroyedUnitControlButton = new List<GameObject>();
    public List<UnitData> allyPrefab = new List<UnitData>();
    public List<UnitData> enemyPrefab = new List<UnitData>();
    public List<Button> destroyEnemySelectButtons = new List<Button>();
    [HideInInspector]
    public int destroyedEnemyCount = 0;

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
        //allies.Add(kingTower.SpawnAlly());
        Ready();
        state = 1;
        kingTower.SetDIstance(0);
        foreach (GameObject go in destroyedUnitControlButton)
            go.GetComponent<Button>().interactable = false;
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

        if (spawnUnitList.Count != 0 && spell > 0)
        {
            Unit unit = kingTower.Spawn(spawnUnitList[0]);
            allies.Add(unit);
            ScrollViewContent.scrollViewContent.AddComponent(unit, true);
            spell -= 1;
            spawnUnitList.Clear();
        }

        if (state == 0 && isStateReady && destroyedEnemyCount == 0 && !endTurnButton.interactable)
        {
            foreach (GameObject go in destroyedUnitControlButton)
                go.GetComponent<Button>().interactable = false;
            endTurnButton.interactable = true;
        }
    }

    public void OnClick()
    {
        if (spawnUnitList != null)
            spawnUnitList.Clear();
        AHText.stateAH(true, "", "", "");
        scrollview.SetActive(false);
        if (from != null && from.GetComponent<SpriteRenderer>().color != originColor)
            from.GetComponent<SpriteRenderer>().color = originColor;
        originColor = Color.white;
        from = null;
        to = null;
        if (state == 0 && isStateReady)
        {
            isPlayerActionTurn = true;
            Ready();
            state = 1;
        }
        else if (state == 1 && isStateReady)
        {
            isPlayerActionTurn = false;
            Fight();
            state = 0;
        }
    }

    public void SetNode(Node n)
    {
        if (isStateReady)
        {
            if (isPlayerActionTurn)
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

                if (from == null && (n.IsKing || n.allies.Count != 0 || n.enemies.Count != 0))
                {
                    from = n;
                    var spriteRenderer = from.GetComponent<SpriteRenderer>();
                    originColor = spriteRenderer.color;
                    spriteRenderer.color = Color.black;

                    scrollview.SetActive(true);
                    buttonUnitList.Clear();
                    ScrollViewContent.scrollViewContent.Reset();
                    if (n.IsKing)
                    {
                        for (int i = 0; i < allyPrefab.Count; i++)
                            ScrollViewContent.scrollViewContent.AddGenButton(allyPrefab[i]);
                    }
                    foreach (Unit u in n.allies)
                    {
                        ScrollViewContent.scrollViewContent.AddComponent(u, true);
                    }
                    foreach (Unit u in n.enemies)
                    {
                        ScrollViewContent.scrollViewContent.AddComponent(u, true);
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
                    ScrollViewContent.scrollViewContent.Reset();
                    scrollview.SetActive(false);
                }
                else if (!from.edges.Contains(n) && buttonUnitList.Count > 0)
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
                        if (to.enemies.Count > 0)
                            unit.movableLength = 0;
                        if (!unit.IsMove)
                            isAllAlliesMoved = false;
                    }

                    if (isAllAlliesMoved)
                    {
                        from.GetComponent<SpriteRenderer>().color = originColor;
                        from = null;
                        to = null;
                        buttonUnitList.Clear();
                        ScrollViewContent.scrollViewContent.Reset();
                        scrollview.SetActive(false);
                    }
                    else
                    {
                        to.RedLight();
                        to = null;
                    }
                }
            }


            else
            {
                originColor = n.GetComponent<SpriteRenderer>().color;
                if (from == n)
                {
                    from = null;
                    ScrollViewContent.scrollViewContent.Reset();
                    destroyEnemySelectButtons.Clear();
                    scrollview.SetActive(false);
                }
                else
                {
                    from = n;
                    scrollview.SetActive(true);
                    ScrollViewContent.scrollViewContent.Reset();
                    destroyEnemySelectButtons.Clear();
                    foreach (Unit unit in n.destroyedEnemies)
                    {
                        ScrollViewContent.scrollViewContent.AddComponent(unit, false);
                    }
                }
            }
        }
    }

    public void Ready()
    {
        isStateReady = false;
        endTurnButton.interactable = false;
        if (karma > enemySpawnBound)
        {
            enemySpawnBound += 2;
            foreach (Node node in enemySpawners)
            {
                enemies.Add(node.Spawn(enemyPrefab[Random.Range(0, enemyPrefab.Count)]));
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
                for (int i = 0; i < enemy.movableLength; i++)
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
                    if (nextNode.Equals(kingTower))
                        enemy.movableLength = 0;
                    if (nextNode.allies.Count > 0)
                        break;
                }
            }
            else
                enemy.isOldOne = true;
        }
    }
    public void PlayerAction()
    {
        foreach (Unit n in allies)
        {
            n.movableLength = n.staticMovableLength;
        }
        endTurnButton.interactable = true;
        whichTurn = "Move ally";
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
                n.destroyedEnemies.Clear();
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
                        n.destroyedEnemies.Add(enemy);
                        enemies.Remove(enemy);
                        destroyEnemy.Add(enemy);
                        destroyedEnemyCount++;
                    }
                }
                foreach (Unit enemy in destroyEnemy)
                {
                    n.enemies.Remove(enemy);
                }

                foreach (Unit ally in n.allies)
                {
                    if (enemyAttack < ally.health)
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
                foreach (Unit ally in destroyAlly)
                {
                    n.allies.Remove(ally);
                    Destroy(ally.gameObject);
                }
            }
            if (n.allies.Count > 0 && n.enemies.Count > 0)
            {
                n.isTerritory = 0;
                foreach (Unit enemy in n.enemies)
                {
                    enemy.movableLength = 0;
                }
            }
        }
        isStateReady = false;
        endTurnButton.interactable = false;
        if (destroyedEnemyCount > 0)
        {
            foreach (GameObject go in destroyedUnitControlButton)
                go.GetComponent<Button>().interactable = true;
        }
        whichTurn = "Fight";

        EndPhase();
    }

    public void EndPhase()
    {
        karma += notoriety;

        foreach (Node n in allNodes)
        {
            if (n.allies.Count > 0 && n.enemies.Count == 0)
            {
                n.isTerritory = 1;
                if (!territories.Contains(n))
                    territories.Add(n);//replace?
            }
            else if (n.allies.Count == 0 && n.enemies.Count > 0)
            {
                n.isTerritory = -1;
                if (territories.Contains(n))
                    territories.Remove(n);
                foreach (Unit enemy in n.enemies)
                    enemy.movableLength = enemy.staticMovableLength;
            }
            else if (n.allies.Count == 0 && n.enemies.Count == 0)
                n.isTerritory = 0;
            else
                n.isTerritory = 0;
        }
        foreach (Node n in allNodes)
        {
            if (!n.IsKing)
            {
                n.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        foreach (Node n in territories)
        {
            if (!n.IsKing)
            {
                n.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }

        spell += territories.Count;
        isStateReady = true;
    }
}
