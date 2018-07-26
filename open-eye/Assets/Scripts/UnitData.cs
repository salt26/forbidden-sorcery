using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    [SerializeField]
    string kind;
    [SerializeField]
    bool isAlly;
    [SerializeField]
    int attack;
    [SerializeField]
    int health;
    [SerializeField]
    int movableLength;
    public UnitData(string kind, bool isAlly, int attack, int health)
    {
        this.kind = kind;
        this.isAlly = isAlly;
        this.attack = attack;
        this.health = health;
    }

    public string Kind
    {
        get
        {
            return kind;
        }

        set
        {
            kind = value;
        }
    }

    public bool IsAlly
    {
        get
        {
            return isAlly;
        }

        set
        {
            isAlly = value;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }

        set
        {
            attack = value;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public int MovableLength
    {
        get
        {
            return movableLength;
        }

        set
        {
            movableLength = value;
        }
    }
}