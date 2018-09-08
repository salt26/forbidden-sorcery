using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextSpawnTurn : MonoBehaviour {

    [SerializeField]
    private Text text;
	public void CalculateNextTurn()
    {
        int rk = GameManager.instance.nextSpawnData.requiredKarma;
        int ck = GameManager.instance.karma;
        int nt = (rk - ck) / GameManager.instance.notoriety;

        if (nt > 100000000 || GameManager.instance.karma >= GameManager.instance.
            config.enemySpawnDataContainer.enemySpawnDatas[GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas.Count - 1].requiredKarma) text.text = string.Format("");
        else text.text = nt >= 0 ? string.Format("leftturn \n{0}", nt) : string.Format("spawned!");
    }
}
