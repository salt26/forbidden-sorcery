using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListScrollView : MonoBehaviour
{
    [SerializeField]
    Transform content;

    [SerializeField]
    Vector3 contentInactive;

    [SerializeField]
    Vector3 contentActive;

    public bool nowListShown { get; private set; }

    void Awake()
    {
        nowListShown = false;
    }
    
    private List<UnitListItem> listItems = new List<UnitListItem>();
    public class UnitSort
    {
        public class UnitMoveListCompare : IComparer<Unit>
        {
            public int Compare(Unit unit1, Unit unit2)
            {
                if (unit1.unitData.aggro.CompareTo(unit2.unitData.aggro) != 0)
                {
                    return unit1.unitData.aggro.CompareTo(unit2.unitData.aggro);
                }
                else if (unit1.CurrentHealth.CompareTo(unit2.CurrentHealth) != 0)
                {
                    return unit1.CurrentHealth.CompareTo(unit2.CurrentHealth);
                }
                else if (unit1.Movement.CompareTo(unit2.Movement) != 0)
                {
                    return unit1.Movement.CompareTo(unit2.Movement);
                }
                else if (unit1.isAlly.CompareTo(unit2.isAlly) != 0)
                {
                    return unit1.isAlly.CompareTo(unit2.isAlly);
                }
                else
                {
                    return 0;
                }
            }
        }
        public class UnitProduceListCompare : IComparer<UnitData>
        {
            public int Compare(UnitData unitData1, UnitData unitData2)
            {
                if (unitData2.cost.CompareTo(unitData1.cost) != 0)
                {
                    return unitData2.cost.CompareTo(unitData1.cost);
                }
                else if (unitData1.aggro.CompareTo(unitData2.aggro) != 0)
                {
                    return unitData1.aggro.CompareTo(unitData2.aggro);
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public void SetUnitList(List<Unit> unitList, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();
        unitList.Sort(new UnitSort.UnitMoveListCompare());
        foreach (var unit in unitList)
        {
            var listItemObject = Instantiate(AssetManager.Instance.GetPrefab("UnitListItem"), content);
            var unitListItem = listItemObject.GetComponent<UnitListItem>();
            unitListItem.SetUnit(unit, onClick);
            unitListItem.SetColor();
            listItems.Add(unitListItem);
        }
    }

    public void SetUnitDataList(List<UnitData> unitDataList, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();
        unitDataList.Sort(new UnitSort.UnitProduceListCompare());
        foreach (var unitData in unitDataList)
        {
            var listItemObject = Instantiate(AssetManager.Instance.GetPrefab("UnitListItem"), content);
            var unitListItem = listItemObject.GetComponent<UnitListItem>();
            unitListItem.SetUnitData(unitData, onClick);
            listItems.Add(unitListItem);
        }
    }

    public void SetControlDestroyedEnemiesList(List<Unit> destroyedEnemies, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();
        //destroyedEnemies.Sort(new UnitSort.UnitMoveListCompare());
        foreach(var dEnemy in destroyedEnemies)
        {
            Debug.Log(dEnemy.unitData.iconName);
            var listItemObject = Instantiate(AssetManager.Instance.GetPrefab("UnitListItem"), content);
            var unitListItem = listItemObject.GetComponent<UnitListItem>();
            unitListItem.SetUnitData(dEnemy.unitData, onClick);
            listItems.Add(unitListItem);
        }
    }

    private void ClearItem()
    {
        listItems.ForEach((item) => Destroy(item.gameObject));
        listItems.Clear();
    }

    public void ShowList(bool show)
    {
        transform.localPosition = show ? contentActive : contentInactive;
        nowListShown = show;
    }
}
