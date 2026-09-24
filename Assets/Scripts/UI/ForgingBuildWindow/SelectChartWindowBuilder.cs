using System.Linq;

public class SelectChartWindowBuilder : SelectWindowBuilder<SelectChartButton>
{
    private SkillTreeData _skillTreeData;

    protected override void Build()
    {
        _skillTreeData = DataManager.ReadData<SkillTreeData>();

        _skillTreeData.ChartDatas.Select(chart => chart.Value).ToList().ForEach(chart =>
        {
            CreateButton().SetChart(chart);
        });
    }
}
