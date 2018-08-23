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
        if (isAlly)
        {
            bool isActivated = false;
            UnitData uD = null;
            foreach (var unitD in GameManager.instance.producableAlliedEnemies)
            {
                if (unitD.Equals(unitData))
                {
                    isActivated = true;
                    uD = unitD;
                    break;
                }
            }
            if (isActivated)
            {
                GameManager.instance.producableAlliedEnemies.Remove(uD);
                GameManager.instance.producedAlliedEnemies.Add(uD);
                GameManager.instance.numberOfProducableAlliedEnemies[uD] -= 1;
            }
        }
        unit.transform.localPosition = n.transform.localPosition;
        unit.position = n;
        
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
