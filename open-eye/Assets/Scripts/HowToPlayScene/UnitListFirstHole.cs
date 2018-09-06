using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitListFirstHole : MonoBehaviour {

    public void BeingClicked()
    {
        int nowPhase = GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().phaseNumber;
        if (nowPhase == 6)
        {
            GameObject.Find("UnitListItem(Clone)").GetComponent<UnitListItem>().OnItemClick();
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().WindowExplanation(GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().bottomWindow, "마왕성에서 유닛이 생산됩니다.");
            GameObject.Find("UnitListFirstHole").SetActive(false);
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().WholeScreen.SetActive(true);
        }
    }
}
