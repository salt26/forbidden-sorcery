using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ExpectedFightResult
{
    public List<IUnitInterface> unitList = new List<IUnitInterface>();
    public ExpectedFightResult()
    {
        unitList = new List<IUnitInterface>();
    }
}
