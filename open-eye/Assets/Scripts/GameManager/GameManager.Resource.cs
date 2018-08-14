using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    [SerializeField]
    private ManaAmount manaAmountText;

    private int notoriety;
    private int karma;

    private int mana;
    private int Mana
    {
        get
        {
            return mana;
        }
        set
        {
            mana = value;
            manaAmountText.SetManaAmount(mana, manaProduce);
        }
    }

    private int manaProduce
    {
        get
        {
            int produce = 0;
            foreach (var territory in territories)
            {
                produce += territory.manaValue;
            }
            return produce;
        }
    }
    
    private void InitializeResource()
    {
        Mana = config.baseMana;
        notoriety = config.baseNotoriety;
        karma = 0;
    }

    private void UpkeepResources()
    {
        karma += notoriety;
        Mana += manaProduce;
    }
}
