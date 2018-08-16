﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Fight
{
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
        result.unitList.Sort();
        if (result.unitList.Count > 0)
        {
            int allyAttack = 0;
            int enemyAttack = 0;
            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                allyAttack += ally.UD.attack;
            }

            foreach (ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly))
            {
                enemyAttack += enemy.UD.attack;
                if (allyAttack < enemy.CurrentHealth)
                {
                    enemy.Damage(allyAttack);
                    allyAttack = 0;
                }
                else
                {
                    allyAttack -= enemy.CurrentHealth;
                    enemy.CurrentHealth = 0;
                }
            }

            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly))
            {
                if (enemyAttack < ally.CurrentHealth)
                {
                    ally.Damage(enemyAttack);
                    enemyAttack = 0;
                }
                else
                {
                    enemyAttack -= ally.CurrentHealth;
                    ally.CurrentHealth = 0;
                }
            }
        }
        return result;
    }
}