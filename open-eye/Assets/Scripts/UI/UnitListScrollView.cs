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

    public void SetUnitList(List<Unit> unitList, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();

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
        transform.localPosition = show ? contentActive : contentInactive;
        nowListShown = show;
    }
}
