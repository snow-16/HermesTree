using System.Linq;

public class SelectTreeWindowBuilder : SelectWindowBuilder<SelectTreeButton>
{
    private SkillTreeData _skillTreeData;

    protected override void Build()
    {
        _skillTreeData = DataManager.ReadData<SkillTreeData>();

        _skillTreeData.TreeDatas.Select(tree => tree.Value).ToList().ForEach(tree =>
        {
            CreateButton().SetTree(tree);
        });
    }
}
