using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitListScrollView : MonoBehaviour
{
    [SerializeField]
    Transform content;

    private List<UnitListItem> listItems = new List<UnitListItem>();

    public void SetUnitList(List<Unit> unitList, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();

        foreach (var unit in unitList)
        {
            var listItemObject = Instantiate(AssetManager.Instance.GetPrefab("UnitListItem"), content);
            var unitListItem = listItemObject.GetComponent<UnitListItem>();
            unitListItem.SetUnit(unit, onClick);
            listItems.Add(unitListItem);
        }
    }

    public void SetUnitDataList(List<UnitData> unitDataList, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();

        foreach (var unitData in unitDataList)
        {
            var listItemObject = Instantiate(AssetManager.Instance.GetPrefab("UnitListItem"), content);
            var unitListItem = listItemObject.GetComponent<UnitListItem>();
            unitListItem.SetUnitData(unitData, onClick);
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
        gameObject.SetActive(show);
    }
}
