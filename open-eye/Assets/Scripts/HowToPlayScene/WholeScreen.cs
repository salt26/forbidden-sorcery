using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WholeScreen : MonoBehaviour {

    public void BeingClicked()
    {
        int nowPhase = GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().phaseNumber;
        if (nowPhase == 1)
        {
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().Phase2Turns();
        }
        else if (nowPhase == 2)
        {
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().Phase3Map();
        }
        else if (nowPhase == 3)
        {
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().Phase4Mana();
        }
        else if (nowPhase == 4)
        {
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().Phase5Notoriety();
        }
        else if (nowPhase == 5)
        {
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().Phase6Produce();
        }
        else if (nowPhase == 6)
        {
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().Phase7Units();
        }
        else if (nowPhase == 7)
        {
            GameObject.Find("HowToPlay").GetComponent<HowToPlayMainLogic>().Phase8Move();
        }
    }
}
