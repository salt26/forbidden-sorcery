using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager
{
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

    private int manaProduce
    {
        get
        {
            int produce = 0;
            foreach (var territory in territories)
            {
                produce += territory.manaValue;
            }
            return produce;
        }
    }

    private EnemySpawnDataContainer.EnemySpawnData nextSpawnData;



    private void StandbyPhase()
    {
        currentState = RoundState.Standby;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        CheckWin();
        CheckAndSpawnEnemy();

        EnemyMovePhase();
    }

    private void EnemyMovePhase()
    {
        currentState = RoundState.EnemyMove;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        MoveEnemy();

        OnEnemyMoveDone();
    }

    private void PlayerActionPhase()
    {
        currentState = RoundState.PlayerAction;
        endTurnButton.interactable = true;
        produceButton.interactable = true;

        foreach (Unit ally in allies)
        {
            ally.Refresh();
        }
    }

    private void FightPhase()
    {
        currentState = RoundState.Fight;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        ResolveAllFight();

        UpkeepPhase();
    }

    private void UpkeepPhase()
    {
        currentState = RoundState.Upkeep;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        CaptureTerritories();
        UpkeepResources();
        RefreshStatus();

        StandbyPhase();
    }





    private void CheckWin()
    {
        if (nextSpawnData.enemyDatas == null && enemies.Count == 0)
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

                    Unit enemy = spawnNode.Spawn(AssetManager.Instance.GetUnitData("SampleEnemy"), false);
                    enemy.onMoveDone += OnEnemyMoveDone;
                    enemies.Add(enemy);
                }
            }

            nextSpawnData = config.enemySpawnDataContainer.GetNextEnemySpawnData(karma);
        }
    }

    private void MoveEnemy()
    {
        foreach (Unit enemy in enemies)
        {
            while (enemy.canMove)
            {
                Node nextNode = enemy.position.edges[0];
                foreach (Node node in enemy.position.edges)
                {
                    if (node.distance < nextNode.distance)
                    {
                        nextNode = node;
                    }
                    else if (nextNode.distance == node.distance)
                    {
                        int r = Random.Range(0, 1);
                        if (r == 0)
                            nextNode = node;
                    }
                }
                enemy.Move(enemy.position, nextNode);
            }
        }
    }

    private void OnEnemyMoveDone()
    {
        if (!isEnemyMoving)
        {
            PlayerActionPhase();
        }
    }

    private void ResolveAllFight()
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
                    allyAttack += ally.unitData.attack;
                }

                foreach (Unit enemy in n.enemies)
                {
                    enemyAttack += enemy.unitData.attack;
                    if (allyAttack < enemy.currentHealth)
                    {
                        enemy.Damage(allyAttack);
                        allyAttack = 0;
                    }
                    else
                    {
                        allyAttack -= enemy.currentHealth;
                        n.destroyedEnemies.Add(enemy);
                        enemies.Remove(enemy);
                        destroyEnemy.Add(enemy);
                    }
                }
                foreach (Unit enemy in destroyEnemy)
                {
                    n.units.Remove(enemy);
                }

                foreach (Unit ally in n.allies)
                {
                    if (enemyAttack < ally.currentHealth)
                    {
                        ally.Damage(enemyAttack);
                        enemyAttack = 0;
                    }
                    else
                    {
                        enemyAttack -= ally.currentHealth;
                        destroyAlly.Add(ally);
                    }
                }
                foreach (Unit ally in destroyAlly)
                {
                    n.units.Remove(ally);
                    Destroy(ally.gameObject);
                }
            }
            if (n.allies.Count > 0 && n.enemies.Count > 0)
            {
                n.isPlayerTerritory = false;
            }
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

        foreach (Unit enemy in enemies)
        {
            enemy.Refresh();
        }
    }
}
