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
    private UnitListScrollView unitListScrollView;

    public DestroyedEnemyControlScrollView destroyedEnemyControlScrollView;
    private DestroyedEnemyControlScrollView pastDestroyedEnemyControlScrollView;

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
    [SerializeField]
    private GameObject destroyedEnemyControlButton;
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
