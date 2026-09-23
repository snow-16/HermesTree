using System.Collections.Generic;

public static class SkillTreeReader
{
    public static List<IBulletProcessor> ReadChart(SkillTreeData.ChartData chart)
    {
        var tree = DataManager.ReadData<SkillTreeData>().TreeDatas[chart.perentTreeId];
        return GenerateProcessors(new(), chart.selected, tree);
    }

    private static List<IBulletProcessor> GenerateProcessors(List<IBulletProcessor> list, List<SkillTreeData.IBranchData> branchs, SkillTreeData.TreeData tree)
    {
        branchs.ForEach(branch =>
        {
            if(branch is SkillTreeData.BranchCell cell)
            {
                list.Add(tree.cells[cell.cellNumber].Processor);
            }
        });

        return list;
    }
}
