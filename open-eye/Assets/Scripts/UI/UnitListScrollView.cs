using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListScrollView : MonoBehaviour
{
    [SerializeField]
    private GameObject ManaTab;

    [SerializeField]
    private GameObject UnitTab;

    [SerializeField]
    Transform content;

    [SerializeField]
    Vector3 contentInactive;

    [SerializeField]
    Vector3 contentActive;

    [SerializeField]
    public Image allyTabFake;

    [SerializeField]
    public Image enemyTabFake;

    [SerializeField]
    public GameObject allyTab;

    [SerializeField]
    public GameObject enemyTab;

    [SerializeField]
    public Color allyTabNormalColor;

    [SerializeField]
    public Color enemyTabNormalColor;

    [SerializeField]
    public Color allyTabPressedColor;

    [SerializeField]
    public Color enemyTabPressedColor;

    [SerializeField]
    public bool nowListShown { get; private set; }

    void Awake()
    {
        nowListShown = false;
    }
    
    public List<UnitListItem> listItems = new List<UnitListItem>();
    public class UnitSort
    {
        public class UnitMoveListCompare : IComparer<Unit>
        {
            public int Compare(Unit unit1, Unit unit2)
            {
                if (unit2.unitData.aggro.CompareTo(unit1.unitData.aggro) != 0)
                {
                    return unit2.unitData.aggro.CompareTo(unit1.unitData.aggro);
                }
                else if (unit1.CurrentHealth.CompareTo(unit2.CurrentHealth) != 0)
                {
                    return unit1.CurrentHealth.CompareTo(unit2.CurrentHealth);
                }
                else if (unit2.Movement.CompareTo(unit1.Movement) != 0)
                {
                    return unit2.Movement.CompareTo(unit1.Movement);
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

    public void SetUnitDataList(Dictionary<UnitData, int> unitDataList1, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();
        List<UnitData> unitDataList = new List<UnitData>();
        foreach (var unitdata in unitDataList1)
        {
            if (unitdata.Value > 0)
                unitDataList.Add(unitdata.Key);
        }
        unitDataList.Sort(new UnitSort.UnitProduceListCompare());
        foreach (var unitData in unitDataList)
        {
            var listItemObject = Instantiate(AssetManager.Instance.GetPrefab("UnitListItem"), content);
            var unitListItem = listItemObject.GetComponent<UnitListItem>();
            unitListItem.SetProducableUnitData(unitData, onClick);
            foreach (var ud in unitDataList1)
            {
                if (ud.Key == unitData)
                {
                    if (ud.Value < 100)
                    {
                        unitListItem.count.text = ud.Value.ToString();
                    }
                    else
                    {
                        unitListItem.count.text = "inf";
                    }
                    break;
                }
            }
            listItems.Add(unitListItem);
        }
    }
    
    public UnitListScrollView SetControlDestroyedEnemiesList(List<Unit> destroyedEnemies, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();
        //destroyedEnemies.Sort(new UnitSort.UnitMoveListCompare());
        foreach(var dEnemy in destroyedEnemies)
        {
            var listItemObject = Instantiate(AssetManager.Instance.GetPrefab("UnitListItem"), content);
            var unitListItem = listItemObject.GetComponent<UnitListItem>();
            unitListItem.SetDestroyedEnemyDataForUnitList(dEnemy, onClick);
            listItems.Add(unitListItem);
        }
        return this;
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
    
    public void ShowUnitTab(bool show)
    {
        if (!show)
        {
            allyTabFake.color = allyTabPressedColor;
            enemyTabFake.color = enemyTabNormalColor;
        }
        ManaTab.SetActive(!show);
        UnitTab.SetActive(show);
        allyTab.SetActive(show);
        enemyTab.SetActive(show);
    }
}
