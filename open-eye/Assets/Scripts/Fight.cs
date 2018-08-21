using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Fight
{                                                                           //이 함수는 Fight 페이즈에 모든 노드에 대해서 한 번씩 실행된다.
    public static ExpectedFightResult Fighting(List<Unit> us)               //특정 노드의 유닛 목록을 argument로 받는다
    {
        ExpectedFightResult result = new ExpectedFightResult();         //result 안에 unitList 있음

        bool allyUnitIsHere = false, enemyUnitIsHere = false;
        int numberOfAllies = 0, numberOfEnemies = 0;

        foreach (Unit u in us)                                      //이 노드에 있는 모든 유닛들에 대해 수행함
        {
            ImaginaryUnit iu = new ImaginaryUnit();
            iu.isAlly = u.isAlly;
            if (u.isAlly) { allyUnitIsHere = true; ++numberOfAllies; }                  //아군 : 0번 ~ (numberOfAllies - 1)번
            if (!u.isAlly) { enemyUnitIsHere = true; ++numberOfEnemies; }               //적군 : (numberOfAllies)번 ~ (numberOfAllies + numberOfEnemies - 1)번
            iu.ID = u.ID;
            iu.Movement = u.Movement;
            iu.CurrentHealth = u.CurrentHealth;
            iu.UD = u.UD;
            result.unitList.Add((iu as IUnitInterface));
        }
        
        result.unitList.Sort();     //무슨 순서에 따라 정렬이지?
        
        if (result.unitList.Count > 0 && allyUnitIsHere && enemyUnitIsHere)                  //전투 개시 조건 : 노드에 유닛이 하나라도 있을 때
        {
            int allyPhysicalAttack = 0, allyMagicalAttack = 0, allyAssassiationAttack = 0;
            int enemyPhysicalAttack = 0, enemyMagicalAttack = 0, enemyAssassinationAttack = 0;

            foreach (ImaginaryUnit livingUnit in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.CurrentHealth > 0))
            {
                if (livingUnit.isAlly)
                {
                    if (livingUnit.UD.herotype == UnitData.HeroType.assassin) allyAssassiationAttack += livingUnit.UD.attack;
                    else if (livingUnit.UD.herotype == UnitData.HeroType.mage) allyMagicalAttack += livingUnit.UD.attack;
                    else allyPhysicalAttack += livingUnit.UD.attack;
                }
                else
                {
                    if (livingUnit.UD.herotype == UnitData.HeroType.assassin) enemyAssassinationAttack += livingUnit.UD.attack;
                    else if (livingUnit.UD.herotype == UnitData.HeroType.mage) enemyMagicalAttack += livingUnit.UD.attack;
                    else enemyPhysicalAttack += livingUnit.UD.attack;
                }
            }

            int allyUnitFrontOrder = 0;
            int allyUnitBackOrder = numberOfAllies - 1;
            int enemyUnitFrontOrder = numberOfAllies;
            int enemyUnitBackOrder = numberOfAllies + numberOfEnemies - 1;

            while(allyAssassiationAttack > 0 && enemyUnitBackOrder >= enemyUnitFrontOrder)          //아군 1페이즈
            {
                ImaginaryUnit leastAggroEnemy = new ImaginaryUnit();
                leastAggroEnemy = (ImaginaryUnit)result.unitList[enemyUnitBackOrder];             //때릴 적군 유닛 결정

                if (allyAssassiationAttack >= leastAggroEnemy.CurrentHealth) --enemyUnitBackOrder;    //때려서 죽으면 가장 뒤의 살아있는 적 유닛 번호 줄여야지
                allyAssassiationAttack -= leastAggroEnemy.CurrentHealth;
                leastAggroEnemy.GetDamaged(allyAssassiationAttack);
            }

            while (enemyAssassinationAttack > 0 && allyUnitBackOrder >= allyUnitFrontOrder)
            {
                ImaginaryUnit leastAggroAlly = new ImaginaryUnit();
                leastAggroAlly = (ImaginaryUnit)result.unitList[allyUnitBackOrder];             //때릴 아군 유닛 결정

                if (enemyAssassinationAttack >= leastAggroAlly.CurrentHealth) --allyUnitBackOrder;    //때려서 죽으면 가장 뒤의 살아있는 적 유닛 번호 줄여야지
                enemyAssassinationAttack -= leastAggroAlly.CurrentHealth;
                leastAggroAlly.GetDamaged(enemyAssassinationAttack);
            }
            
            while(allyPhysicalAttack > 0 && enemyUnitBackOrder >= enemyUnitFrontOrder)          //아군 2페이즈
            {
                ImaginaryUnit mostAggroEnemy = new ImaginaryUnit();
                mostAggroEnemy = (ImaginaryUnit)result.unitList[enemyUnitFrontOrder];

                if (allyPhysicalAttack >= mostAggroEnemy.CurrentHealth) ++enemyUnitFrontOrder;
                allyPhysicalAttack -= mostAggroEnemy.CurrentHealth;
                mostAggroEnemy.GetDamaged(allyPhysicalAttack);
            }

            while (enemyPhysicalAttack > 0 && allyUnitBackOrder >= allyUnitFrontOrder)          //적군 2페이즈
            {
                ImaginaryUnit mostAggroAlly = new ImaginaryUnit();
                mostAggroAlly = (ImaginaryUnit)result.unitList[allyUnitFrontOrder];

                if (enemyPhysicalAttack >= mostAggroAlly.CurrentHealth) ++allyUnitFrontOrder;
                enemyPhysicalAttack -= mostAggroAlly.CurrentHealth;
                mostAggroAlly.GetDamaged(enemyPhysicalAttack);
            }

            foreach(ImaginaryUnit enemy in result.unitList.FindAll((unit) => unit is ImaginaryUnit && !unit.isAlly && unit.CurrentHealth > 0))
            {
                enemy.GetDamaged(allyMagicalAttack);
            }

            foreach (ImaginaryUnit ally in result.unitList.FindAll((unit) => unit is ImaginaryUnit && unit.isAlly && unit.CurrentHealth > 0))
            {
                ally.GetDamaged(enemyMagicalAttack);
            }
        }
        return result;
    }
}