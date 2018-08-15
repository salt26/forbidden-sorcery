using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarmaGaugeIncrease : MonoBehaviour {

    float pivotGuage = 0.5f; //임시 값, GameManager.instance.karma / (최후의 레이드 필요 업보)로 바꿔줘요

    public void ChangeKarmaGauge()
    {
        GetComponent<RectTransform>().localPosition = new Vector3 (0f, 300.5f - 552f + 552f * pivotGuage, 0f); 
    }
}
