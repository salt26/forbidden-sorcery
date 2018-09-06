using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceButtonHole : MonoBehaviour {

	public void BeingClicked()
    {
        int nowPhase = GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().phaseNumber;
        if (nowPhase == 6)
        {
            GameManager.instance.OnClickProduceButton();
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().WindowExplanation(GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().bottomWindow, "가장 위에 있는 유닛을 클릭해 보십시오");
            GameObject.Find("ProduceButtonHole").SetActive(false);
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().UnitListFirstHole.SetActive(true);
        }
    }
}
