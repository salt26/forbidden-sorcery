using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyedEnemyControlButton : MonoBehaviour
{
    public enum KindOfButton
    {
        dominate,
        kill,
        free
    }

    [SerializeField]
    KindOfButton kind;
    public KindOfButton kindOfButton { get; private set; }

    public bool isSelected { get; set; }

    List<Unit> selectedDestroyedEnemyList;

    private void Awake()
    {
        kindOfButton = kind;
        isSelected = false;
        selectedDestroyedEnemyList = new List<Unit>();
    }

    public void OnClick()
    {
        GameManager.instance.destroyedEnemyControlScrollView = GetComponent<DestroyedEnemyControlScrollView>();
        List<Unit> managerDestroyedEnemies = GameManager.instance.SelectedDestroyedEnemyList;
        if (managerDestroyedEnemies != null && managerDestroyedEnemies.Equals(selectedDestroyedEnemyList))
        {
            GameManager.instance.OnClickDestroyedEnemyControlButton(null, this);
        }
        else
        {
            GameManager.instance.OnClickDestroyedEnemyControlButton(selectedDestroyedEnemyList, this);
        }
    }

    public void OnClickResetButton()
    {
        GameManager.instance.OnClickRefreshControlButton();
    }

    public void Fetch()
    {
        switch ((int)kindOfButton)
        {
            case 0:
                Dominate();
                break;
            case 1:
                Kill();
                break;
            case 2:
                Free();
                break;
        }

    }
    
    private void Dominate()
    {
        foreach (Unit u in selectedDestroyedEnemyList)
        {
            GameManager.instance.config.producableUnits.Add(u.unitData);
        }
    }

    private void Kill()
    {

    }

    private void Free()
    {

    }
}