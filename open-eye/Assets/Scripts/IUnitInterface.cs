using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IUnitInterface : IComparable
{
    UnitData UD { get; set; }
    int ID { get; set; }
    int Movement { get; set; }
    int CurrentHealth { get; set; }
    bool isAlly { get; set; }
}