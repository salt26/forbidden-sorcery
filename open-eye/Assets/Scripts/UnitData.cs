using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : ScriptableObject
{
    public string kind;
    public bool isAlly;
    public int attack;
    public int health;
    public int movableLength;
    public int cost;

    public UnitData(string kind, bool isAlly, int attack, int health, int movableLength, int cost)
    {
        this.kind = kind;
        this.isAlly = isAlly;
        this.attack = attack;
        this.health = health;
        this.movableLength = movableLength;
        this.cost = cost;
    }
}