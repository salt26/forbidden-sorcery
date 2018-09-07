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

    [SerializeField]
    Color colorAuto;

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

    public void SetUnitData(UnitData unitData, OnClickUnitListItem onClick, bool isForDestroyed = false)
    {
        this.unitData = unitData;
        this.onClick = onClick;
        unitName.text = unitData.unitName;
        unitIcon.sprite = AssetManager.Instance.GetSprite(unitData.iconName);
        attackValue.text = unitData.attack.ToString();
        if (isForDestroyed)
            healthValue.text = string.Format("{0}/{1}", 0, unitData.health);
        else
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
        SetUnitData(unit.unitData, onClick, true);
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
        ClickSoundManager.instance.PlaySound();
        if (onClick != null)
        {
            onClick(this);
        }
        if (!GameManager.instance.unitListScrollView.isForProduce)
        {
            isSelected = !isSelected;
            unit.isAuto = false;
        }
        var colors = button.colors;
        var color = colorAlly;
        if (unit != null)
        {
            if (unit.isAlly)
            {
                if (unit.canMove)
                {
                    color = colors.normalColor == colorNormal ? (unit.isAuto ? colorAuto : colorAlly) : colorNormal;
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

    public void OnItemClickNoSound()
    {
        if (onClick != null)
        {
            onClick(this);
        }
        if (!GameManager.instance.unitListScrollView.isForProduce)
        {
            isSelected = !isSelected;
            unit.isAuto = false;
        }
        var colors = button.colors;
        var color = colorAlly;
        if (unit != null)
        {
            if (unit.isAlly)
            {
                if (unit.canMove)
                {
                    color = colors.normalColor == colorNormal ? (unit.isAuto ? colorAuto : colorAlly) : colorNormal;
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
        ClickSoundManager.instance.PlaySound();
        if (onClick != null)
        {
            onClick(this);
        }
    }

    public void SetColor()
    {
        var colors = button.colors;
        var color = unit.isAlly ? (unit.isAuto ? colorAuto : colorAlly) : colorEnemy;
        colors.normalColor = color;
        colors.highlightedColor = color;
        button.colors = colors;
    }
}
