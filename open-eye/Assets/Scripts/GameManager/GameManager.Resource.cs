using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    private int notoriety;
    private int karma;
    private int mana;

    private void InitializeResource()
    {
        mana = config.baseMana;
        notoriety = config.baseNotoriety;
        karma = 0;
    }

    private void UpkeepResources()
    {
        karma += notoriety;
        mana += manaProduce;
    }
}
