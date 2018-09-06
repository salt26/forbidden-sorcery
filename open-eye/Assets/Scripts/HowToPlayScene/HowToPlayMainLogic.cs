using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMainLogic : MonoBehaviour {

    Vector3 cameraLocation = new Vector3(-6.259621f, -1.076924f, -10);

    public int phaseNumber;

    public void WindowExplanation(GameObject window, string explanation)
    {
        window.SetActive(true);
        window.transform.Find("Text").GetComponent<Text>().text = explanation;
    }

    public GameObject bottomWindow, right1Window, right2Window, right3Window;

    public GameObject WholeScreen;
    
    public GameObject ProduceButtonHole, NextButtonHole, UnitListFirstHole, DominateHole, CastleNodeHole, AnotherNodeHole;

    void WindowsDisable()
    {
        bottomWindow.SetActive(false);
        right1Window.SetActive(false);
        right2Window.SetActive(false);
        right3Window.SetActive(false);
    }

    void Awake()
    {
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetDestination(cameraLocation);
        WindowExplanation(bottomWindow, "Hello, World!");
        WindowsDisable();
        Phase0();
    }

    void Phase0()
    {
        phaseNumber = 0;
        Phase1Objective();
    }

    void Phase1Objective()
    {
        phaseNumber = 1;
        WindowExplanation(bottomWindow, "몰려오는 적들을 막아 마왕성을 지켜야 합니다");
    }

    public void Phase2Turns()
    {
        phaseNumber = 2;
        WindowExplanation(bottomWindow, "턴은 적군이동 -> 아군이동 -> 전투 로 진행됩니다");
    }

    public void Phase3Map()
    {
        phaseNumber = 3;
        WindowExplanation(bottomWindow, "맵은 마왕성과 노드들로 구성됩니다.");
    }

    public void Phase4Mana()
    {
        phaseNumber = 4;
        WindowExplanation(bottomWindow, "마나는 생산과 관련된 자원입니다");
    }

    public void Phase5Notoriety()
    {
        phaseNumber = 5;
        WindowExplanation(bottomWindow, "업보는 적들의 움직임과 관련이 있습니다");
    }

    public void Phase6Produce()
    {
        phaseNumber = 6;
        WindowExplanation(bottomWindow, "이제 유닛을 생성해 보겠습니다. Produce 버튼을 눌러 주십시오.");
        WholeScreen.SetActive(false);
        ProduceButtonHole.SetActive(true);
    }
    
    public void Phase7Units()
    {
        phaseNumber = 7;
        WindowExplanation(bottomWindow, "노드를 클릭하면 해당 노드에 있는 유닛들의 정보를 볼 수 있습니다.");
        CastleNodeHole.SetActive(true);
    }

    public void Phase8Move()
    {
        phaseNumber = 8;
    }

    public void Phase9Fight()
    {
        phaseNumber = 9;
    }

    public void Phase10EnemyTreatment()
    {
        phaseNumber = 10;
    }

    public void Phase11Conquer()
    {
        phaseNumber = 11;
    }

    public void Phase12Superiority()
    {
        phaseNumber = 12;
    }

    public void Phase13ManaProduce()
    {
        phaseNumber = 13;
    }

    public void Phase14EnemyInvade()
    {
        phaseNumber = 14;
    }

    public void Phase15Defeat()
    {
        phaseNumber = 15;
    }

    public void Phase16FinalWave()
    {
        phaseNumber = 16;
    }

    public void Phase17Victory()
    {
        phaseNumber = 17;
    }
}
