using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class WholeScreenScript : MonoBehaviour {

    public int explanationOrder;

    [SerializeField]
    GameObject HowToPlay;

    HowToPlayMainLogic howToPlayMainLogic;

    public GameObject ally, enemy1, enemy2;
    
    public GameObject bottomWindow, right1Window, right2Window, right3Window;

    void Awake()
    {
        explanationOrder = 0;
        howToPlayMainLogic = HowToPlay.GetComponent<HowToPlayMainLogic>();
    }

    public void BeingClicked()
    {
        int phase = howToPlayMainLogic.phaseNumber;
        switch (phase)
        {
            case 1:
                howToPlayMainLogic.Phase4Mana();
                break;
                /*
            case 2:
                howToPlayMainLogic.Phase3Map();
                break;
            case 3:
                if (explanationOrder == 0)
                {
                    //마왕성에 너모 치기
                    howToPlayMainLogic.WindowExplanation(bottomWindow, "이 중 마왕성은 마왕님의 근거지로, 가장 우선적으로 지켜야 할 영토이자 아군 유닛이 생산되는 곳입니다.");
                    ++explanationOrder;
                }
                else if (explanationOrder == 1)
                {
                    //마왕성에 친 네모 사라짐
                    howToPlayMainLogic.Phase4Mana();
                    explanationOrder = 0;
                }
                break;
                */
            case 4:
                if(explanationOrder == 0)
                {
                    //마력에 네모 침
                    howToPlayMainLogic.WindowExplanation(bottomWindow, "첫 번째는 마력입니다. 마력은 유닛을 생산하는 데 소모되는 자원입니다. 마왕님의 화면 좌측 아래에 현재 마력(+ 다음 턴에 증가할 마력) 형식으로 표시되어 있습니다.");
                    ++explanationOrder;
                }
                else if (explanationOrder == 1)
                {
                    //마력에 친 네모 사라짐
                    howToPlayMainLogic.Phase5Karma();
                    explanationOrder = 0;
                }
                break;

            case 5:
                if(explanationOrder == 0)
                {
                    //카르마 게이지에 친 네모 사라짐
                    howToPlayMainLogic.WindowExplanation(bottomWindow, "카르마는 매 턴 증가하는데, 증가하는 속도는 마왕님의 행동에 따라 달라집니다.\n카르마가 증가하는 속도는 카르마 게이지를 둘러싼 오라의 색깔을 통해 파악할 수 있습니다.");
                    ++explanationOrder;
                }
                else if (explanationOrder == 1)
                {
                    howToPlayMainLogic.Phase6Produce();
                    explanationOrder = 0;
                }
                break;

            case 6:
                if (explanationOrder == 0)
                {
                    //Produce 버튼에 네모 치기
                    howToPlayMainLogic.WindowExplanation(bottomWindow, "Produce 버튼을 누르면 생산 가능 유닛이 표시됩니다.\n유닛을 누르면 해당 유닛에 표시된 만큼 마력이 줄어들고 마왕성에서 해당 유닛이 생산됩니다.");
                    ++explanationOrder;
                }
                else if (explanationOrder == 1)
                {
                    //Produce 버튼에 친 네모 사라짐
                    howToPlayMainLogic.Phase7UnitStats();
                    explanationOrder = 0;
                }
                break;

            case 7:
                if(explanationOrder == 0)
                {
                    //마왕성에 친 네모 사라짐
                    //마왕성에 있는 아군 유닛 리스트가 보임
                    howToPlayMainLogic.WindowExplanation(bottomWindow, "가장 왼쪽에 있는 아이콘은 각 유닛의 클래스를 나타냅니다. 클래스에는 검사, 방패병, 궁수, 마법사, 암살자가 있습니다.");
                    ++explanationOrder;
                }
                else if (explanationOrder == 1)
                {
                    howToPlayMainLogic.WindowExplanation(bottomWindow, "아이콘 오른쪽 아래에는 유닛의 레벨이 적혀 있습니다. 같은 클래스의 용사라도 레벨이 높을수록 스텟이 높습니다.");
                    ++explanationOrder;
                }
                else if (explanationOrder == 2)
                {
                    howToPlayMainLogic.WindowExplanation(bottomWindow, "그 오른쪽에는 유닛의 체력, 공격력, 어그로, 이동력이 표시됩니다. 어그로 수치가 높을수록 전투에서 적의 공격을 우선적으로 받게 됩니다. 이동력은 유닛이 현재 단계에서 이동할 수 있는 거리를 나타냅니다.");
                    ++explanationOrder;
                }
                else if (explanationOrder == 3)
                {
                    howToPlayMainLogic.Phase8Move();
                    explanationOrder = 0;
                }
                break;

            case 8:
                if (explanationOrder == 0)
                {
                    //유닛 이동 애니메이션 재생
                    howToPlayMainLogic.WindowExplanation(bottomWindow, "랠리 포인트를 찍을 수도 있습니다.(설명 필요)");
                    ++explanationOrder;
                }
                else if (explanationOrder == 1)
                {
                    howToPlayMainLogic.Phase9Fight();
                    explanationOrder = 0;
                }
                break;

            case 9:
                if(explanationOrder == 0)
                {
                    //전투 UI가 보여짐

                    ++explanationOrder;
                }
                break;

            case 10:
                if(explanationOrder == 0)
                {

                }
                break;

            case 11:
                if (explanationOrder == 0)
                {

                }
                break;

            case 12:
                if (explanationOrder == 0)
                {

                }
                break;

            case 13:
                if (explanationOrder == 0)
                {

                }
                break;

            case 14:
                if (explanationOrder == 0)
                {

                }
                break;

            case 15:
                howToPlayMainLogic.Phase16FinalWave();
                break;
            case 16:
                howToPlayMainLogic.Phase17Victory();
                break;
            case 17:
                howToPlayMainLogic.Phase18();
                break;
        }
    }
}
