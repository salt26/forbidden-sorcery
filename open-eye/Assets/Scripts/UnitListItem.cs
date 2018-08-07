using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListItem : MonoBehaviour
{
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

    public void SetUnit(Unit unit)
    {
        SetUnitData(unit.unitData);
        healthValue.text = string.Format("{0}/{1}", unit.currentHealth, unit.unitData.health);
        movementValue.text = string.Format("{0}/{1}", unit.movement, unit.unitData.movement);
        ShowCostObject(false);
    }

    public void SetUnitData(UnitData unitData)
    {
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
    }
}
