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
    
    public Color normalColor;
    public Color disabledColor;

    [SerializeField]
    KindOfButton kind;
    [SerializeField]
    Text text;
    public KindOfButton kindOfButton { get; private set; }

    public bool isSelected { get; set; }

    List<Unit> selectedDestroyedEnemyList;
    public int dominateNotorietychange;
    public int dominateManaChange;
    public int killNotorietyChange;
    public int killManaChange;
    public int freeNotorietyChange;
    public int freeManaChange;

    private void Awake()
    {
        kindOfButton = kind;
        isSelected = false;
        selectedDestroyedEnemyList = new List<Unit>();
    }

    public void OnClick()
    {
        ClickSoundManager.instance.PlaySound();
        GameManager.instance.destroyedEnemyControlScrollView = GetComponent<DestroyedEnemyControlScrollView>();
        GameManager.instance.OnClickDestroyedEnemyControlButton(selectedDestroyedEnemyList, this);
    }

    public void OnClickResetButton()
    {
        ClickSoundManager.instance.PlaySound();
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

    public void Clear()
    {
        selectedDestroyedEnemyList.ForEach((item) => Destroy(item.gameObject));
        selectedDestroyedEnemyList.Clear();
    }

    private void Dominate()
    {
        foreach (Unit u in selectedDestroyedEnemyList)
        {
            if (GameManager.instance.numberOfProducableAlliedEnemies.ContainsKey(u.unitData))
                GameManager.instance.numberOfProducableAlliedEnemies[u.unitData] += 1;
            else
                GameManager.instance.numberOfProducableAlliedEnemies[u.unitData] = 1;

            GameManager.instance.Mana -= u.unitData.cost / 4;
            GameManager.instance.notoriety += u.unitData.level * 2;
        }
    }

    private void Kill()
    {
        foreach (Unit u in selectedDestroyedEnemyList)
        {
            GameManager.instance.Mana += u.unitData.cost / 2 + u.unitData.level * 100;
            GameManager.instance.notoriety += u.unitData.level * 3;
        }
    }

    private void Free()
    {
        foreach (Unit u in selectedDestroyedEnemyList)
        {
            //TODO
        }
    }
    
    public void ShowExpectedResourceChange()
    {
        switch ((int)kindOfButton)
        {
            case 0:
                text.text = string.Format("{0}", dominateManaChange);
                break;
            case 1:
                text.text = string.Format("{0}", killManaChange);
                break;
            case 2:
                text.text = string.Format("{0}", freeManaChange);
                break;
        }
    }
}