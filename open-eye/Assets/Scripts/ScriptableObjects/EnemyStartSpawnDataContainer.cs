using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartSpawnDataContainer : ScriptableObject {

    [System.Serializable]
    public class EnemyData
    {
        public string enemyStatus;
        public List<string> enemySpawnNodes;
    }
    public List<EnemyData> enemySpawnDatas;
}
