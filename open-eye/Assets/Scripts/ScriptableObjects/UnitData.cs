﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : ScriptableObject
{
    public enum HeroType
    {
        soldier,
        tanker,
        mage,
        archer,
        assassin
    }

    public HeroType herotype;

    public bool isHero;

    public string unitName;
    public string spriteName;
    public string iconName;
    public int attack;
    public int health;
    public int aggro;
    public int movement;
    public int cost;
    public int level;

    public override bool Equals(object obj)
    {
        var data = obj as UnitData;
        return data != null &&
               base.Equals(obj) &&
               herotype == data.herotype &&
               isHero == data.isHero &&
               unitName == data.unitName &&
               spriteName == data.spriteName &&
               iconName == data.iconName &&
               attack == data.attack &&
               health == data.health &&
               aggro == data.aggro &&
               movement == data.movement &&
               cost == data.cost &&
               level == data.level;
    }

    public override int GetHashCode()
    {
        var hashCode = 1819745940;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + herotype.GetHashCode();
        hashCode = hashCode * -1521134295 + isHero.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(unitName);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(spriteName);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(iconName);
        hashCode = hashCode * -1521134295 + attack.GetHashCode();
        hashCode = hashCode * -1521134295 + health.GetHashCode();
        hashCode = hashCode * -1521134295 + aggro.GetHashCode();
        hashCode = hashCode * -1521134295 + movement.GetHashCode();
        hashCode = hashCode * -1521134295 + cost.GetHashCode();
        hashCode = hashCode * -1521134295 + level.GetHashCode();
        return hashCode;
    }
}