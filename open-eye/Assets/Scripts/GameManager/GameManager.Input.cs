using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager
{
    public bool isMouseInMap { get; private set; }

    public int unitMovingOrder;
    public int unitMovingOrderCount;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            OnClickProduceButton();
        }

        if (Input.GetKeyUp(KeyCode.F2))
        {
            if (selectedNode != null && unitListScrollView.nowListShown && !unitListScrollView.isForProduce)
            {
                ClickSoundManager.instance.PlaySound();
                foreach (var unitListItem in unitListScrollView.listItems)
                {
                    if (!unitListItem.isSelected)
                    {
                        unitListItem.OnItemClickNoSound();
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            if (selectedNode != null && unitListScrollView.nowListShown && !unitListScrollView.isForProduce)
            {
                ClickSoundManager.instance.PlaySound();
                foreach (var unitListItem in unitListScrollView.listItems)
                {
                    if (unitListItem.unit.Movement == 1 && !unitListItem.isSelected)
                    {
                        unitListItem.OnItemClickNoSound();
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha2) && unitListScrollView.nowListShown && !unitListScrollView.isForProduce)

        {
            if (selectedNode != null && unitListScrollView.nowListShown)
            {
                foreach (var unitListItem in unitListScrollView.listItems)
                {
                    ClickSoundManager.instance.PlaySound();
                    if (unitListItem.unit.Movement == 2 && !unitListItem.isSelected)
                    {
                        unitListItem.OnItemClickNoSound();
                    }
                }
            }
        }
    }

    private bool isNodeInputActive
    {
        get
        {
            switch (currentState)
            {
                case RoundState.PlayerAction:
                    return isMouseInMap;
                default:
                    return false;
            }
        }
    }

    private void InitializeInput()
    {
        isMouseInMap = true;
        destroyedEnemyControlUnit.SetActive(false);
        unitListScrollView.ShowList(false);
        unitListScrollView.ShowUnitTab(false);
    }

    public void SetRallyPoint(Node node)
    {
        if (rallyPoint != null)
        {
            rallyPoint.isRallyPoint = false;
            rallyPoint.GetComponent<SpriteRenderer>().color = unSelectedColor;
        }
        if (node.Equals(rallyPoint))
        {
            rallyPoint.isRallyPoint = false;
            node.GetComponent<SpriteRenderer>().color = unSelectedColor;
            rallyPoint = null;
        }
        else
        {
            rallyPoint = node;
            node.isRallyPoint = true;
            node.GetComponent<SpriteRenderer>().color = rallyPointColor;
        }
    }

    public void OnMouseEnterMap()
    {
        isMouseInMap = true;
    }

    public void OnMouseExitMap()
    {
        isMouseInMap = false;
    }

    public void UnitListShow(bool show)
    {
        selectedUnitList.Clear();
        unitListScrollView.isForProduce = false;
        if (!show)
        {
            if (selectedNode != null)
            {
                selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.isRallyPoint ? rallyPointColor : unSelectedColor;
            }
            selectedNode = null;
        }
        unitListScrollView.ShowList(show);
        unitListScrollView.ShowUnitTab(false);
    }

    public void OnSelectUnitForMove(UnitListItem item)
    {
        if (selectedUnitList.Contains(item.unit))
        {
            selectedUnitList.Remove(item.unit);
        }
        else if (item.unit != null && item.unit.isAlly)
        {
            selectedUnitList.Add(item.unit);
        }
    }

    public void OnSelectUnitForProduce(UnitListItem item)
    {
        if (item.unitData.cost <= Mana)
        {
            Mana -= item.unitData.cost;
            Unit ally = Spawner.spawner.Spawn(item.unitData, true, castle);
            if (rallyPoint != null)
            {
                ally.currentMoveType = Unit.MoveType.moveToSelectedNode;
                ally.targetNode = rallyPoint;
                ally.isAuto = true;
            }
        }
        Dictionary<UnitData, int> unitDatas = new Dictionary<UnitData, int>();
        foreach (var producableAlliedEnemy in numberOfProducableAlliedEnemies)
        {
            if (!unitDatas.ContainsKey(producableAlliedEnemy.Key))
            {
                unitDatas.Add(producableAlliedEnemy.Key, producableAlliedEnemy.Value);
            }
        }
        foreach (UnitData unitdata in config.producableUnits)
        {
            unitDatas.Add(unitdata, int.MaxValue);
        }
        unitListScrollView.SetUnitDataList(unitDatas, OnSelectUnitForProduce);
    }

    public void OnSelectUnitForControlDestroyedEnemy(UnitListItem item)
    {
        if (SelectedDestroyedEnemyList != null && SelectedDestroyedEnemyList.Contains(item.unit))
        {
            SelectedDestroyedEnemyList.Remove(item.unit);
            foreach (DestroyedEnemyControlButton decb in destroyedEnemyControlButtons)
            {
                if (decb.isSelected)
                {
                    decb.dominateNotorietychange -= item.unit.unitData.level;
                    decb.dominateManaChange += item.unit.unitData.cost / 4;
                    decb.killManaChange -= item.unit.unitData.cost / 2 + item.unit.unitData.level * 100;
                    decb.killNotorietyChange -= item.unit.unitData.level * 2;
                    decb.freeNotorietyChange += item.unit.unitData.level;
                    decb.ShowExpectedResourceChange();
                }
            }
            destroyedEnemies.Add(item.unit);
        }
        else if (item.unit != null)
        {
            SelectedDestroyedEnemyList.Add(item.unit);
            foreach (DestroyedEnemyControlButton decb in destroyedEnemyControlButtons)
            {
                if (decb.isSelected)
                {
                    decb.dominateNotorietychange += item.unit.unitData.level;
                    decb.dominateManaChange -= item.unit.unitData.cost / 4;
                    decb.killManaChange += item.unit.unitData.cost / 2 + item.unit.unitData.level * 100;
                    decb.killNotorietyChange += item.unit.unitData.level * 2;
                    decb.freeNotorietyChange -= item.unit.unitData.level;
                    decb.ShowExpectedResourceChange();
                }
            }
            destroyedEnemies.Remove(item.unit);
        }

        if (destroyedEnemies.Count == 0)
            endTurnButton.interactable = true;
        else
            endTurnButton.interactable = false;

        var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
        destroyedEnemyControlScrollView.SetControlDestroyedEnemiesList(selectedDestroyedEnemyList, OnSelectUnitForControlDestroyedEnemy);
        foreach (var g in unitScrollView.listItems)
        {
            g.GetComponent<Button>().interactable = true;
        }
    }

    public void OnClickDestroyedEnemyControlButton(List<Unit> units, DestroyedEnemyControlButton button)
    {
        if (pastDestroyedEnemyControlScrollView != null)
        {
            var temp = pastDestroyedEnemyControlScrollView.SetControlDestroyedEnemiesList(selectedDestroyedEnemyList, OnSelectUnitForControlDestroyedEnemy);
            foreach (var g in temp.listItems)
            {
                g.GetComponent<Image>().raycastTarget = false;
                g.GetComponent<Button>().interactable = false;
            }
        }
        selectedDestroyedEnemyList = units;
        var controlScrollView = destroyedEnemyControlScrollView.SetControlDestroyedEnemiesList(selectedDestroyedEnemyList, OnSelectUnitForControlDestroyedEnemy);

        if (button.isSelected)
        {
            foreach (DestroyedEnemyControlButton b in destroyedEnemyControlButtons)
            {
                b.GetComponent<Image>().color = b.normalColor;
                b.isSelected = false;
            }
            foreach (Button g in destroyedEnemyControlResetButtons)
            {
                g.interactable = false;
            }
            var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
            foreach (var g in unitScrollView.listItems)
            {
                g.GetComponent<Button>().interactable = false;
            }
            foreach (var g in controlScrollView.listItems)
            {
                g.GetComponent<Image>().raycastTarget = false;
                g.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            foreach (DestroyedEnemyControlButton b in destroyedEnemyControlButtons)
            {
                if (b.Equals(button))
                {
                    b.GetComponent<Image>().color = b.normalColor;
                    b.isSelected = true;
                }
                else
                {
                    b.GetComponent<Image>().color = b.disabledColor;
                    b.isSelected = false;
                }
            }
            foreach (Button g in destroyedEnemyControlResetButtons)
            {
                g.interactable = false;
            }
            destroyedEnemyControlResetButtons[(int)button.gameObject.GetComponent<DestroyedEnemyControlButton>().kindOfButton].interactable = true;
            var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
            foreach (var g in unitScrollView.listItems)
            {
                g.GetComponent<Button>().interactable = true;
            }
            foreach (var g in controlScrollView.listItems)
            {
                g.GetComponent<Image>().raycastTarget = true;
                g.GetComponent<Button>().interactable = true;
            }
        }
        pastDestroyedEnemyControlScrollView = destroyedEnemyControlScrollView;
    }

    public void OnClickRefreshControlButton()
    {
        destroyedEnemies.AddRange(selectedDestroyedEnemyList);
        selectedDestroyedEnemyList.Clear();
        foreach (DestroyedEnemyControlButton decb in destroyedEnemyControlButtons)
        {
            if (decb.isSelected)
            {
                decb.dominateNotorietychange = 0;
                decb.dominateManaChange = 0;
                decb.killManaChange = 0;
                decb.killNotorietyChange = 0;
                decb.freeNotorietyChange = 0;
                decb.ShowExpectedResourceChange();
            }
        }
        var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
        foreach (var g in unitScrollView.listItems)
        {
            g.GetComponent<Button>().interactable = true;
        }
        destroyedEnemyControlScrollView.SetControlDestroyedEnemiesList(selectedDestroyedEnemyList, OnSelectUnitForControlDestroyedEnemy);
    }

    public void OnClickEndTurnButton()
    {
        ClickSoundManager.instance.PlaySound();
        selectedUnitList.Clear();
        unitListScrollView.ShowList(false);
        unitListScrollView.isForProduce = false;
        unitListScrollView.ShowUnitTab(false);
        if (selectedNode != null)
        {
            selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.isRallyPoint ? rallyPointColor : unSelectedColor;
        }
        selectedNode = null;
        StartCoroutine(ChangePhase());
    }

    public void OnClickEndTurnButtonNoSound()
    {
        selectedUnitList.Clear();
        unitListScrollView.ShowList(false);
        unitListScrollView.isForProduce = false;
        unitListScrollView.ShowUnitTab(false);
        if (selectedNode != null)
        {
            selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.isRallyPoint ? rallyPointColor : unSelectedColor;
        }
        selectedNode = null;
        StartCoroutine(ChangePhase());
    }

    public void OnClickProduceButton()
    {
        ClickSoundManager.instance.PlaySound();

        unitListScrollView.ShowUnitTab(false);

        if (currentState != RoundState.PlayerAction)
        {
            return;
        }

        if (unitListScrollView.nowListShown == false)
        {
            unitListScrollView.ShowList(true);
            unitListScrollView.isForProduce = true;
            Dictionary<UnitData, int> unitDatas = new Dictionary<UnitData, int>();
            foreach (var producableAlliedEnemy in numberOfProducableAlliedEnemies)
            {
                if (!unitDatas.ContainsKey(producableAlliedEnemy.Key))
                {
                    unitDatas.Add(producableAlliedEnemy.Key, producableAlliedEnemy.Value);
                }
            }
            foreach (UnitData unitdata in config.producableUnits)
            {
                unitDatas.Add(unitdata, int.MaxValue);
            }
            unitListScrollView.SetUnitDataList(unitDatas, OnSelectUnitForProduce);
        }
        else
        {
            if (selectedNode != null)
            {
                unitListScrollView.isForProduce = true;
                Dictionary<UnitData, int> unitDatas = new Dictionary<UnitData, int>();
                foreach (var producableAlliedEnemy in numberOfProducableAlliedEnemies)
                {
                    if (!unitDatas.ContainsKey(producableAlliedEnemy.Key))
                    {
                        unitDatas.Add(producableAlliedEnemy.Key, producableAlliedEnemy.Value);
                    }
                }
                foreach (UnitData unitdata in config.producableUnits)
                {
                    unitDatas.Add(unitdata, int.MaxValue);
                }
                unitListScrollView.SetUnitDataList(unitDatas, OnSelectUnitForProduce);
                selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.isRallyPoint ? rallyPointColor : unSelectedColor;
            }
            else
            {
                unitListScrollView.ShowList(false);
                unitListScrollView.isForProduce = false;
            }
            selectedUnitList.Clear();
            selectedNode = null;
        }


        /*if (selectedNode != null && selectedNode.GetComponent<SpriteRenderer>().color != originColor)
        {
            selectedNode.GetComponent<SpriteRenderer>().color = originColor;
        }
        originColor = Color.white;
        selectedNode = null;

        unitListScrollView.ShowList(true);
        unitListScrollView.SetUnitDataList(config.producableUnits, OnSelectUnitForProduce);*/
    }

    public void SetNode(Node node)
    {
        if (isNodeInputActive)
        {
            if (currentState == RoundState.PlayerAction)
            {
                if (selectedNode == null && node.units.Count > 0)
                {
                    ClickSoundManager.instance.PlaySound();
                    selectedNode = node;
                    var spriteRenderer = selectedNode.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = selectedColor;

                    unitListScrollView.ShowList(true);
                    unitListScrollView.isForProduce = false;
                    unitListScrollView.ShowUnitTab(true);
                    if (node.allies.Count > 0)
                    {
                        unitListScrollView.SetUnitList(node.allies, OnSelectUnitForMove);
                    }
                    else
                    {
                        unitListScrollView.enemyTabFake.color = unitListScrollView.enemyTabPressedColor;
                        unitListScrollView.allyTabFake.color = unitListScrollView.allyTabNormalColor;
                        unitListScrollView.SetUnitList(node.enemies, OnSelectUnitForMove);
                    }
                }
                else if (selectedNode == null || (selectedUnitList.Count == 0 && node.units.Count == 0))
                {
                    node.RedLight();
                }
                else if (selectedNode == node)
                {
                    ClickSoundManager.instance.PlaySound();
                    selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.isRallyPoint ? rallyPointColor : unSelectedColor;
                    selectedNode = null;

                    selectedUnitList.Clear();
                    unitListScrollView.ShowList(false);
                    unitListScrollView.isForProduce = false;
                    unitListScrollView.ShowUnitTab(false);
                }
                else if (selectedUnitList.Count == 0)
                {
                    ClickSoundManager.instance.PlaySound();
                    selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.isRallyPoint ? rallyPointColor : unSelectedColor;
                    selectedNode = node;
                    node.GetComponent<SpriteRenderer>().color = selectedColor;

                    selectedUnitList.Clear();
                    unitListScrollView.ShowUnitTab(false);
                    unitListScrollView.ShowUnitTab(true);
                    if (node.allies.Count > 0)
                    {
                        unitListScrollView.SetUnitList(node.allies, OnSelectUnitForMove);
                    }
                    else
                    {
                        unitListScrollView.enemyTabFake.color = unitListScrollView.enemyTabPressedColor;
                        unitListScrollView.allyTabFake.color = unitListScrollView.allyTabNormalColor;
                        unitListScrollView.SetUnitList(node.enemies, OnSelectUnitForMove);
                    }
                }
                else if (!selectedNode.edges.Contains(node) && selectedUnitList.Count > 0)
                {
                    node.RedLight();
                }
                else
                {
                    ClickSoundManager.instance.PlaySound();
                    if (currentState == RoundState.PlayerAction)
                    {
                        endTurnButton.interactable = false;
                    }
                    unitMovingOrder = 0;
                    unitMovingOrderCount = 0;
                    foreach (Unit unit in selectedUnitList)
                        ++unitMovingOrder;
                    foreach (Unit unit in selectedUnitList)
                    {
                        if (unit.canMove)
                        {
                            unit.MoveBetweenNodes(selectedNode, node);
                            ++unitMovingOrderCount;
                        }
                    }
                    unitMovingOrder = 0;
                    selectedNode.RefineUnitPosition(selectedNode.allies.Count, selectedNode.enemies.Count);
                    //selectedNode.DecideAndShowMainUnit();
                    foreach (Unit unit in selectedNode.units)
                    {
                        if (unit.moveQueue.Count > 0 && !unit.IsMoving)
                            StartCoroutine(unit.moveQueue.Dequeue());
                    }

                    node.RefineUnitPosition(node.allies.Count, node.enemies.Count);
                    //node.DecideAndShowMainUnit();
                    foreach (Unit unit in node.units)
                    {
                        if (unit.moveQueue.Count > 0 && !unit.IsMoving)
                            StartCoroutine(unit.moveQueue.Dequeue());
                    }

                    selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.isRallyPoint ? rallyPointColor : unSelectedColor;

                    selectedNode = null;
                    selectedUnitList.Clear();
                    unitListScrollView.ShowList(false);
                    unitListScrollView.isForProduce = false;
                    unitListScrollView.ShowUnitTab(false);

                    StartCoroutine(waitUntilAllyIsNotMoving());
                }
            }
            else
            {
                if (selectedNode == node)
                {
                    selectedNode = null;
                    selectedUnitList.Clear();
                    unitListScrollView.ShowList(false);
                    unitListScrollView.isForProduce = false;
                    unitListScrollView.ShowUnitTab(false);
                }
                else
                {
                    selectedNode = node;

                    unitListScrollView.ShowList(true);
                    unitListScrollView.isForProduce = false;
                    unitListScrollView.ShowUnitTab(false);
                    unitListScrollView.SetUnitList(node.destroyedEnemies);
                }
            }
        }
    }

    public void OnClickAllyTabButton()
    {
        ClickSoundManager.instance.PlaySound();
        unitListScrollView.allyTabFake.color = unitListScrollView.allyTabPressedColor;
        unitListScrollView.enemyTabFake.color = unitListScrollView.enemyTabNormalColor;
        unitListScrollView.ShowList(true);
        unitListScrollView.ShowUnitTab(true);
        unitListScrollView.SetUnitList(selectedNode.allies, OnSelectUnitForMove);
    }

    public void OnClickEnemyTabButton()
    {
        ClickSoundManager.instance.PlaySound();
        unitListScrollView.enemyTabFake.color = unitListScrollView.enemyTabPressedColor;
        unitListScrollView.allyTabFake.color = unitListScrollView.allyTabNormalColor;
        unitListScrollView.ShowList(true);
        unitListScrollView.ShowUnitTab(true);
        unitListScrollView.SetUnitList(selectedNode.enemies, OnSelectUnitForMove);
    }

    private IEnumerator waitUntilAllyIsNotMoving()
    {
        unitMovingOrderCount = 0;
        yield return new WaitWhile(() => isAllyMoving);
        if (currentState == RoundState.PlayerAction)
        {
            endTurnButton.interactable = true;
        }
        else if(currentState == RoundState.AutoMove)
        {
            StartCoroutine(ChangePhase());
        }
    }
}
