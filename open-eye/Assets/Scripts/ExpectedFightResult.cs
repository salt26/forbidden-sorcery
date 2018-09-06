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

    public static ExpectedFightResult ExpectFightResultAfterSomeTurn(ExpectedFightResult inReport, int n)
    {
        ExpectedFightResult report = inReport;
        for(int i = 0; i < n; i++)
        {
            report = Fight.Fighting(report.unitList);
        }
        return report;
    }
}
