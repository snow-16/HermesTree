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

    private SkillTreeData.TreeData _treeBuilder;
    private SkillTreeData.ChartData _chartBuilder;

    private SkillTreeDataBase _skillTreeDataBase;
    private SkillTreeData _skillTreeData;

    public void OpenMenu(ForgingMenuBuilder.MenuBuilder builder)
    {
        _skillTreeDataBase = DataManager.ReadData<SkillTreeDataBase>();
        _skillTreeData = DataManager.ReadData<SkillTreeData>();

        if(builder.isTree)
        {
            if(builder.createNew)
            {
                var baseTree = SkillTreeFiler.ReadTreeFromText(DataManager.ReadData<BulletDataBase>().BulletList[builder.bulletType].BaseTree.text);
                _treeBuilder = baseTree.SetId(builder.treeId).SetName("NewTree");
            }
            else
            {
                _treeBuilder = _skillTreeData.TreeDatas[builder.treeId].Clone();
            }

            _treeNameField.text = _treeBuilder.name;
            OpenTree(_treeBuilder, UnlockCell);
        }
        else
        {
            if(builder.createNew)
            {
                _chartBuilder = new SkillTreeData.ChartData(builder.chartId)
                .SetPerentTree(_skillTreeData.TreeDatas[builder.treeId])
                .SetName("NewChart");
            }
            else
            {
                _chartBuilder = _skillTreeData.ChartDatas[builder.chartId].Clone();
            }

            _treeNameField.text = _chartBuilder.name;
            OpenTree(_skillTreeData.TreeDatas[_chartBuilder.perentTreeId], SelectCell);
        }
    }

    private void OpenTree(SkillTreeData.TreeData tree, Action<SimplePosition, SkillTreeData.CellData> action)
    {
        tree.cells.ToList().ForEach(cell =>
        {
            BuildCell(tree, cell.Key, cell.Value, action);
        });
    }

    private void BuildCell(SkillTreeData.TreeData tree, SimplePosition cellPosition, SkillTreeData.CellData cellData, Action<SimplePosition, SkillTreeData.CellData> action)
    {
        var cellBuilder = Instantiate(_cellPrefab).GetComponent<CellButton>();
        cellBuilder.AddClickAction(action);
        cellBuilder.Initialize(_treeBuilder != null, tree, _chartBuilder, cellPosition, cellData, _connectersPerent);
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

    public void SelectCell(SimplePosition cellPosition, SkillTreeData.CellData cellData)
    {
        _chartBuilder.AddCell(cellPosition, cellData);
    }

    public void Save()
    {
        if(string.IsNullOrEmpty(_treeNameField.text))
        {
            Debug.LogError("名前を入力してください。");
        }

        if(_treeBuilder != null)
        {
            _treeBuilder = _treeBuilder?.SetName(_treeNameField.text);
            _skillTreeData.SetTree(_treeBuilder);
            SkillTreeFiler.WriteTree(_treeBuilder);
        }
        else
        {
            _chartBuilder = _chartBuilder?.SetName(_treeNameField.text);
            _skillTreeData.SetChart(_chartBuilder);
            SkillTreeFiler.WriteChart(_chartBuilder);
        }
    }

    public void EndBuild()
    {
        _treeBuilder = null;
        _chartBuilder = null;

        foreach(Transform child in _cellsPerent)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in _connectersPerent)
        {
            Destroy(child.gameObject);
        }
    }
}
