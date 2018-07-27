using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    public bool isForStatus;
    private void Start()
    {
        if (!isForStatus)
            Manager.manager.AHText = this;
    }
    private void FixedUpdate()
    {
        if (isForStatus)
            this.GetComponent<Text>().text = "spell : " + Manager.manager.spell + "\nkarma : " + Manager.manager.karma + "\nnotoriety : " + Manager.manager.notoriety + "\nnext wave : " + Manager.manager.enemySpawnBound + "\nTurn : " + Manager.manager.whichTurn;
    }

    public void stateAH(bool clear, string attack, string health, string movableLength)
    {
        if (!isForStatus && !clear)
        {
            GetComponent<Text>().text = "Attack : " + attack + "\nHealth : " + health + "\nMovableLength : " + movableLength;
        }
        else if(!isForStatus && clear)
        {
            GetComponent<Text>().text = "";
        }
    }
}
