using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListItem : MonoBehaviour
{
    public delegate void OnClickUnitListItem(UnitListItem listItem);

    [SerializeField]
    Text unitName;

    [SerializeField]
    Image unitIcon;

    [SerializeField]
    GameObject costObject;

    [SerializeField]
    GameObject ProducableCount;

    [SerializeField]
    Text manaValue;

    [SerializeField]
    Text attackValue;

    [SerializeField]
    Text healthValue;

    [SerializeField]
    Text aggroValue;

    [SerializeField]
    Text movementValue;

    [SerializeField]
    Text level;

    [SerializeField]
    public Text count;

    [SerializeField]
    Button button;

    [SerializeField]
    Color colorNormal;

    [SerializeField]
    Color colorAlly;

    [SerializeField]
    Color colorEnemy;

    public Unit unit { get; private set; }
    public UnitData unitData { get; private set; }
    private OnClickUnitListItem onClick;
    public bool isSelected { get; private set; }

    public void SetUnit(Unit unit, OnClickUnitListItem onClick)
    {
        this.unit = unit;
        this.onClick = onClick;
        SetUnitData(unit.unitData, onClick);
        healthValue.text = string.Format("{0}/{1}", unit.CurrentHealth, unit.unitData.health);
        movementValue.text = string.Format("{0}/{1}", unit.Movement, unit.unitData.movement);
        ShowCostObject(false);
        ShowProducableCount(false);
    }

    public void SetProducableUnitData(UnitData unitData, OnClickUnitListItem onClick)
    {
        this.unitData = unitData;
        this.onClick = onClick;
        unitName.text = unitData.unitName;
        unitIcon.sprite = AssetManager.Instance.GetSprite(unitData.iconName);
        attackValue.text = unitData.attack.ToString();
        healthValue.text = string.Format("{0}/{1}", unitData.health, unitData.health);
        aggroValue.text = unitData.aggro.ToString();
        movementValue.text = string.Format("{0}/{1}", unitData.movement, unitData.movement);
        manaValue.text = unitData.cost.ToString();
        level.text = unitData.level.ToString();
        ShowCostObject(true);
        ShowProducableCount(true);
    }

    public void SetUnitData(UnitData unitData, OnClickUnitListItem onClick)
    {
        this.unitData = unitData;
        this.onClick = onClick;
        unitName.text = unitData.unitName;
        unitIcon.sprite = AssetManager.Instance.GetSprite(unitData.iconName);
        attackValue.text = unitData.attack.ToString();
        healthValue.text = string.Format("{0}/{1}", unitData.health, unitData.health);
        aggroValue.text = unitData.aggro.ToString();
        movementValue.text = string.Format("{0}/{1}", unitData.movement, unitData.movement);
        manaValue.text = unitData.cost.ToString();
        level.text = unitData.level.ToString();
        ShowCostObject(true);
        ShowProducableCount(false);
    }

    public void SetDestroyedEnemyDataForControlScrollView(Unit unit, OnClickUnitListItem onClick)
    {
        this.unit = unit;
        this.onClick = onClick;
        this.unitData = unit.unitData;
        GetComponent<Image>().sprite = AssetManager.Instance.GetSprite(unitData.iconName);
    }

    public void SetDestroyedEnemyDataForUnitList(Unit unit, OnClickUnitListItem onClick)
    {
        this.unit = unit;
        SetUnitData(unit.unitData, onClick);
    }
    
    public void ShowCostObject(bool show)
    {
        costObject.SetActive(show);
    }

    public void ShowProducableCount(bool show)
    {
        ProducableCount.SetActive(show);
    }

    public void OnItemClick()
    {
        if (onClick != null)
        {
            onClick(this);
        }
        var colors = button.colors;
        var color = colorAlly;
        if (unit != null)
        {
            if (unit.isAlly)
            {
                if (unit.canMove)
                {
                    color = colors.normalColor == colorNormal ? colorAlly : colorNormal;
                }
            }
            else
            {
                color = colorEnemy;
            }
        }
        else
        {
            color = colorAlly;
        }
        colors.normalColor = color;
        colors.highlightedColor = color;
        button.colors = colors;
    }

    public void OnIconClick()
    {
        if (onClick != null)
        {
            onClick(this);
        }
    }
    
    public void SetColor()
    {
        var colors = button.colors;
        var color = unit.isAlly ? colorAlly : colorEnemy;
        colors.normalColor = color;
        colors.highlightedColor = color;
        button.colors = colors;
    }
}
