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
    private Button produceButton;

    [SerializeField]
    private UnitListScrollView unitListScrollView;
    
    private List<Unit> selectedUnitList = new List<Unit>();

    private Node selectedNode = null;
    private Color originColor = Color.white;

    private List<Unit> allies = new List<Unit>();
    private List<Unit> enemies = new List<Unit>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeInput();
        InitializeMap();
        InitializeResource();
        InitializeGame();

        StandbyPhase();
    }

    private void InitializeGame()
    {
        nextSpawnData = config.enemySpawnDataContainer.GetNextEnemySpawnData(karma);
    }
}
