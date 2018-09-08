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
        GameObject rallyPointFlag = GameObject.Find("RallyPointFlag");
        castle.SetDIstance(0);
        Vector3 rallyPointFlagPosition = new Vector3(0f, -0.8f, 0f);
        rallyPointFlag.GetComponent<Transform>().position = castle.GetComponent<Transform>().position + rallyPointFlagPosition;
        SpawnStartUnit();
    }
}
