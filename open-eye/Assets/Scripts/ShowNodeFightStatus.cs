using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNodeFightStatus : MonoBehaviour {
    public void ShowExpectedFightResult()
    {
        List<Unit> nodeUnits = GetComponentInParent<Node>().units;
        ExpectedFightResult now = new ExpectedFightResult();
        now.unitList = nodeUnits.ConvertAll<IUnitInterface>((u) => u as IUnitInterface);
        ExpectedFightResult after1Turn = ExpectedFightResult.ExpectFightResultAfterSomeTurn(now, 1);
        ExpectedFightResult after5Turn = ExpectedFightResult.ExpectFightResultAfterSomeTurn(now, 5);
        if(nodeUnits.FindAll((unit) => unit.isAlly).Count > 0)
        {
            if(nodeUnits.FindAll((unit) => !unit.isAlly).Count > 0)
            {
                //while fighting
                if(after1Turn.unitList.FindAll((unit) => unit.isAlly).FindAll((unit) => unit.CurrentHealth > 0).Count > 0 && after1Turn.unitList.FindAll((unit) => !unit.isAlly).FindAll((unit) => unit.CurrentHealth > 0).Count == 0)
                {
                    //will win
                    GetComponent<SpriteRenderer>().sprite = GameManager.instance.map.willWinNodeSprite;
                }
                else if(after1Turn.unitList.FindAll((unit) => unit.isAlly).FindAll((unit) => unit.CurrentHealth > 0).Count == 0 && after1Turn.unitList.FindAll((unit) => !unit.isAlly).FindAll((unit) => unit.CurrentHealth > 0).Count > 0)
                {
                    //will lose
                    GetComponent<SpriteRenderer>().sprite = GameManager.instance.map.willLoseNodeSprite;
                }
                else
                {
                    //longer than 1 turn or all die
                    if(after5Turn.unitList.FindAll((unit) => unit.isAlly).FindAll((unit) => unit.CurrentHealth > 0).Count > 0 && after5Turn.unitList.FindAll((unit) => !unit.isAlly).FindAll((unit) => unit.CurrentHealth > 0).Count == 0)
                    {
                        //Superiority
                        GetComponent<SpriteRenderer>().sprite = GameManager.instance.map.SuperiorityNodeSprite;
                    }
                    else if(after5Turn.unitList.FindAll((unit) => unit.isAlly).FindAll((unit) => unit.CurrentHealth > 0).Count == 0 && after5Turn.unitList.FindAll((unit) => !unit.isAlly).FindAll((unit) => unit.CurrentHealth > 0).Count > 0)
                    {
                        //inferiority
                        GetComponent<SpriteRenderer>().sprite = GameManager.instance.map.inferiorityNodeSprite;
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = GameManager.instance.map.fixedFightNodeSprite;
                    }
                }
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
