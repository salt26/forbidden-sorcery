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
        if (GameManager.instance.currentState == GameManager.RoundState.PlayerAction)
        {
            if (isForMove)
            {
                if (GameManager.instance.allies.Contains(unit))
                {
                    if (!selected)
                    {
                        GameManager.instance.buttonUnitList.Add(unit);
                        GetComponent<Image>().color = Color.black;
                        selected = true;
                    }
                    else
                    {
                        GameManager.instance.buttonUnitList.Remove(unit);
                        GetComponent<Image>().color = Color.white;
                        selected = false;
                    }
                }
            }
            else
            {
                GameManager.instance.spawnUnitList.Add(unitData);
            }
        }

        else
        {
            if (!selected)
            {
                GameManager.instance.destroyEnemySelectButtons.Add(this.GetComponent<Button>());
                GetComponent<Image>().color = Color.black;
                selected = true;
            }
            else
            {
                GameManager.instance.destroyEnemySelectButtons.Remove(this.GetComponent<Button>());
                GetComponent<Image>().color = Color.white;
                selected = false;
            }
        }
    }
}
