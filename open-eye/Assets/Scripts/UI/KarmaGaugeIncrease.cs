using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KarmaGaugeIncrease : MonoBehaviour {

    [HideInInspector]
    public float pivotGuage;

    [HideInInspector]
    public float NextTurnpivotGuage;

    [HideInInspector]
    public float NextEnemySpawnPivotGuage;
    private int HowManySpawned = 0;

    public void ChangeKarmaGauge()
    {
        pivotGuage = (float)GameManager.instance.karma / GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas
        [GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas.Count - 1].requiredKarma;

        NextTurnpivotGuage = GameManager.instance.karma + (float)GameManager.instance.notoriety / GameManager.instance
        .config.enemySpawnDataContainer.enemySpawnDatas[GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas.Count - 1].requiredKarma;
        GetComponent<RectTransform>().localPosition = new Vector3 (0f, 300.5f - 552f + 552f * pivotGuage, 0f);

        if(GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas[HowManySpawned].requiredKarma < GameManager.instance.karma)
        {
            HowManySpawned++;
        }
        NextEnemySpawnPivotGuage = (float)GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas
        [HowManySpawned].requiredKarma / GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas
        [GameManager.instance.config.enemySpawnDataContainer.enemySpawnDatas.Count - 1].requiredKarma;

        GameObject.Find("NextTurnKarma").GetComponent<RectTransform>().localPosition = new Vector3(0f, 552f * NextTurnpivotGuage - 552f, 0f);
        GameObject.Find("NextEnemySpawnKarma").GetComponent<RectTransform>().localPosition = new Vector3(0f, 552f * NextEnemySpawnPivotGuage - 552f, 0f);
    }
}
