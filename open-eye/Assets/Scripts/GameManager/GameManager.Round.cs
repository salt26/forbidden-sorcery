﻿using System.Collections;
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
        AutoMove,
        Fight,
        Captive,
        Upkeep,
    }

    public RoundState currentState
    {
        get;
        private set;
    }

    public float enemySpawnDuration;

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

    [HideInInspector]
    public const int indexMax = 300; 

    [HideInInspector]
    public int fightingNodeNumber = 0;

    public float fightAnimationDuration;

    [HideInInspector]
    public List<int> publicAllyAssassinAttack = new List<int>();
    [HideInInspector]
    public List<int> publicAllyAttack = new List<int>();
    [HideInInspector]
    public List<int> publicAllyMageAttack = new List<int>();
    [HideInInspector]
    public List<int> publicEnemyAssassinAttack = new List<int>();
    [HideInInspector]
    public List<int> publicEnemyAttack = new List<int>();
    [HideInInspector]
    public List<int> publicEnemyMageAttack = new List<int>();

    public EnemySpawnDataContainer.EnemySpawnData nextSpawnData;

    IEnumerator ChangePhase()
    {
        yield return new WaitUntil(() => GameObject.Find("PhaseAlertText").GetComponent<PhaseAlertText>().isPhaseNoticeDone);
        switch ((int)currentState)
        {
            case 0:
                yield return new WaitUntil(() => isSpawnEnemyAnimationFinished);
                EnemyMovePhase();
                break;
            case 1:
                PlayerActionPhase();
                break;
            case 2:
                AutoMovePhase();
                break;
            case 3:
                FightPhase();
                break;
            case 4:
                Captive();
                break;
            case 5:
                UpkeepPhase();
                break;
            case 6:
                StandbyPhase();
                break;
        }
    }

    IEnumerator Initialize()
    {
        yield return new WaitUntil(() => allNodes.Count >= 61);
        InitializeInput();
        InitializeMap();
        InitializeGame();
        InitializeResource();

        initializePublicLists();

        FightAnimationUI.isPastFightAnimationFinished[0] = true;

        StandbyPhase();
    }

    void initializePublicLists()
    {
        for(int i = 1; i <= indexMax; i++)
        {
            publicAllyAssassinAttack.Add(0);
            publicAllyAttack.Add(0);
            publicAllyMageAttack.Add(0);
            publicEnemyAssassinAttack.Add(0);
            publicEnemyAttack.Add(0);
            publicEnemyMageAttack.Add(0);
        }
    }

    private void StandbyPhase()
    {
        currentState = RoundState.Standby;
        endTurnButton.interactable = false;
        produceButton.interactable = false;
        
        StartCoroutine(phaseAlertText.GetComponent<PhaseAlertText>().AlertPhase());

        CheckWin();
        CheckLose();
        isSpawnEnemyAnimationFinished = false;
        StartCoroutine(CheckAndSpawnEnemy());
        if (isLast)
        {
            AllEnemyAttack();
        }
        StartCoroutine(ChangePhase());
    }

    private void EnemyMovePhase()
    {
        currentState = RoundState.EnemyMove;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        StartCoroutine(phaseAlertText.GetComponent<PhaseAlertText>().AlertPhase());

        MoveEnemy();
        foreach(Node node in allNodes)
        {
            node.ShowNodeInformation();
        }
        OnEnemyMoveDone();
        
    }

    private void PlayerActionPhase()
    {
        currentState = RoundState.PlayerAction;

        StartCoroutine(phaseAlertText.GetComponent<PhaseAlertText>().AlertPhase());

        foreach (Node n in allNodes)
        {
            foreach (Unit ally in n.allies)
            {
                ally.Refresh();
            }
        }
    }

    private void AutoMovePhase()
    {
        currentState = RoundState.AutoMove;
        endTurnButton.interactable = false;
        produceButton.interactable = false;
        MoveAutoUnitToRallyPoint();
        StartCoroutine(waitUntilAllyIsNotMoving());
    }

    IEnumerator FinishFightPhase()
    {
        yield return new WaitUntil(() => FightAnimationUI.isPastFightAnimationFinished[fightingNodeNumber]);
        StartCoroutine(ChangePhase());
    }

    private void FightPhase()
    {
        foreach(Node n in allNodes)
        {
            n.GetComponent<SpriteRenderer>().color = unSelectedColor;
        }
        currentState = RoundState.Fight;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        StartCoroutine(phaseAlertText.GetComponent<PhaseAlertText>().AlertPhase());

        fightingNodeNumber = 0;

        ResolveAllFight();

        foreach (Node node in allNodes)
        {
            node.ShowNodeInformation();
        }

        for (int i = 1; i <= fightingNodeNumber; i++)
        {
            FightAnimationUI.isPastFightAnimationFinished[i] = false;
        }

        for (int i = 1; i <= fightingNodeNumber; i++)
        {
            StartCoroutine(FightAnimationUI.FightAnimation(i));      
        }

        StartCoroutine(FinishFightPhase());
    }

    private void Captive()
    {
        foreach (var v in destroyedEnemyControlButtons)
            v.GetComponent<DestroyedEnemyControlScrollView>().ClearItem();
        currentState = RoundState.Captive;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        destroyedEnemies = new List<Unit>();

        foreach (Node n in allNodes)
        {
            destroyedEnemies.AddRange(n.enemies.FindAll((unit) => unit.CurrentHealth == 0));
        }

        if (destroyedEnemies.Count > 0)
        {
            endTurnButton.interactable = false;

            var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
            foreach (var g in unitScrollView.listItems)
            {
                g.GetComponent<Button>().interactable = false;
            }

            foreach (DestroyedEnemyControlButton button in destroyedEnemyControlButtons)
            {
                //
                button.GetComponent<Button>().interactable = true;
                button.GetComponent<Image>().color = button.normalColor;
                button.isSelected = false;
                //
                button.gameObject.SetActive(true);
            }

            unitListScrollView.ShowList(true);
            unitListScrollView.ShowUnitTab(false);
            destroyedEnemyControlUnit.SetActive(true);
        }
        else
        {
            OnClickEndTurnButtonNoSound();
        }
    }

    private void UpkeepPhase()
    {
        currentState = RoundState.Upkeep;
        endTurnButton.interactable = false;
        produceButton.interactable = false;

        foreach (var button in destroyedEnemyControlButtons)
        {
            button.Fetch();
            button.dominateManaChange = 0;
            button.dominateNotorietychange = 0;
            button.killManaChange = 0;
            button.killNotorietyChange = 0;
            button.freeManaChange = 0;
            button.freeNotorietyChange = 0;
            button.ShowExpectedResourceChange();
        }

        destroyedEnemyControlUnit.SetActive(false);
        destroyedEnemyControlButtons.ForEach((button) => button.Clear());

        //foreach (DestroyedEnemyControlButton button in destroyedEnemyControlButtons)
        //{
        //    button.gameObject.SetActive(false);
        //}
        
        Color defaultColor = new Color32(25, 26, 54, 255);
        GameObject.Find("TurnLeft").GetComponent<Text>().color = defaultColor;


        foreach (Node n in allNodes)
        {
            n.FetchDestroy();
        }
        
        CaptureTerritories();
        UpkeepResources();
        RefreshStatus();

        StartCoroutine(ChangePhase());
    }
    
    private void CheckLose()
    {
        if (!castle.isPlayerTerritory)
        {
            isLose = true;
            gameEnd.enabled = true;
            gameEnd.ShowVictoryOrLose(false);
            produceButton.enabled = false;
            endTurnButton.enabled = false;
        }
    }
    private void CheckWin()
    {
        int numberOfEnemies = 0;
        foreach (Node n in allNodes)
        {
            foreach (Unit enemy in n.enemies) {
                if (enemy.currentMoveType != Unit.MoveType.stay && enemy.currentMoveType != Unit.MoveType.cover)
                {
                    numberOfEnemies++;
                }
            }
        }
        if (nextSpawnData.enemyDatas == null && numberOfEnemies == 0)
        {
            gameEnd.enabled = true;
            gameEnd.ShowVictoryOrLose(true);
            produceButton.enabled = false;
            endTurnButton.enabled = false;
        }
    }

    bool isSpawnEnemyAnimationFinished = false;

    private IEnumerator CheckAndSpawnEnemy()
    {
        Vector3 tempvector = GameObject.Find("Main Camera").GetComponent<Transform>().position;
        if (karma > nextSpawnData.requiredKarma)
        {
            string[] enemySpawnNodes = new string[100];
            int index = 0;

            foreach (var enemyData in nextSpawnData.enemyDatas)
            {
                foreach (var enemySpawnNode in enemyData.enemySpawnNodes)
                {
                    if (index == 0)
                    {
                        index = 1;
                        enemySpawnNodes[index] = enemySpawnNode;
                    }
                    for (int i = 1; i <= index; i++)
                    {
                        if (enemySpawnNode == enemySpawnNodes[i]) break;
                        if (i == index)
                        {
                            index++;
                            enemySpawnNodes[index] = enemySpawnNode;
                        }
                    }
                }
            }
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
                string[] spawnData = enemyData.enemyStatus.Split(" "[0]);
                string spawnName = spawnData[0];
                string moveType = spawnData[1];
                int howMuchSpawn = int.Parse(spawnData[2]);
                if (enemyData.requiredNotoriety <= notoriety)
                {
                    for (int i = 0; i < howMuchSpawn; i++)
                    {
                        var spawnNode = spawners[Random.Range(0, spawners.Count)];
                        Unit enemy = Spawner.spawner.Spawn(AssetManager.Instance.GetUnitData(spawnName), false, spawnNode);
                        enemy.onMoveDone += OnEnemyMoveDone;
                        switch (moveType)
                        {
                            case "S":
                                enemy.currentMoveType = Unit.MoveType.stay;
                                break;
                            case "D":
                                enemy.currentMoveType = Unit.MoveType.directToCastle;
                                break;
                            case "C":
                                enemy.currentMoveType = Unit.MoveType.cover;
                                break;
                            case "N":
                                enemy.currentMoveType = Unit.MoveType.nearTerritory;
                                break;
                            case "M":
                                enemy.currentMoveType = Unit.MoveType.moveToSelectedNode;
                                foreach (Node node in allNodes)
                                {
                                    string[] s = node.name.Split("_"[0]);
                                    if (s[0] == spawnData[3])
                                    {
                                        enemy.targetNode = node;
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            if (config.enemySpawnDataContainer.GetNextEnemySpawnData(karma) != null)
            {
                nextSpawnData = config.enemySpawnDataContainer.GetNextEnemySpawnData(karma);
            }
            else
            {
                nextSpawnData = null;
                isLast = true;
            }

            for (int i = 1; i <= index; i++)
            {
                EnemySpawnAlertManager.instance.PlaySound();
                GameObject.Find("Main Camera").GetComponent<CameraController>().SetDestination(
                GameObject.Find(enemySpawnNodes[i]).GetComponent<Transform>().position);
                yield return new WaitForSeconds(enemySpawnDuration);
            }

            // EnemySpawnAlertManager.instance.PlaySound();
        }
        isSpawnEnemyAnimationFinished = true;
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetDestination(tempvector);
        GameObject.Find("NextEnemySpawnKarma").GetComponent<NextSpawnTurn>().CalculateNextTurn(notoriety);
        yield return null;
    }

    private void MoveEnemy()
    {
        foreach (Node n in allNodes)
        {
            foreach (Unit enemy in n.enemies)
            {
                while (enemy.canMove)
                {
                    bool flag = false;
                    Node nextNode = enemy.NextNode();
                    if (nextNode.Equals(enemy.position))
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        break;
                    }
                    else
                    {
                        enemy.MoveBetweenNodes(enemy.position, nextNode);
                    }
                    
                }
            }
        }
        foreach(Node n in allNodes)
        {
            n.RefineUnitPosition(n.allies.Count, n.enemies.Count);
            //n.DecideAndShowMainUnit();
            foreach (Unit unit in n.units)
            {
                if (unit.moveQueue.Count > 0 && !unit.IsMoving)
                    StartCoroutine(unit.moveQueue.Dequeue());
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
            FightAnimationUI.nodeName[fightingNodeNumber + 1] = n.name;

            n.FetchFight(Fight.Fighting(n.units.ConvertAll<IUnitInterface>((unitUnit) => unitUnit as IUnitInterface)));
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
                if (n.isPlayerTerritory)
                {
                    notoriety -= n.notoriety;
                }
                n.isPlayerTerritory = false;
                n.isNeutralTerritory = false;
                           
            }

            if (n.allies.Count > 0)
            {
                if (!n.isPlayerTerritory)
                {
                    notoriety += n.notoriety;
                }
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
                n.GetComponent<SpriteRenderer>().sprite = map.allySprite;
            }
            // if(n.IsCastle)
        }

        foreach (Node n in allNodes)
        {
            if (!n.isPlayerTerritory && !n.isNeutralTerritory)
            {
                n.GetComponent<SpriteRenderer>().sprite = map.enemySprite;
            }
            foreach (Unit enemy in n.enemies)
            {
                enemy.Refresh();
            }
        }

        foreach (Node n in allNodes)
        {
            n.GetComponentInChildren<ShowNodeFightStatus>().ShowExpectedFightResult();
        }
    }

    public void SpawnStartUnit()
    {
        foreach (Node node in allNodes)
        {
            foreach (string enemyData in node.startEnemies)
            {
                string[] spawnStatus = enemyData.Split(" "[0]);
                string spawnName = spawnStatus[0];
                string moveType = spawnStatus[1];
                int number = int.Parse(spawnStatus[2]);
                
                for (int i = 0; i < number; i++)
                {
                    Unit enemy = Spawner.spawner.Spawn(AssetManager.Instance.GetUnitData(spawnName), false, node);
                    enemy.onMoveDone += OnEnemyMoveDone;
                    switch (moveType)
                    {
                        case "S":
                            enemy.currentMoveType = Unit.MoveType.stay;
                            break;
                        case "D":
                            enemy.currentMoveType = Unit.MoveType.directToCastle;
                            break;
                        case "C":
                            enemy.currentMoveType = Unit.MoveType.cover;
                            break;
                        case "N":
                            enemy.currentMoveType = Unit.MoveType.nearTerritory;
                            break;
                        case "M":
                            enemy.currentMoveType = Unit.MoveType.moveToSelectedNode;
                            foreach (Node node1 in allNodes)
                            {
                                string[] s = node1.name.Split("_"[0]);
                                if (s[0] == spawnStatus[3])
                                {
                                    enemy.targetNode = node1;
                                    break;
                                }
                            }
                            break;
                    }
                }
            }

            foreach (string allyData in node.startAllies)
            {
                string[] spawnStatus = allyData.Split(" "[0]);
                string spawnName = spawnStatus[0];
                int number = int.Parse(spawnStatus[1]);

                for (int i = 0; i < number; i++)
                {
                    Unit ally = Spawner.spawner.Spawn(AssetManager.Instance.GetUnitData(spawnName), true, node);
                    ally.isAuto = false;
                }
            }
        }
    }
    private void AllEnemyAttack()
    {
        foreach (Node node in allNodes)
        {
            foreach (Unit unit in node.enemies)
            {
                unit.currentMoveType = Unit.MoveType.directToCastle;
            }
        }
    }
    private void MoveAutoUnitToRallyPoint()
    {
        foreach (Node node in allNodes)
        {
            foreach (Unit ally in node.allies)
            {
                if (ally.position.isFighting)
                {
                    ally.isAuto = false;
                }
                else if (ally.isAuto && ally.canMove)
                {
                    Node nextNode = ally.NextNode();
                    ally.MoveBetweenNodes(ally.position, nextNode);
                    if (ally.position.Equals(ally.targetNode))
                    {
                        ally.isAuto = false;
                    }
                }
            }
        }
        foreach (Node n in allNodes)
        {
            n.RefineUnitPosition(n.allies.Count, n.enemies.Count);
            //n.DecideAndShowMainUnit();
            foreach (Unit unit in n.units)
            {
                if (unit.moveQueue.Count > 0 && !unit.IsMoving)
                    StartCoroutine(unit.moveQueue.Dequeue());
            }
        }
    }
}
