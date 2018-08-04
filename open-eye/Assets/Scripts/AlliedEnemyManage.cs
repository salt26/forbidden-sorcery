﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlliedEnemyManage : MonoBehaviour {
    [SerializeField]
    int classify;//1 for dominate, 2 for kill, 3 for free
    List<Button> destroyEnemySelectButtons;
    

    private void Start()
    {
        this.destroyEnemySelectButtons = Manager.manager.destroyEnemySelectButtons;
    }

    public void OnButtonClick()
    {
        Debug.Log("destroyEnemySelectButtons.Count : " + destroyEnemySelectButtons.Count);
        if (classify == 1)
        {
            foreach (Button button in destroyEnemySelectButtons)
            {
                UnitData ud = button.GetComponent<UnitButton>().Unit.unitData;
                ud = new UnitData(ud.kind, true, ud.attack, ud.health, ud.movableLength, ud.cost);
                Manager.manager.allyPrefab.Add(ud);
                Manager.manager.nodeDestroyedEnemies.Remove(button.GetComponent<UnitButton>().Unit);
                ScrollViewContent.scrollViewContent.Remove(button);
                Destroy(button.GetComponent<UnitButton>().Unit.gameObject);
                Manager.manager.destroyedEnemyCount--;
            }
            destroyEnemySelectButtons.Clear();
        }
        else if (classify == 2)
        {
            foreach (Button button in destroyEnemySelectButtons)
            {
                Manager.manager.spell += 3;
                Manager.manager.notoriety += 1;
                Manager.manager.nodeDestroyedEnemies.Remove(button.GetComponent<UnitButton>().Unit);
                ScrollViewContent.scrollViewContent.Remove(button);
                Destroy(button.GetComponent<UnitButton>().Unit.gameObject);
                Manager.manager.destroyedEnemyCount--;
            }
            destroyEnemySelectButtons.Clear();
        }
        else if (classify == 3)
        {
            foreach (Button button in destroyEnemySelectButtons)
            {
                Manager.manager.notoriety -= Manager.manager.notoriety < 3 ? Manager.manager.notoriety : 3;
                Manager.manager.nodeDestroyedEnemies.Remove(button.GetComponent<UnitButton>().Unit);
                ScrollViewContent.scrollViewContent.Remove(button);
                Destroy(button.GetComponent<UnitButton>().Unit.gameObject);
                Manager.manager.destroyedEnemyCount--;
            }
            destroyEnemySelectButtons.Clear();
        }
    }
}
