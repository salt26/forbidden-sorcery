using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedEnemyControlScrollView : MonoBehaviour
{
    [SerializeField]
    Transform content;

    private List<UnitListItem> listItems = new List<UnitListItem>();
    public void SetControlDestroyedEnemiesList(List<Unit> destroyedEnemies, UnitListItem.OnClickUnitListItem onClick = null)
    {
        ClearItem();
        //destroyedEnemies.Sort(new UnitSort.UnitMoveListCompare());
        foreach (var dEnemy in destroyedEnemies)
        {
            var listItemObject = Instantiate(AssetManager.Instance.GetPrefab("UnitListItemIcon"), content);
            var unitListItem = listItemObject.GetComponent<UnitListItem>();
            unitListItem.SetDestroyedEnemyDataForControlScrollView(dEnemy, onClick);
            listItems.Add(unitListItem);
        }
    }

    private void ClearItem()
    {
        listItems.ForEach((item) => Destroy(item.gameObject));
        listItems.Clear();
    }
}
