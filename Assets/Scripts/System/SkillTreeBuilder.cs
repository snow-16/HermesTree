using UnityEngine;

public class SkillTreeBuilder : MonoBehaviour
{
    private SkillTreeData.TreeData _treeBuilder = new(0);

    public void StartCreateTree(int id)
    {
        _treeBuilder = new(id);
        _treeBuilder.SetName("Test");
    }

    public void AddCell(int cellNumber, SkillTreeData.CellData cellData)
    {
        _treeBuilder.AddCell(cellNumber, cellData);
    }

    public void UnlockCell(int cellNumber, SkillTreeData.CellData cellData)
    {
        _treeBuilder.AddUnlocked(cellNumber);
    }

    public void SetTree()
    {
        DataManager.ReadData<SkillTreeData>().SetTree(_treeBuilder);
        SkillTreeFiler.WriteTree(_treeBuilder);
    }
}
