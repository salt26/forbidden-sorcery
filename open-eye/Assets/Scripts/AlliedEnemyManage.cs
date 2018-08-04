using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlliedEnemyManage : MonoBehaviour {
    [SerializeField]
    int classify;//1 for dominate, 2 for kill, 3 for free
    List<Button> destroyEnemySelectButtons;
    

    private void Start()
    {
        this.destroyEnemySelectButtons = GameManager.instance.destroyEnemySelectButtons;
    }

    public void OnButtonClick()
    {
        Debug.Log("destroyEnemySelectButtons.Count : " + destroyEnemySelectButtons.Count);
        if (classify == 1)
        {
            foreach (Button button in destroyEnemySelectButtons)
            {
                UnitData ud = button.GetComponent<UnitButton>().Unit.unitData;
                GameManager.instance.allyPrefab.Add(ud);
                GameManager.instance.nodeDestroyedEnemies.Remove(button.GetComponent<UnitButton>().Unit);
                ScrollViewContent.scrollViewContent.Remove(button);
                Destroy(button.GetComponent<UnitButton>().Unit.gameObject);
                GameManager.instance.destroyedEnemyCount--;
            }
            destroyEnemySelectButtons.Clear();
        }
        else if (classify == 2)
        {
            foreach (Button button in destroyEnemySelectButtons)
            {
                GameManager.instance.mana += 3;
                GameManager.instance.notoriety += 1;
                GameManager.instance.nodeDestroyedEnemies.Remove(button.GetComponent<UnitButton>().Unit);
                ScrollViewContent.scrollViewContent.Remove(button);
                Destroy(button.GetComponent<UnitButton>().Unit.gameObject);
                GameManager.instance.destroyedEnemyCount--;
            }
            destroyEnemySelectButtons.Clear();
        }
        else if (classify == 3)
        {
            foreach (Button button in destroyEnemySelectButtons)
            {
                GameManager.instance.notoriety -= GameManager.instance.notoriety < 3 ? GameManager.instance.notoriety : 3;
                GameManager.instance.nodeDestroyedEnemies.Remove(button.GetComponent<UnitButton>().Unit);
                ScrollViewContent.scrollViewContent.Remove(button);
                Destroy(button.GetComponent<UnitButton>().Unit.gameObject);
                GameManager.instance.destroyedEnemyCount--;
            }
            destroyEnemySelectButtons.Clear();
        }
    }
}
