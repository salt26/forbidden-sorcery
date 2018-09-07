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

    void WindowsDisable()
    {
        bottomWindow.SetActive(false);
        right1Window.SetActive(false);
        right2Window.SetActive(false);
        right3Window.SetActive(false);
    }

    void Awake()
    {
        GameObject.Find("Main Camera").GetComponent<Transform>().position = (cameraLocation);
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
        WindowExplanation(bottomWindow, "적 용사들은 곧 마왕님은 토벌하기 위해 몰려올 것입니다. 마왕님은 아군 유닛들을 통솔하여 용사들의 공격을 막아야 합니다. 마지막까지 살아남아 마왕성을 적들로부터 지켜냅시다.");
    }
    /*
    public void Phase2Turns()
    {
        phaseNumber = 2;
        WindowExplanation(bottomWindow, "적과의 대결은 크게 '적군 이동', '아군 관리', '전투' 단계로 구성됩니다. 적군이 이동하면 마왕님은 그에 대응하여 아군을 배치하면 됩니다. 그 과정에서 아군이 적과 맞닥뜨리면 전투가 펼쳐질 것입니다. 자세한 과정은 차차 설명해 드리겠습니다.");
    }

    public void Phase3Map()
    {
        phaseNumber = 3;
        WholeScreen.GetComponent<WholeScreenScript>().explanationOrder = 0;     //이것들은 설명이 여러 단계일 때 사용
        WindowExplanation(bottomWindow, "이 지역 지도는 지금 보시는 것과 같이 영토들이 배치되어 있습니다. 영토 간 이동은 정해진 길을 따라서만 할 수 있습니다.");
    }
    */
    public void Phase4Mana()
    {
        phaseNumber = 4;
        WindowExplanation(bottomWindow, "마왕님은 주로 두 개의 요소를 관리하게 될 것입니다.");
    }

    public void Phase5Karma()
    {
        phaseNumber = 5;
        //카르마 게이지에 네모 침
        WindowExplanation(bottomWindow, "두 번째는 카르마입니다. 카르마는 마왕이 지금까지 세계에 얼마나 악명을 떨쳤는지를 나타내는 수치입니다. 마왕님의 화면 우측에 게이지 형식으로 표현되어 있습니다. 카르마가 증가할수록 마왕님을 토벌하려는 용사들이 나타날 것입니다.");
    }

    public void Phase6Produce()
    {
        phaseNumber = 6;
        WindowExplanation(bottomWindow, "이제 본격적으로 유닛들을 통솔하는 방법에 대해 말씀드리겠습니다. 우선 유닛을 생산하는 방법입니다.");
    }

    public void Phase7UnitStats()
    {
        //마왕성에 네모 침
        phaseNumber = 7;
        WindowExplanation(bottomWindow, "영토를 클릭하면 해당 영토에 있는 유닛들을 볼 수 있습니다. 유닛의 정보가 좌측에 차례대로 표시됩니다.");
    }

    public void Phase8Move()
    {
        //목록 가장 위에 있는 유닛이 회색으로 바뀜
        phaseNumber = 8;
        WindowExplanation(bottomWindow, "아군 유닛 목록에서 이동력이 남아 있는 유닛들은 선택하여 이동시킬 수 있습니다.\n이동시키고 싶은 유닛들을 클릭하면 회색으로 바뀌고, 이 상태에서 다른 노드를 클릭하면 유닛들이 이동합니다.");
    }

    public void Phase9Fight()
    {
        phaseNumber = 9;
        WindowExplanation(bottomWindow, "아군 유닛과 적군 유닛이 같은 노드에 있을 때 전투 페이즈가 시작되면 전투가 발생합니다.");
    }

    public void Phase10EnemyControl()
    {
        phaseNumber = 10;
    }

    public void Phase11Occupation()
    {
        phaseNumber = 11;
    }

    public void Phase12ManaProduce()
    {
        phaseNumber = 12;
    }

    public void Phase13Superiority()
    {
        phaseNumber = 13;
    }

    public void Phase14EnemyWava()
    {
        phaseNumber = 14;
    }

    public void Phase15Defeat()
    {
        phaseNumber = 15;
        WindowExplanation(bottomWindow, "적들이 마왕성에까지 쳐들어와 전투를 한 끝에 마왕성이 적들에게 점령당한다면 마왕님의 패배입니다. 마왕성은 파괴되고 마왕님 또한 무사하지 못할 것입니다.");
    }

    public void Phase16FinalWave()
    {
        phaseNumber = 16;
        WindowExplanation(bottomWindow, "카르마 게이지가 가득 차게 되면 적들의 마지막 침공이 시작됩니다.");
    }

    public void Phase17Victory()
    {
        phaseNumber = 17;
        WindowExplanation(bottomWindow, "적들의 마지막 침공까지도 막아내면 마왕님의 승리입니다.");
    }

    public void Phase18()
    {
        WindowExplanation(bottomWindow, "이상으로 설명은 끝입니다.\n마왕님, 건투를 빕니다.");
    }
}
