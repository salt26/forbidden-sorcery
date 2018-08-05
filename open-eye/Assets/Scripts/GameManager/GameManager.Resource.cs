using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    public int notoriety;
    public int karma;
    public int mana;

    private void InitializeResource()
    {
        mana = config.baseMana;
        notoriety = config.baseNotoriety;
        karma = 0;
    }
}
