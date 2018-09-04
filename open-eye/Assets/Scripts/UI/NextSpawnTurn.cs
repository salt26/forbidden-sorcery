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
        Debug.Log(rk);
        Debug.Log(ck);
        Debug.Log(GameManager.instance.notoriety);
        Debug.Log(nt);

        text.text = nt >= 0 ? string.Format("leftturn \n{0}", nt) : string.Format("spawned!");
    }
}
