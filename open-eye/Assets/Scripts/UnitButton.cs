using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    Unit unit;
    bool isRed;
    bool selected;
    bool isForMove = true;

    public void Awake()
    {
        selected = false;
    }

    public void AddUnit(Unit u, bool isForMove)
    {
        unit = u;
        this.isForMove = isForMove;
    }

    public void OnButtonClick()
    {
        if (isForMove)
        {
            if (Manager.manager.allies.Contains(unit))
            {
                if (!selected)
                {
                    Manager.manager.buttonUnitList.Add(unit);
                    selected = true;
                }
                else
                {
                    Manager.manager.buttonUnitList.Remove(unit);
                    selected = false;
                }
            }
        }
        else
        {
            Manager.manager.spawnableUnitList.Add(unit);
        }
    }
}
