using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    [HideInInspector]
    public int notoriety;
    private int karma;
    [HideInInspector]
    public int mana;

    private void InitializeResource()
    {
        mana = config.baseMana;
        notoriety = config.baseNotoriety;
        karma = 0;
    }
}
