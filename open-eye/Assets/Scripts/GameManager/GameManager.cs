using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("게임데이터")]
    [SerializeField]
    public GameConfig config;
    
    [Header("UI")]
    [SerializeField]
    private Button endTurnButton;

    [SerializeField]
    private Button produceButton;
    
    [SerializeField]
    private UnitListScrollView unitListScrollView;

    public DestroyedEnemyControlScrollView destroyedEnemyControlScrollView;

    private List<Unit> selectedUnitList = new List<Unit>();

    private Node selectedNode = null;
    private Color originColor = Color.white;

    private List<Unit> destroyedEnemies;
    private List<Unit> selectedDestroyedEnemyList;
    [SerializeField]
    private List<DestroyedEnemyControlButton> destroyedEnemyControlButtons = new List<DestroyedEnemyControlButton>();
    [SerializeField]
    private List<Button> destroyedEnemyControlResetButtons = new List<Button>();
    [SerializeField]
    private GameObject destroyedEnemyControlUnit;
    public List<Unit> SelectedDestroyedEnemyList
    {
        get
        {
            return selectedDestroyedEnemyList;
        }
    }
    
    public IEnumerator DelayTime (float second)
    {
        yield return new WaitForSeconds(second);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        destroyedEnemyControlUnit.SetActive(false);
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
