using System.Linq;
using UnityEngine;
using TMPro;
using System;

public class SkillTreeBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject _cellPrefab;
    [SerializeField]
    private Transform _cellsPerent;
    [SerializeField]
    private Transform _connectersPerent;
    [SerializeField]
    private TMP_InputField _treeNameField;

    private SkillTreeData.TreeData? _treeBuilder;

    private SkillTreeDataBase _skillTreeDataBase;
    private SkillTreeData _skillTreeData;

    void Start()
    {
        _skillTreeDataBase = DataManager.ReadData<SkillTreeDataBase>();
        _skillTreeData = DataManager.ReadData<SkillTreeData>();
    }

    public void CreateTree()
    {
        var usedIds = _skillTreeData.TreeDatas.Select(tree => tree.Key).ToList();
        var id = Enumerable.Range(0, _skillTreeData.TreeDatas.Count + 1).Except(usedIds).ToList().First();
        _treeBuilder = new SkillTreeData.TreeData(id);
        _skillTreeData.SetTree(_treeBuilder.Value);

        var sideLength = 1 + (_skillTreeDataBase.LayerCount - 1) * 2;
        var offset = _skillTreeDataBase.LayerCount - 1;
        for(int i = 0; i < sideLength * sideLength; i++)
        {
            BuildCell(id, new(i % sideLength - offset, i / sideLength - offset), new(CellType.SpeedUp), AddCell);
        }
    }

    public void OpenTree(int id)
    {
        _treeBuilder = _skillTreeData.TreeDatas[id];

        _treeBuilder?.cells.ToList().ForEach(cell =>
        {
            BuildCell(id, cell.Key, cell.Value, UnlockCell);
        });
    }

    private void BuildCell(int id, SimplePosition cellPosition, SkillTreeData.CellData cellData, Action<SimplePosition, SkillTreeData.CellData> action)
    {
        var cellBuilder = Instantiate(_cellPrefab).GetComponent<CellButton>();
        cellBuilder.AddClickAction(action);
        cellBuilder.Initialize(id, cellPosition, cellData, _connectersPerent);
        cellBuilder.transform.SetParent(_cellsPerent);
    }

    public void AddCell(SimplePosition cellPosition, SkillTreeData.CellData cellData)
    {
        _treeBuilder?.AddCell(cellPosition, cellData);
    }

    public void UnlockCell(SimplePosition cellPosition, SkillTreeData.CellData cellData)
    {
        _treeBuilder?.AddUnlocked(cellPosition);
    }

    public void SetTree()
    {
        if(_treeBuilder != null)
        {
            _treeBuilder?.SetName(_treeNameField.text);
            _skillTreeData.SetTree(_treeBuilder.Value);
            SkillTreeFiler.WriteTree(_treeBuilder.Value);
            _treeBuilder = null;
        }
        else
        {
            Debug.LogError("スキルツリーが開かれていません。");
        }
    }
}
