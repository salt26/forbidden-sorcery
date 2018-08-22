using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    private Node castle;

    [HideInInspector]
    public List<Node> allNodes = new List<Node>();
    private List<Node> territories
    {
        get
        {
            return allNodes.FindAll((node) => node.isPlayerTerritory);
        }
    }
    private List<Node> enemySpawners
    {
        get
        {
            return allNodes.FindAll((node) => node.isEnemySpawner);
        }
    }

    public void SetCastle(Node castle)
    {
        this.castle = castle;
    }

    private void InitializeMap()
    {
        castle.SetDIstance(0);
        foreach (var enemyData in config.enemyStartSpawnDataContainer.enemySpawnDatas)
        {
            string[] spawnStatus = enemyData.enemyStatus.Split(" "[0]);
            string spawnName = spawnStatus[0];
            int number = int.Parse(spawnStatus[1]);
            Node spawnNode = null;
            foreach (var enemySpawnNode in enemyData.enemySpawnNodes)
            {
                foreach (Node node in allNodes)
                {
                    string[] spawnNodeName = node.name.Split("_"[0]);
                    if (enemySpawnNode == spawnNodeName[0])
                    {
                        spawnNode = node;
                    }
                }
            }
            for (int i=0; i<number; i++)
            {
                Unit enemy = Spawner.spawner.Spawn(AssetManager.Instance.GetUnitData(spawnName), false, spawnNode);
                enemy.onMoveDone += OnEnemyMoveDone;
            }
        }
    }
}
