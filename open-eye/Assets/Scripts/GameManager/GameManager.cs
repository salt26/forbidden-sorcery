using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("게임데이터")]
    [SerializeField]
    private GameConfig config;
    
    [Header("UI")]
    [SerializeField]
    private Button endTurnButton;

    [SerializeField]
    private GameObject scrollViewObject;

    [Header("정리중")]
    public List<Unit> buttonUnitList = new List<Unit>();
    public List<UnitData> spawnUnitList = new List<UnitData>();
    public List<UnitData> allyPrefab = new List<UnitData>();
    public List<Button> destroyEnemySelectButtons = new List<Button>();

    private Node from = null;
    private Node to = null;
    private Color originColor = Color.white;

    private List<Unit> allies = new List<Unit>();
    private List<Unit> enemies = new List<Unit>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeMap();
        InitializeResource();

        nextSpawnData = config.enemySpawnDataContainer.GetNextEnemySpawnData(karma);

        StandbyPhase();
    }

    private void FixedUpdate()
    {
        if (spawnUnitList.Count != 0 && mana > 0)
        {
            Unit unit = castle.Spawn(spawnUnitList[0], true);
            allies.Add(unit);
            ScrollViewContent.scrollViewContent.AddComponent(unit, true);
            mana -= 1;
            spawnUnitList.Clear();
        }
    }

    public void OnClickEndTurnButton()
    {
        if (spawnUnitList != null)
            spawnUnitList.Clear();
        scrollViewObject.SetActive(false);
        if (from != null && from.GetComponent<SpriteRenderer>().color != originColor)
            from.GetComponent<SpriteRenderer>().color = originColor;
        originColor = Color.white;
        from = null;
        to = null;
        if (currentState == RoundState.Captive)
        {
            UpkeepPhase();
        }
        else if (currentState == RoundState.PlayerAction)
        {
            FightPhase();
        }
    }

    public void SetNode(Node node)
    {
        if (isNodeInputActive)
        {
            if (currentState == RoundState.PlayerAction)
            {
                if (from == null && (node.IsCastle || node.allies.Count != 0 || node.enemies.Count != 0))
                {
                    from = node;
                    var spriteRenderer = from.GetComponent<SpriteRenderer>();
                    originColor = spriteRenderer.color;
                    spriteRenderer.color = Color.black;

                    scrollViewObject.SetActive(true);
                    buttonUnitList.Clear();
                    ScrollViewContent.scrollViewContent.Reset();
                    if (node.IsCastle)
                    {
                        for (int i = 0; i < allyPrefab.Count; i++)
                            ScrollViewContent.scrollViewContent.AddGenButton(allyPrefab[i]);
                    }
                    foreach (Unit u in node.allies)
                    {
                        ScrollViewContent.scrollViewContent.AddComponent(u, true);
                    }
                    foreach (Unit u in node.enemies)
                    {
                        ScrollViewContent.scrollViewContent.AddComponent(u, true);
                    }
                }

                else if (from == null)
                {
                    node.RedLight();
                }
                else if (from == node)
                {
                    from.GetComponent<SpriteRenderer>().color = originColor;
                    from = null;

                    buttonUnitList.Clear();
                    ScrollViewContent.scrollViewContent.Reset();
                    scrollViewObject.SetActive(false);
                }
                else if (!from.edges.Contains(node) && buttonUnitList.Count > 0)
                {
                    node.RedLight();
                }
                else if (to == null)
                {
                    to = node;
                    foreach (Unit unit in buttonUnitList)
                    {
                        unit.Move(from, to);
                    }

                    if (isAllyMoving)
                    {
                        to.RedLight(); from.GetComponent<SpriteRenderer>().color = originColor;
                    }
                    else
                    {
                        from.GetComponent<SpriteRenderer>().color = originColor;
                    }

                    from = null;
                    to = null;
                    buttonUnitList.Clear();
                    ScrollViewContent.scrollViewContent.Reset();
                    scrollViewObject.SetActive(false);

                }
            }


            else
            {
                originColor = node.GetComponent<SpriteRenderer>().color;
                if (from == node)
                {
                    from = null;
                    ScrollViewContent.scrollViewContent.Reset();
                    destroyEnemySelectButtons.Clear();
                    scrollViewObject.SetActive(false);
                }
                else
                {
                    from = node;
                    scrollViewObject.SetActive(true);
                    ScrollViewContent.scrollViewContent.Reset();
                    destroyEnemySelectButtons.Clear();
                    foreach (Unit unit in node.destroyedEnemies)
                    {
                        ScrollViewContent.scrollViewContent.AddComponent(unit, false);
                    }
                }
            }
        }
    }
}
