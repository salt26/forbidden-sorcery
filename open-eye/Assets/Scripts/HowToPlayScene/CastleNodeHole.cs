using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleNodeHole : MonoBehaviour {

    public GameObject castleNode;

    public void BeingClicked()
    {
        int nowPhase = GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().phaseNumber;
        if (nowPhase == 7)
        {
            Debug.Log("asd");
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().WindowExplanation(GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().bottomWindow, "유닛의 직업, 공격력, 체력, 어그로 수치가 표시됩니다.");
            castleNode.GetComponent<Node>().OnMouseUpAsButton();
            
            GameObject.Find("CastleNodeHole").SetActive(false);
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().WholeScreen.SetActive(true);
        }
    }
}
