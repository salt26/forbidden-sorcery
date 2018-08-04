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

    public void Standby()
    {
        currentState = RoundState.Standby;
        endTurnButton.interactable = false
            ;
        if (karma > enemySpawnBound)
        {
            enemySpawnBound += 2;
            foreach (Node node in enemySpawners)
            {
                Unit enemy = node.Spawn(AssetManager.Instance.GetUnitData("SampleEnemy"), false);
                enemy.onMoveDone += OnEnemyMoveDone;
                enemies.Add(enemy);
            }
        }

        EnemyMove();
    }

    public void EnemyMove()
    {
        currentState = RoundState.EnemyMove;
        endTurnButton.interactable = false;

        foreach (Unit enemy in enemies)
        {
            while (enemy.canMove)
            {
                Node nextNode = enemy.position.edges[0];
                foreach (Node n in enemy.position.edges)
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
                enemy.Move(enemy.position, nextNode);
            }
        }

        OnEnemyMoveDone();
    }

    private void OnEnemyMoveDone()
    {
        if (!isEnemyMoving)
        {
            PlayerAction();
        }
    }

    public void PlayerAction()
    {
        currentState = RoundState.PlayerAction;
        endTurnButton.interactable = true;

        foreach (Unit ally in allies)
        {
            ally.Refresh();
        }
    }

    public void Fight()
    {
        currentState = RoundState.Fight;
        endTurnButton.interactable = false;

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
                        destroyedEnemyCount++;
                    }
                }
                foreach (Unit enemy in destroyEnemy)
                {
                    n.enemies.Remove(enemy);
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
                    n.allies.Remove(ally);
                    Destroy(ally.gameObject);
                }
            }
            if (n.allies.Count > 0 && n.enemies.Count > 0)
            {
                n.isPlayerTerritory = false;
            }
        }

        Upkeep();
    }

    public void Upkeep()
    {
        currentState = RoundState.Upkeep;
        endTurnButton.interactable = false;

        karma += notoriety;

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

        mana += territories.Count;

        Standby();
    }
}
