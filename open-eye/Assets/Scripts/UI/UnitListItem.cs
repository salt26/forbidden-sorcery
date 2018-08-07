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
    Text manaValue;

    [SerializeField]
    Text attackValue;

    [SerializeField]
    Text healthValue;

    [SerializeField]
    Text aggroValue;

    [SerializeField]
    Text movementValue;

    public Unit unit { get; private set; }
    public UnitData unitData { get; private set; }
    private OnClickUnitListItem onClick;

    public void SetUnit(Unit unit, OnClickUnitListItem onClick = null)
    {
        this.unit = unit;
        this.onClick = onClick;
        SetUnitData(unit.unitData, onClick);
        healthValue.text = string.Format("{0}/{1}", unit.currentHealth, unit.unitData.health);
        movementValue.text = string.Format("{0}/{1}", unit.movement, unit.unitData.movement);
        ShowCostObject(false);
    }

    public void SetUnitData(UnitData unitData, OnClickUnitListItem onClick = null)
    {
        this.unitData = unitData;
        this.onClick = onClick;
        unitName.text = unitData.unitName;
        unitIcon.sprite = AssetManager.Instance.GetSprite(unitData.iconName);
        attackValue.text = unitData.attack.ToString();
        healthValue.text = unitData.health.ToString();
        aggroValue.text = unitData.aggro.ToString();
        movementValue.text = unitData.movement.ToString();
        manaValue.text = unitData.cost.ToString();
        ShowCostObject(true);
    }

    public void ShowCostObject(bool show)
    {
        costObject.SetActive(show);
    }

    public void OnItemClick()
    {
        if (onClick != null)
        {
            onClick(this);
        }
    }
}
