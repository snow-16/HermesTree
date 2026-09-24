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
    private SkillTreeData.ChartData? _chartBuilder;

    private SkillTreeDataBase _skillTreeDataBase;
    private SkillTreeData _skillTreeData;

    public void OpenMenu(ForgingMenuBuilder.MenuBuilder builder)
    {
        _skillTreeDataBase = DataManager.ReadData<SkillTreeDataBase>();
        _skillTreeData = DataManager.ReadData<SkillTreeData>();

        if(builder.isTree)
        {
            _treeBuilder = _skillTreeData.TreeDatas[builder.treeId];
            _treeNameField.text = _treeBuilder.Value.name;
            OpenTree(_treeBuilder.Value);
        }
        else
        {
            _chartBuilder = _skillTreeData.ChartDatas[builder.chartId];
            _treeNameField.text = _chartBuilder.Value.name;
            OpenTree(_skillTreeData.TreeDatas[_chartBuilder.Value.perentTreeId]);
        }
    }

    private void OpenTree(SkillTreeData.TreeData tree)
    {
        tree.cells.ToList().ForEach(cell =>
        {
            BuildCell(tree.id, cell.Key, cell.Value, UnlockCell);
        });
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
            _treeBuilder = _treeBuilder?.SetName(_treeNameField.text);
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
