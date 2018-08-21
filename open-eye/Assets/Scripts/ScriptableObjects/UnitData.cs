using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : ScriptableObject
{

    public enum MoveType
    {
        directToCastle,
        nearTerritory
    }

    public enum HeroType
    {
        soldier,
        tanker,
        mage,
        archer,
        assassin
    }

    public HeroType herotype;

    public MoveType currentMoveType;
    public string unitName;
    public string spriteName;
    public string iconName;
    public int attack;
    public int health;
    public int aggro;
    public int movement;
    public int cost;
}