using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatusText : MonoBehaviour {
    private void FixedUpdate()
    {
        this.GetComponent<Text>().text = "spell : " + Manager.manager.spell + "\nkarma : " + Manager.manager.karma + "\nnotoriety : " + Manager.manager.notoriety + "\nnext wave : " + Manager.manager.enemySpawnBound + "\nTurn : " + Manager.manager.whichTurn;
    }
}
