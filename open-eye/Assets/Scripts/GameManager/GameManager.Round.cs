using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager
{

    public float phaseNoticeDuration;

    public Text PhaseAlertText;

    private bool isPhaseNoticeDone = false;

    public enum RoundState
    {
        Standby,
        EnemyMove,
        PlayerAction,
        Fight,
        Captive,
        Upkeep,
    }

    public RoundState currentState
    {
        get;
        private set;
    }

    [HideInInspector]
    public List<Unit> movingUnits = new List<Unit>();

    public bool isEnemyMoving
    {
        get
        {
            foreach (var unit in movingUnits)
            {
                if (!unit.isAlly)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool isAllyMoving
    {
        get
        {
            foreach (var unit in movingUnits)
            {
                if (unit.isAlly)
                {
                    return true;
                }
            }
            return false;
        }
    }

    private EnemySpawnDataContainer.EnemySpawnData nextSpawnData;

    IEnumerator AlertPhase()
    {
        isPhaseNoticeDone = false;
        switch ((int)currentState)
        {
            case 0:
                PhaseAlertText.text = "Standby";
                break;
            case 1:
                PhaseAlertText.text = "EnemyMove";
                break;
            case 2:
                PhaseAlertText.text = "PlayerAction";
                break;
            case 3:
                PhaseAlertText.text = "Fight";
                break;
            case 4:
                PhaseAlertText.text = "Captive";
                break;
            case 5:
                PhaseAlertText.text = "Upkeep";
                break;
        }
        yield return new WaitForSeconds(phaseNoticeDuration);
        PhaseAlertText.text = "";
        isPhaseNoticeDone = true;
    }

    IEnumerator ChangePhase()
    {
        yield return new WaitUntil(() => isPhaseNoticeDone);
        switch ((int)currentState)
        {
            case 0:
                EnemyMovePhase();
                break;
            case 1:
                PlayerActionPhase();
                break;
            case 2:
                FightPhase();
                break;
            case 3:
                Captive();
                break;
            case 4:
                UpkeepPhase();
                break;
            case 5:
                StandbyPhase();
                break;
        }
    }

    private void StandbyPhase()
    {
        currentState = RoundState.Standby;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        StartCoroutine(AlertPhase());

        CheckWin();
        CheckAndSpawnEnemy();

        StartCoroutine(ChangePhase());
    }

    private void EnemyMovePhase()
    {
        currentState = RoundState.EnemyMove;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        StartCoroutine(AlertPhase());

        MoveEnemy();
        OnEnemyMoveDone();
    }

    private void PlayerActionPhase()
    {
        currentState = RoundState.PlayerAction;
        endTurnButton.interactable = true;
        produceButton.interactable = true;

        StartCoroutine(AlertPhase());

        foreach (Node n in allNodes)
        {
            foreach (Unit ally in n.allies)
            {
                ally.Refresh();
            }
        }
    }

    private void FightPhase()
    {
        currentState = RoundState.Fight;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        StartCoroutine(AlertPhase());

        ResolveAllFight();

        StartCoroutine(ChangePhase());
    }

    private void Captive()
    {
        currentState = RoundState.Captive;
        endTurnButton.interactable = true;
        produceButton.interactable = false;

        destroyedEnemies = new List<Unit>();

        foreach (Node n in allNodes)
        {
            destroyedEnemies.AddRange(n.enemies.FindAll((unit) => unit.CurrentHealth == 0));
        }

        if (destroyedEnemies.Count > 0)
            endTurnButton.interactable = false;

        var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
        foreach (var g in unitScrollView.listItems)
        {
            g.GetComponent<Button>().interactable = false;
        }

        foreach (DestroyedEnemyControlButton button in destroyedEnemyControlButtons)
        {
            button.gameObject.SetActive(true);
        }

        unitListScrollView.ShowList(true);
        unitListScrollView.ShowUnitTab(false);
        destroyedEnemyControlUnit.SetActive(true);
    }

    private void UpkeepPhase()
    {
        currentState = RoundState.Upkeep;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        destroyedEnemyControlUnit.SetActive(false);
        destroyedEnemyControlButtons.ForEach((button) => button.Clear());
        
        //foreach (DestroyedEnemyControlButton button in destroyedEnemyControlButtons)
        //{
        //    button.gameObject.SetActive(false);
        //}

        foreach (var button in destroyedEnemyControlButtons)
        {
            button.Fetch();
        }

        foreach (Node n in allNodes)
        {
            n.FetchDestroy();
        }

        StartCoroutine(AlertPhase());

        CaptureTerritories();
        UpkeepResources();
        RefreshStatus();

        StartCoroutine(ChangePhase());
    }





    private void CheckWin()
    {
        int numberOfEnemies = 0;
        foreach (Node n in allNodes)
        {
            numberOfEnemies += n.enemies.Count;
        }
        if (nextSpawnData.enemyDatas == null && numberOfEnemies == 0)
        {
            //TODO : 승리 처리
        }
    }

    private void CheckAndSpawnEnemy()
    {
        if (karma > nextSpawnData.requiredKarma)
        {
            foreach (var enemyData in nextSpawnData.enemyDatas)
            {
                List<Node> spawners = new List<Node>();
                foreach (var enemySpawnNode in enemyData.enemySpawnNodes)
                {
                    foreach (Node spawner in enemySpawners)
                    {
                        string[] spawnerPosition = spawner.name.Split("_"[0]);
                        if (enemySpawnNode.CompareTo(spawnerPosition[0]) == 0)
                        {
                            spawners.Add(spawner);
                        }
                    }
                }
                string[] spawnData;
                spawnData = enemyData.enemyStatus.Split(" "[0]);
                string spawnName = spawnData[0];
                int howMuchSpawn = int.Parse(spawnData[1]);
                if (enemyData.requiredNotoriety <= notoriety)
                {
                    for (int i=0; i<howMuchSpawn; i++)
                    {
                        var spawnNode = spawners[Random.Range(0, spawners.Count)];
                        Unit enemy = Spawner.spawner.Spawn(AssetManager.Instance.GetUnitData(spawnName), false, spawnNode);
                        enemy.onMoveDone += OnEnemyMoveDone;
                    }
                }
            }
            nextSpawnData = config.enemySpawnDataContainer.GetNextEnemySpawnData(karma);
        }
    }

    private void MoveEnemy()
    {
        foreach (Node n in allNodes)
        {
            foreach (Unit enemy in n.enemies)
            {
                while (enemy.canMove)
                {
                    Node nextNode = enemy.NextNode();
                    enemy.MoveBetweenNodes(enemy.position, nextNode);
                }
            }
        }
    }

    private void OnEnemyMoveDone()
    {
        if (!isEnemyMoving)
        {
            StartCoroutine(ChangePhase());
        }
    }

    private void ResolveAllFight()
    {
        foreach (Node n in allNodes)
        {
            n.FetchFight(Fight.Fighting(n.units));
        }
    }

    private void CaptureTerritories()
    {
        foreach (Node n in allNodes)
        {
            if (n.isFighting)
            {
                continue;
            }

            if (n.enemies.Count > 0)
            {
                n.isPlayerTerritory = false;
                n.isNeutralTerritory = false;
            }

            if (n.allies.Count > 0)
            {
                n.isPlayerTerritory = true;
                n.isNeutralTerritory = false;
            }
        }
        manaAmountText.SetManaAmount(mana, manaProduce);
    }

    private void RefreshStatus()
    {
        foreach (Node n in territories)
        {
            if (!n.IsCastle)
            {
                n.GetComponent<SpriteRenderer>().sprite = n.allySprite;
            }
        }

        foreach (Node n in allNodes)
        {
            if (!n.isPlayerTerritory && !n.isNeutralTerritory)
            {
                n.GetComponent<SpriteRenderer>().sprite = n.enemySprite;
            }
            foreach (Unit enemy in n.enemies)
            {
                enemy.Refresh();
            }
        }
    }
}
