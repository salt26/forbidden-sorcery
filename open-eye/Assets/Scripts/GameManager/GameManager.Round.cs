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

        foreach (Node n in allNodes)
        {
            unitListScrollView.SetControlDestroyedEnemiesList(n.enemies.FindAll((unit) => unit.CurrentHealth == 0), OnSelectUnitForControlDestroyedEnemy);
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

        //foreach (DestroyedEnemyControlButton button in destroyedEnemyControlButtons)
        //{
        //    button.gameObject.SetActive(false);
        //}

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
            var spawners = enemySpawners;

            foreach (var enemyData in nextSpawnData.enemyDatas)
            {
                if (enemyData.requiredNotoriety <= notoriety)
                {
                    var spawnNode = spawners[Random.Range(0, spawners.Count)];

                    Unit enemy = Spawner.spawner.Spawn(AssetManager.Instance.GetUnitData("SampleEnemy"), false, spawnNode);
                    enemy.onMoveDone += OnEnemyMoveDone;
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
            n.DecideAndShowMainUnit();
            n.SetUnitPosition();
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
            }

            if (n.allies.Count > 0)
            {
                n.isPlayerTerritory = true;
            }

            if (!n.IsCastle)
            {
                n.GetComponent<SpriteRenderer>().color = Color.white;
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
                n.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        foreach (Node n in allNodes)
        {
            foreach (Unit enemy in n.enemies)
            {
                enemy.Refresh();
            }
        }
    }
}
