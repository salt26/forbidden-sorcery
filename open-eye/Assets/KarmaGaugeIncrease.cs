using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarmaGaugeIncrease : MonoBehaviour {

    [HideInInspector]
    public float pivotGuage = 0.5f; /*(float)GameManager.instance.karma / GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas
        [GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas.Count - 1].requiredKarma;*/

    public void ChangeKarmaGauge()
    {
        GetComponent<RectTransform>().localPosition = new Vector3 (0f, 300.5f - 552f + 552f * pivotGuage, 0f); 
    }
}
