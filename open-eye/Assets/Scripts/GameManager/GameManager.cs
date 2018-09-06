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
    public Button EndTurnButton { get { return endTurnButton; } }
    [SerializeField]
    private Button produceButton;
    public Button ProduceButton { get { return produceButton; } }
    [SerializeField]
    public UnitListScrollView unitListScrollView;
    [HideInInspector]
    public DestroyedEnemyControlScrollView destroyedEnemyControlScrollView;
    private DestroyedEnemyControlScrollView pastDestroyedEnemyControlScrollView;
    private List<Unit> selectedUnitList = new List<Unit>();
    private Node selectedNode = null;
    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color unSelectedColor;
    private List<Unit> destroyedEnemies;
    private List<Unit> selectedDestroyedEnemyList;
    [SerializeField]
    private List<DestroyedEnemyControlButton> destroyedEnemyControlButtons = new List<DestroyedEnemyControlButton>();
    [SerializeField]
    private List<Button> destroyedEnemyControlResetButtons = new List<Button>();
    [SerializeField]
    private GameObject destroyedEnemyControlUnit;
    [HideInInspector]
    public List<UnitData> producedAlliedEnemies = new List<UnitData>();
    [HideInInspector]
    public Dictionary<UnitData, int> numberOfProducableAlliedEnemies = new Dictionary<UnitData, int>();
    [SerializeField]
    public GameObject phaseAlertText;
    [SerializeField]
    public Map map;
    [HideInInspector]
    private bool isLast = false;
    [HideInInspector]
    public Node rallyPoint;
    [SerializeField]
    public Color rallyPointColor;
    public GameObject nodeFightStatusDefault;
    [SerializeField]
    public GameEnd gameEnd;
    [HideInInspector]
    public bool isLose = false;

    public List<Unit> SelectedDestroyedEnemyList
    {
        get
        {
            return selectedDestroyedEnemyList;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        selectedDestroyedEnemyList = new List<Unit>();

        StartCoroutine(Initialize());
    }

    private void InitializeGame()
    {
        nextSpawnData = config.enemySpawnDataContainer.GetNextEnemySpawnData(karma);
    }
}
