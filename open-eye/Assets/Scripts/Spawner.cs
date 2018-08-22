using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int id = 0;
    public static Spawner spawner;
    void Awake()
    {
        spawner = this;
    }
    public Unit Spawn(UnitData unitData, bool isAlly, Node n)
    {
        var unitObject = Instantiate(AssetManager.Instance.GetPrefab("Unit"));
        var unit = unitObject.GetComponent<Unit>();
        unit.SetUnit(unitData);
        unit.isAlly = isAlly;
        unit.ID = ++id;
        unit.transform.localPosition = n.transform.localPosition;
        unit.position = n;

        if (GameManager.instance.karma == 0)
        {
            unit.unitData.currentMoveType = (UnitData.MoveType)2;
        }
        n.units.Add(unit); n.RefineUnitPosition(n.allies.Count, n.enemies.Count);
        foreach (Unit i in n.units)
        {
            if (i.moveQueue.Count > 0 && !i.IsMoving)
                StartCoroutine(i.moveQueue.Dequeue());
        }
        n.DecideAndShowMainUnit();

        return unit;
    }
}
