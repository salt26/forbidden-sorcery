using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager
{
    public bool isMouseInMap { get; private set; }

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
        unitListScrollView.ShowList(false);
        unitListScrollView.ShowUnitTab(false);
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
        if (!show)
        {
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
            Spawner.spawner.Spawn(item.unitData, true, castle);
        }
    }

    public void OnSelectUnitForControlDestroyedEnemy(UnitListItem item)
    {
        if (SelectedDestroyedEnemyList != null && SelectedDestroyedEnemyList.Contains(item.unit))
        {
            SelectedDestroyedEnemyList.Remove(item.unit);
            destroyedEnemies.Add(item.unit);
        }
        else if (item.unit != null)
        {
            SelectedDestroyedEnemyList.Add(item.unit);
            destroyedEnemies.Remove(item.unit);
        }

        if (destroyedEnemies.Count == 0)
            endTurnButton.interactable = true;

        var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
        destroyedEnemyControlScrollView.SetControlDestroyedEnemiesList(selectedDestroyedEnemyList, OnSelectUnitForControlDestroyedEnemy);
        foreach (var g in unitScrollView.listItems)
        {
            g.GetComponent<Button>().interactable = true;
        }
    }

    public void OnClickDestroyedEnemyControlButton(List<Unit> units, DestroyedEnemyControlButton button)
    {
        selectedDestroyedEnemyList = units;
        var controlScrollView = destroyedEnemyControlScrollView.SetControlDestroyedEnemiesList(selectedDestroyedEnemyList, OnSelectUnitForControlDestroyedEnemy);

        if (button.isSelected)
        {
            foreach (DestroyedEnemyControlButton b in destroyedEnemyControlButtons)
            {
                b.GetComponent<Button>().interactable = true;
                b.isSelected = false;
            }
            destroyedEnemyControlButton = null;
            foreach(Button g in destroyedEnemyControlResetButtons)
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
                g.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            foreach (DestroyedEnemyControlButton b in destroyedEnemyControlButtons)
            {
                if (b.Equals(button))
                {
                    b.isSelected = true;
                }
                else
                {
                    b.GetComponent<Button>().interactable = false;
                    b.isSelected = false;
                }
            }
            destroyedEnemyControlButton = button.gameObject;
            destroyedEnemyControlResetButtons[(int)destroyedEnemyControlButton.GetComponent<DestroyedEnemyControlButton>().kindOfButton].interactable = true;
            var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
            foreach (var g in unitScrollView.listItems)
            {
                g.GetComponent<Button>().interactable = true;
            }
            foreach (var g in controlScrollView.listItems)
            {
                g.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void OnClickRefreshControlButton()
    {
        destroyedEnemies.AddRange(selectedDestroyedEnemyList);
        selectedDestroyedEnemyList.Clear();
        var unitScrollView = unitListScrollView.SetControlDestroyedEnemiesList(destroyedEnemies, OnSelectUnitForControlDestroyedEnemy);
        foreach (var g in unitScrollView.listItems)
        {
            g.GetComponent<Button>().interactable = true;
        }
        destroyedEnemyControlScrollView.SetControlDestroyedEnemiesList(selectedDestroyedEnemyList, OnSelectUnitForControlDestroyedEnemy);
    }

    public void OnClickEndTurnButton()
    {
        selectedUnitList.Clear();
        unitListScrollView.ShowList(false);
        unitListScrollView.ShowUnitTab(false);
        if (selectedNode != null && selectedNode.GetComponent<SpriteRenderer>().color != originColor)
        {
            selectedNode.GetComponent<SpriteRenderer>().color = originColor;
        }
        originColor = Color.white;
        selectedNode = null;
        StartCoroutine(ChangePhase());
    }

    public void OnClickProduceButton()
    {
        unitListScrollView.ShowUnitTab(false);

        if (currentState != RoundState.PlayerAction)
        {
            return;
        }

        if (unitListScrollView.nowListShown == false)
        {
            unitListScrollView.ShowList(true);
            unitListScrollView.SetUnitDataList(config.producableUnits, OnSelectUnitForProduce);
        }
        else
        {
            if (selectedNode != null)
            {
                unitListScrollView.SetUnitDataList(config.producableUnits, OnSelectUnitForProduce);
                if (selectedNode.GetComponent<SpriteRenderer>().color != originColor)
                {
                    selectedNode.GetComponent<SpriteRenderer>().color = originColor;
                }
            }
            else
            {
                unitListScrollView.ShowList(false);
            }
            selectedUnitList.Clear();
            originColor = Color.white;
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
                    selectedNode = node;
                    var spriteRenderer = selectedNode.GetComponent<SpriteRenderer>();
                    originColor = spriteRenderer.color;
                    spriteRenderer.color = Color.black;

                    unitListScrollView.ShowList(true);
                    unitListScrollView.ShowUnitTab(true);
                    unitListScrollView.SetUnitList(node.allies, OnSelectUnitForMove);
                }
                else if (selectedNode == null)
                {
                    node.RedLight();
                }
                else if (selectedNode == node)
                {
                    selectedNode.GetComponent<SpriteRenderer>().color = originColor;
                    selectedNode = null;

                    selectedUnitList.Clear();
                    unitListScrollView.ShowList(false);
                    unitListScrollView.ShowUnitTab(false);
                }
                else if (!selectedNode.edges.Contains(node) && selectedUnitList.Count > 0)
                {
                    node.RedLight();
                }
                else
                {
                    foreach (Unit unit in selectedUnitList)
                    {
                        if (unit.canMove)
                        {
                            unit.MoveBetweenNodes(selectedNode, node);
                        }
                    }

                    if (isAllyMoving)
                    {
                        node.RedLight(); selectedNode.GetComponent<SpriteRenderer>().color = originColor;
                    }
                    else
                    {
                        selectedNode.GetComponent<SpriteRenderer>().color = originColor;
                    }

                    selectedNode = null;
                    selectedUnitList.Clear();
                    unitListScrollView.ShowList(false);
                    unitListScrollView.ShowUnitTab(false);

                }
            }
            else
            {
                originColor = node.GetComponent<SpriteRenderer>().color;
                if (selectedNode == node)
                {
                    selectedNode = null;
                    selectedUnitList.Clear();
                    unitListScrollView.ShowList(false);
                    unitListScrollView.ShowUnitTab(false);
                }
                else
                {
                    selectedNode = node;

                    unitListScrollView.ShowList(true);
                    unitListScrollView.ShowUnitTab(false);
                    unitListScrollView.SetUnitList(node.destroyedEnemies);
                }
            }
        }
    }

    public void OnClickAllyTabButton()
    {
        unitListScrollView.allyTabFake.color = unitListScrollView.allyTabPressedColor;
        unitListScrollView.enemyTabFake.color = unitListScrollView.enemyTabNormalColor;
        unitListScrollView.ShowList(true);
        unitListScrollView.ShowUnitTab(true);
        unitListScrollView.SetUnitList(selectedNode.allies, OnSelectUnitForMove);
    }

    public void OnClickEnemyTabButton()
    {
        unitListScrollView.enemyTabFake.color = unitListScrollView.enemyTabPressedColor;
        unitListScrollView.allyTabFake.color = unitListScrollView.allyTabNormalColor;
        unitListScrollView.ShowList(true);
        unitListScrollView.ShowUnitTab(true);
        unitListScrollView.SetUnitList(selectedNode.enemies, OnSelectUnitForMove);
    }
}
