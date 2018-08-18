using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Unit spawnedAlly = Spawner.spawner.Spawn(item.unitData, true, castle);
            allies.Add(spawnedAlly);
        }
    }

    public void OnSelectUnitForControlDestroyedEnemy(UnitListItem item)
    {
    }

    public void OnClickEndTurnButton()
    {
        selectedUnitList.Clear();
        unitListScrollView.ShowList(false);
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
                    unitListScrollView.SetUnitList(node.units, OnSelectUnitForMove);
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
                }
                else
                {
                    selectedNode = node;

                    unitListScrollView.ShowList(true);
                    unitListScrollView.SetUnitList(node.destroyedEnemies);
                }
            }
        }
    }
}
