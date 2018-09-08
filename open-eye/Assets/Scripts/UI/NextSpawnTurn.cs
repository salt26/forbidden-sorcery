using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextSpawnTurn : MonoBehaviour {

    [SerializeField]
    private Text text;
	public void CalculateNextTurn(int notoriety)
    {
        int rk = GameManager.instance.nextSpawnData.requiredKarma;
        int ck = GameManager.instance.karma;
        if (ck >= rk)
        {
            ck = rk;
        }
        int nt = (rk - ck) / notoriety;
        text.text = string.Format("{0}", nt);
    }
}
