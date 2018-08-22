using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Comparer : IComparer<IUnitInterface>
{
    public int Compare(IUnitInterface left, IUnitInterface right)
    {
        if (left.UD.aggro < right.UD.aggro) return 1;
        if (left.UD.health < right.UD.health) return -1;
        if (left.UD.movement < right.UD.movement) return 1;
        return 0;
    }
}

public class ComparerAssassin : IComparer<IUnitInterface>
{
    public int Compare(IUnitInterface left, IUnitInterface right)
    {
        if (left.UD.aggro < right.UD.aggro) return -1;
        if (left.UD.health < right.UD.health) return 1;
        if (left.UD.movement < right.UD.movement) return -1;
        return 0;
    }
}

class Fight
{
    static Comparer comparer = new Comparer();
    static ComparerAssassin comparerAssassin = new ComparerAssassin();
    public static ExpectedFightResult Fighting(List<Unit> us)
    {
        ExpectedFightResult result = new ExpectedFightResult();
        foreach (Unit u in us)
        {
            ImaginaryUnit iu = new ImaginaryUnit();
            iu.isAlly = u.isAlly;
            iu.ID = u.ID;
            iu.Movement = u.Movement;
            iu.CurrentHealth = u.CurrentHealth;
            iu.UD = u.UD;
            result.unitList.Add((iu as IUnitInterface));
        }
        result.unitList.Sort(comparerAssassin);
        if (result.unitList.Count > 0)
        {
            int allyAttack = 0;
            int enemyAttack = 0;
            int allyAssassinAttack = 0;
            int enemyAssassinAttack = 0;

            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (ally.UD.herotype == UnitData.HeroType.assassin)
                {
                    allyAssassinAttack += ally.UD.attack;
                }
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly && unit.UD.health > 0 &&
            unit.UD.herotype != UnitData.HeroType.mage && unit.UD.herotype != UnitData.HeroType.assassin))
            {
                if (enemy.UD.herotype == UnitData.HeroType.assassin)
                {
                    enemyAssassinAttack += enemy.UD.attack;
                }
            }

            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (enemyAssassinAttack < ally.CurrentHealth)
                {
                    ally.GetDamaged(enemyAssassinAttack);
                    enemyAssassinAttack = 0;
                }
                else
                {
                    enemyAssassinAttack -= ally.CurrentHealth;
                    ally.CurrentHealth = 0;
                }
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
            {
                if (allyAssassinAttack < enemy.CurrentHealth)
                {
                    enemy.GetDamaged(allyAssassinAttack);
                    allyAssassinAttack = 0;
                }
                else
                {
                    allyAssassinAttack -= enemy.CurrentHealth;
                    enemy.CurrentHealth = 0;
                }
            }

            result.unitList.Sort(comparer);

            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (ally.UD.herotype != UnitData.HeroType.mage && ally.UD.herotype != UnitData.HeroType.assassin && ally.UD.health > 0)
                {
                    allyAttack += ally.UD.attack;
                }
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly && unit.UD.health > 0 &&
            unit.UD.herotype != UnitData.HeroType.mage && unit.UD.herotype != UnitData.HeroType.assassin))
            {
                if (enemy.UD.herotype != UnitData.HeroType.mage && enemy.UD.herotype != UnitData.HeroType.assassin && enemy.UD.health > 0)
                {
                    enemyAttack += enemy.UD.attack;
                }
            }

            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (enemyAttack < ally.CurrentHealth)
                {
                    ally.GetDamaged(enemyAttack);
                    enemyAttack = 0;
                }
                else
                {
                    enemyAttack -= ally.CurrentHealth;
                    ally.CurrentHealth = 0;
                }
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
            {
                if (allyAttack < enemy.CurrentHealth)
                {
                    enemy.GetDamaged(allyAttack);
                    allyAttack = 0;
                }
                else
                {
                    allyAttack -= enemy.CurrentHealth;
                    enemy.CurrentHealth = 0;
                }
            }

            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly && unit.UD.health > 0)) // 아군 마법사 공격
            {
                if (ally.UD.herotype == UnitData.HeroType.mage)
                {
                    foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
                    {
                        enemy.GetDamaged(ally.UD.attack);
                    }
                }
            }


            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly && unit.UD.health > 0)) // 적군 마법사 공격
            {
                if (enemy.UD.herotype == UnitData.HeroType.mage)
                {
                    foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
                    {
                        ally.GetDamaged(enemy.UD.attack);
                    }
                }
            }
        }
        return result;
    }
}
