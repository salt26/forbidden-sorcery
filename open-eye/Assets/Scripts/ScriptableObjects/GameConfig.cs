using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : ScriptableObject
{
    public int baseMana;
    public int baseNotoriety;
    public EnemySpawnDataContainer enemySpawnDataContainer;
    public List<UnitData> producableUnits;
}
