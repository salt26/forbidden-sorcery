using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDataContainer : ScriptableObject
{
    [System.Serializable]
    public class EnemyData
    {
        public int requiredNotoriety;
        public string enemyStatus;
        public List<string> enemySpawnNodes;
    }

    [System.Serializable]
    public class EnemySpawnData
    {
        public int requiredKarma;
        public List<EnemyData> enemyDatas;

        public EnemySpawnData()
        {
            requiredKarma = int.MaxValue;
            enemyDatas = null;
        }
    }

    public List<EnemySpawnData> enemySpawnDatas;

    public EnemySpawnData GetNextEnemySpawnData(int karma)
    {
        EnemySpawnData data = new EnemySpawnData();

        foreach (var spawnData in enemySpawnDatas)
        {
            if (spawnData.requiredKarma > karma && spawnData.requiredKarma < data.requiredKarma)
            {
                data = spawnData;
            }
        }

        return data;
    }
}
