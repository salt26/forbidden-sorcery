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

    public Sprite allySprite;
    public Sprite enemySprite;
    public Sprite neutralSprite;
    public Sprite devilKingCastleSprite;

    [SerializeField]
    private NodeType type;

    [SerializeField]
    public int manaValue;

    [SerializeField]
    public List<Node> edges;
    public List<Unit> startEnemies;

    private Vector3 centralStandingPosition, allyStandingPosition, enemyStandingPosition;

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
    public bool isNeutralTerritory = true;

    [HideInInspector]
    public int distance = int.MaxValue;

    [HideInInspector]
    static public bool nodeBeingClicked;

    [HideInInspector]
    public bool isChecked;

    public GameObject centralPositionIndicator, allyPositionIndicator, enemyPositionIndicator;

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
        GetComponent<Transform>().localScale = new Vector3(0.4f, 0.4f, 1f);
        centralStandingPosition = this.GetComponent<Transform>().position;
        allyStandingPosition = centralStandingPosition + allyPositionIndicator.transform.localPosition;
        enemyStandingPosition = centralStandingPosition + enemyPositionIndicator.transform.localPosition;

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

        foreach (Unit enemy in startEnemies)
        {
            enemies.Add(enemy);
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
            if (isCastle) GetComponent<SpriteRenderer>().sprite = devilKingCastleSprite;
            if (!isCastle) this.GetComponent<SpriteRenderer>().sprite = allySprite;
        }
        if (!isPlayerTerritory)
        {
            if (isNeutralTerritory) GetComponent<SpriteRenderer>().sprite = neutralSprite;
            else GetComponent<SpriteRenderer>().sprite = enemySprite;
        }
        GameManager.instance.allNodes.Add(this);
    }

    void OnMouseDown()
    {
        nodeBeingClicked = true;
    }

    void OnMouseUpAsButton()
    {
        GameManager.instance.SetNode(this);
    }

    public void DecideAndShowMainUnit()
    {
        bool decideAllyMainUnit = false, decideEnemyMainUnit = false;

        foreach (Unit unit in units)
        {
            if (!decideAllyMainUnit && unit.isAlly)
            {
                unit.mainUnitInNode = true;
                decideAllyMainUnit = true;
            }
            else if (!decideEnemyMainUnit && !unit.isAlly)
            {
                unit.mainUnitInNode = true;
                decideEnemyMainUnit = true;
            }
            else
            {
                unit.mainUnitInNode = false;
            }
        }

        foreach (Unit unit in units)
        {
            if (unit.mainUnitInNode)
            {
                unit.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                unit.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    //public static void RefineUnitPositionInAllNodes()
    //{
    //    foreach (Node n in GameManager.instance.allNodes)
    //    {
    //        if(n.unitMovedThisTurn)
    //        {
    //            n.unitMovedThisTurn = false;
    //            n.RefineUnitPosition();
    //        }
    //    }
    //}

    public void RefineUnitPosition(int allyNumber, int enemyNumber)//, Unit u = null)
    {
        //if (GameManager.instance.currentState == GameManager.RoundState.PlayerAction)
        //GameManager.instance.EndTurnButton.interactable = false;
        //List<Unit> re = new List<Unit>();

        if (allyNumber > 0 && enemyNumber > 0)
        {
            foreach (Unit unit in units)
            {
                if (unit.isAlly)
                {
                    unit.moveQueue.Enqueue(unit.MoveInNodeAnimation(allyStandingPosition));
                    unit.transform.SetParent(allyPositionIndicator.GetComponent<Transform>());
                }
                else
                {
                    unit.moveQueue.Enqueue(unit.MoveInNodeAnimation(enemyStandingPosition));
                    unit.transform.SetParent(enemyPositionIndicator.GetComponent<Transform>());
                }
                //re.Add(unit);
            }
        }
        else
        {
            foreach (Unit unit in units)
            {
                unit.moveQueue.Enqueue(unit.MoveInNodeAnimation(centralStandingPosition));
                unit.transform.SetParent(centralPositionIndicator.GetComponent<Transform>());
                //re.Add(unit);
            }
        }



        //if (u != null)
        //{
        //    if (allyNumber > 0 && enemyNumber > 0)
        //    {
        //        if (u.isAlly)
        //        {
        //            u.moveQueue.Enqueue(u.MoveInNodeAnimation(allyStandingPosition));
        //            u.transform.SetParent(allyPositionIndicator.GetComponent<Transform>());
        //        }
        //        else
        //        {
        //            u.moveQueue.Enqueue(u.MoveInNodeAnimation(enemyStandingPosition));
        //            u.transform.SetParent(enemyPositionIndicator.GetComponent<Transform>());
        //        }
        //        re.Add(u);
        //    }
        //    else
        //    {
        //        u.moveQueue.Enqueue(u.MoveInNodeAnimation(centralStandingPosition));
        //        u.transform.SetParent(centralPositionIndicator.GetComponent<Transform>());
        //        re.Add(u);
        //    }
        //}
        //return re;
    }

    public void RedLight()
    {
        if (isRed == false)
        {
            StartCoroutine(RedLightAnimation());
        }
    }

    //public Unit Spawn(UnitData unitData, bool isAlly)
    //{
    //    var unitObject = Instantiate(AssetManager.Instance.GetPrefab("Unit"));
    //    var unit = unitObject.GetComponent<Unit>();
    //    unit.SetUnit(unitData);
    //    unit.isAlly = isAlly;

    //    unit.transform.localPosition = this.transform.localPosition;
    //    unit.position = this;

    //    units.Add(unit);

    //    return unit;
    //}

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

    public void FetchFight(ExpectedFightResult EFR)
    {
        foreach (IUnitInterface ui in EFR.unitList)
        {
            foreach (Unit u in units)
            {
                if (ui.ID == u.ID)
                {
                    u.Movement = ui.Movement;
                    u.CurrentHealth = ui.CurrentHealth;
                }
            }
        }
    }

    public void FetchDestroy()
    {
        List<Unit> tempU = units.FindAll((unit) => unit.CurrentHealth == 0);
        units.RemoveAll((unit) => unit.CurrentHealth == 0);
        foreach (Unit u in tempU)
        {
            if (u.isAlly)
            {
                bool isActivated = false;
                UnitData unitData = null;
                foreach(var unitD in GameManager.instance.producedAlliedEnemies)
                {
                    if (unitD.Equals(u.unitData))
                    {
                        isActivated = true;
                        unitData = unitD;
                        break;
                    }
                }
                if (isActivated)
                    GameManager.instance.producedAlliedEnemies.Remove(unitData);
            }
            Destroy(u.gameObject);
        }
        DecideAndShowMainUnit();
        RefineUnitPosition(allies.Count, enemies.Count);
        foreach (Unit unit in units)
        {
            if (unit.moveQueue.Count > 0 && !unit.IsMoving)
                StartCoroutine(unit.moveQueue.Dequeue());
        }
    }
}
