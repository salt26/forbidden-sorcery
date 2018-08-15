using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ImaginaryUnit : IUnitInterface
{
    public bool isAlly { get; set; }
    public UnitData UD { get; set; }
    public int Movement { get; set; }
    public int CurrentHealth { get; set; }
    public int ID { get; set; }

    public void Damage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }

    public int CompareTo(object obj)
    {
        IUnitInterface imaginaryUnit;
        if (!(obj is IUnitInterface))
            return 1;
        else
        {
            imaginaryUnit = obj as IUnitInterface;
            return this.UD.aggro - imaginaryUnit.UD.aggro;
        }
    }
}