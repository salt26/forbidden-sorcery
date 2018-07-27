using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    Unit unit;
    UnitData unitData;
    bool isRed;
    bool selected;
    bool isForMove = true;

    public Unit Unit
    {
        get
        {
            return unit;
        }
    }

    private void Awake()
    {
        selected = false;
    }

    public void AddUnit(Unit u, UnitData unitData, bool isForMove = false)
    {
        unit = u;
        this.unitData = unitData;
        this.isForMove = isForMove;
    }

    public void OnButtonClick()
    {
        if (unit != null)
            Manager.manager.AHText.stateAH(false, unit.attck.ToString(), unit.health.ToString(), unit.movableLength.ToString());
        if (Manager.manager.isPlayerActionTurn)
        {
            if (isForMove)
            {
                if (Manager.manager.allies.Contains(unit))
                {
                    if (!selected)
                    {
                        Manager.manager.buttonUnitList.Add(unit);
                        GetComponent<Image>().color = Color.black;
                        selected = true;
                    }
                    else
                    {
                        Manager.manager.buttonUnitList.Remove(unit);
                        GetComponent<Image>().color = Color.white;
                        selected = false;
                    }
                }
            }
            else
            {
                Manager.manager.spawnUnitList.Add(unitData);
            }
        }

        else
        {
            if (!selected)
            {
                Manager.manager.destroyEnemySelectButtons.Add(this.GetComponent<Button>());
                GetComponent<Image>().color = Color.black;
                selected = true;
            }
            else
            {
                Manager.manager.destroyEnemySelectButtons.Remove(this.GetComponent<Button>());
                GetComponent<Image>().color = Color.white;
                selected = false;
            }
        }
    }
}
