using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotorietyColorChange : MonoBehaviour {
	public void ChangeNotorietyColor()
    {
        GetComponent<Image>().color = new Color(Mathf.Lerp((float)0xA7 / 255, (float)0x79 / 255, GameObject.Find("KarmaGaugeMid").GetComponent<KarmaGaugeIncrease>().pivotGuage),
           Mathf.Lerp((float)0xCA / 255, (float)0x39 / 255, GameObject.Find("KarmaGaugeMid").GetComponent<KarmaGaugeIncrease>().pivotGuage),
           Mathf.Lerp((float)0xE9 / 255, (float)0x93 / 255, GameObject.Find("KarmaGaugeMid").GetComponent<KarmaGaugeIncrease>().pivotGuage));
    }
}
