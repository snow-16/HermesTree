using System.Linq;
using UnityEngine;

public class SkillTreeBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject _cellPrefab;

    private SkillTreeData.TreeData? _treeBuilder;

    private SkillTreeData _skillTreeData;

    void Start()
    {
        _skillTreeData = DataManager.ReadData<SkillTreeData>();
    }

    public void OpenTree(int id)
    {
        _treeBuilder = _skillTreeData.TreeDatas[id];

        _treeBuilder?.cells.ToList().ForEach(cell =>
        {
            var cellBuilder = Instantiate(_cellPrefab).GetComponent<CellButton>();
            cellBuilder.AddClickAction(UnlockCell);
            cellBuilder.Initialize(cell.Key, cell.Value);
            cellBuilder.transform.SetParent(transform);
        });
    }

    public void StartCreateTree(int id)
    {
        _treeBuilder = new(id);
        _treeBuilder?.SetName("Test");
    }

    public void AddCell(Vector2 cellPosition, SkillTreeData.CellData cellData)
    {
        _treeBuilder?.AddCell(cellPosition, cellData);
    }

    public void UnlockCell(Vector2 cellPosition, SkillTreeData.CellData cellData)
    {
        _treeBuilder?.AddUnlocked(cellPosition);
    }

    public void SetTree()
    {
        if(_treeBuilder != null)
        {
            DataManager.ReadData<SkillTreeData>().SetTree(_treeBuilder.Value);
            SkillTreeFiler.WriteTree(_treeBuilder.Value);
            _treeBuilder = null;
        }
        else
        {
            Debug.LogError("スキルツリーが開かれていません。");
        }
    }
}
