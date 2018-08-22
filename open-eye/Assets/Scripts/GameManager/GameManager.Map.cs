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
        SpawnStartEnemyUnit();
    }
}
