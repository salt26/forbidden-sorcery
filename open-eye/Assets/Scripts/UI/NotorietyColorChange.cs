using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotorietyColorChange : MonoBehaviour {

    [HideInInspector]
    public float notorietyPivotGuage;

	public void ChangeNotorietyColor()
    {
        notorietyPivotGuage = (GameManager.instance.maxNotoriety < GameManager.instance.notoriety) ?
        1.0f : (float) GameManager.instance.notoriety / GameManager.instance.maxNotoriety;

        GetComponent<Image>().color = new Color(Mathf.Lerp((float)0xA7 / 255, (float)0x79 / 255, notorietyPivotGuage),
        Mathf.Lerp((float)0xCA / 255, (float)0x39 / 255, notorietyPivotGuage),
        Mathf.Lerp((float)0xE9 / 255, (float)0x93 / 255, notorietyPivotGuage));
    }
}
