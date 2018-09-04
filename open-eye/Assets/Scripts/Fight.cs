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
        if (left.UD.aggro > right.UD.aggro) return -1;
        if (left.CurrentHealth < right.CurrentHealth) return -1;
        if (left.CurrentHealth > right.CurrentHealth) return 1;
        if (left.UD.movement < right.UD.movement) return 1;
        if (left.UD.movement > right.UD.movement) return -1;
        return 0;
    }
}

public class ComparerAssassin : IComparer<IUnitInterface>
{
    public int Compare(IUnitInterface left, IUnitInterface right)
    {
        if (left.UD.aggro < right.UD.aggro) return -1;
        if (left.UD.aggro > right.UD.aggro) return 1;
        if (left.CurrentHealth < right.CurrentHealth) return 1;
        if (left.CurrentHealth > right.CurrentHealth) return -1;
        if (left.UD.movement < right.UD.movement) return -1;
        if (left.UD.movement > right.UD.movement) return 1;
        return 0;
    }
}

class Fight
{
    static Comparer comparer = new Comparer();
    static ComparerAssassin comparerAssassin = new ComparerAssassin();
    public static ExpectedFightResult Fighting(List<Unit> us)
    {
        bool isAllyExist = false;
        bool isEnemyExist = false;
        ExpectedFightResult result = new ExpectedFightResult();
        foreach (Unit u in us)
        {
            ImaginaryUnit iu = new ImaginaryUnit();
            iu.isAlly = u.isAlly;
            if (iu.isAlly) isAllyExist = true;
            else isEnemyExist = true;
            iu.ID = u.ID;
            iu.Movement = u.Movement;
            iu.CurrentHealth = u.CurrentHealth;
            iu.UD = u.UD;
            result.unitList.Add((iu as IUnitInterface));
        }

        if (result.unitList.Count > 0 && isAllyExist && isEnemyExist)
        {
            GameManager.instance.fightingNodeNumber++;
            int allyAssassinAttack = 0;
            int enemyAssassinAttack = 0;
            int allyAttack = 0;
            int enemyAttack = 0;
            int allyMageAttack = 0;
            int enemyMageAttack = 0;
            // Sort for Phase 1
            result.unitList.Sort(comparerAssassin);
            // Prepare for Fight Animation
            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (ally.UD.herotype == UnitData.HeroType.tanker && ally.UD.isHero)
                {
                    FightAnimationUI.isThereTanker[GameManager.instance.fightingNodeNumber] = true;
                }
                if (ally.UD.herotype == UnitData.HeroType.soldier && ally.UD.isHero)
                {
                    FightAnimationUI.isThereSoldier[GameManager.instance.fightingNodeNumber] = true;
                }
                if (ally.UD.herotype == UnitData.HeroType.archer && ally.UD.isHero)
                {
                    FightAnimationUI.isThereArcher[GameManager.instance.fightingNodeNumber] = true;
                }
                if (ally.UD.herotype == UnitData.HeroType.assassin)
                {
                    FightAnimationUI.isThereAssassin[GameManager.instance.fightingNodeNumber] = true;
                }
                if (ally.UD.herotype == UnitData.HeroType.mage)
                {
                    FightAnimationUI.isThereMage[GameManager.instance.fightingNodeNumber] = true;
                }
                if (ally.UD.herotype == UnitData.HeroType.tanker && !ally.UD.isHero)
                {
                    FightAnimationUI.isThereZombie[GameManager.instance.fightingNodeNumber] = true;
                }
                if (ally.UD.herotype == UnitData.HeroType.soldier && !ally.UD.isHero)
                {
                    FightAnimationUI.isThereWolf[GameManager.instance.fightingNodeNumber] = true;
                }
                if (ally.UD.herotype == UnitData.HeroType.archer && !ally.UD.isHero)
                {
                    FightAnimationUI.isThereSkeleton[GameManager.instance.fightingNodeNumber] = true;
                }
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
            {
                if (enemy.UD.herotype == UnitData.HeroType.tanker)
                {
                    FightAnimationUI.isThereEnemyTanker[GameManager.instance.fightingNodeNumber] = true;
                }
                if (enemy.UD.herotype == UnitData.HeroType.soldier)
                {
                    FightAnimationUI.isThereEnemySoldier[GameManager.instance.fightingNodeNumber] = true;
                }
                if (enemy.UD.herotype == UnitData.HeroType.archer)
                {
                    FightAnimationUI.isThereEnemyArcher[GameManager.instance.fightingNodeNumber] = true;
                }
                if (enemy.UD.herotype == UnitData.HeroType.assassin)
                {
                    FightAnimationUI.isThereEnemyAssassin[GameManager.instance.fightingNodeNumber] = true;
                }
                if (enemy.UD.herotype == UnitData.HeroType.mage)
                {
                    FightAnimationUI.isThereEnemyMage[GameManager.instance.fightingNodeNumber] = true;
                }
            }
            // Phase 1
            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (ally.UD.herotype == UnitData.HeroType.assassin)
                {
                    allyAssassinAttack += ally.UD.attack;
                }
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
            {
                if (enemy.UD.herotype == UnitData.HeroType.assassin)
                {
                    enemyAssassinAttack += enemy.UD.attack;
                }
            }

            GameManager.instance.publicAllyAssassinAttack[GameManager.instance.fightingNodeNumber] = allyAssassinAttack;
            GameManager.instance.publicEnemyAssassinAttack[GameManager.instance.fightingNodeNumber] = enemyAssassinAttack;

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
            // Sort for Phase 2, 3
            result.unitList.Sort(comparer);
            // Phase 2
            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (ally.UD.herotype != UnitData.HeroType.mage && ally.UD.herotype != UnitData.HeroType.assassin && ally.CurrentHealth > 0)
                {
                    allyAttack += ally.UD.attack;
                }
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
            {
                if (enemy.UD.herotype != UnitData.HeroType.mage && enemy.UD.herotype != UnitData.HeroType.assassin && enemy.CurrentHealth > 0)
                {
                    enemyAttack += enemy.UD.attack;
                }
            }

            GameManager.instance.publicAllyAttack[GameManager.instance.fightingNodeNumber] = allyAttack;
            GameManager.instance.publicEnemyAttack[GameManager.instance.fightingNodeNumber] = enemyAttack;

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
            // Phase 3
            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (ally.UD.herotype == UnitData.HeroType.mage && ally.CurrentHealth > 0)
                {
                    allyMageAttack += ally.UD.attack;
                }
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
            {
                if (enemy.UD.herotype == UnitData.HeroType.mage && enemy.CurrentHealth > 0)
                {
                    enemyMageAttack += enemy.UD.attack;
                }
            }

            GameManager.instance.publicAllyMageAttack[GameManager.instance.fightingNodeNumber] = allyMageAttack;
            GameManager.instance.publicEnemyMageAttack[GameManager.instance.fightingNodeNumber] = enemyMageAttack;

            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                ally.GetDamaged(enemyMageAttack);
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
            {
                enemy.GetDamaged(allyMageAttack);
            }
        }
        return result;
    }
}
