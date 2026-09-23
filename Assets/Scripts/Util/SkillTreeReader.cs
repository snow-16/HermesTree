using System.Collections.Generic;

public static class SkillTreeReader
{
    public static List<IBulletProcessor> ReadChart(SkillTreeData.ChartData chart)
    {
        var tree = DataManager.ReadData<SkillTreeData>().TreeDatas[chart.perentTreeId];
        var cellList = DataManager.ReadData<CellDataBase>().CellList;
        return GenerateProcessors(new(), chart.selected, tree, cellList);
    }

    private static List<IBulletProcessor> GenerateProcessors(
        List<IBulletProcessor> list, List<SkillTreeData.IBranchData> branchs,
        SkillTreeData.TreeData tree, Dictionary<CellType, CellSettingData> cellList)
    {
        branchs.ForEach(branch =>
        {
            if(branch is SkillTreeData.BranchCell cell)
            {
                list.Add(cellList[tree.cells[cell.cellNumber].cellType].Processor);
            }
        });

        return list;
    }
}
