using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ForgingMenuBuilder : MonoBehaviour
{
    [SerializeField]
    private OpenForgeMenuEvent _onOpenForge;
    [SerializeField]
    private GameObject _forgeMenu;
    [SerializeField]
    private GameObject _mainWindow;
    [SerializeField]
    private GameObject _buildTreeWindow;
    [SerializeField]
    private GameObject _buildChartWindow;
    [SerializeField]
    private GameObject _selectBulletWindow;
    [SerializeField]
    private GameObject _selectTreeWindow;
    [SerializeField]
    private GameObject _selectChartWindow;
    
    private MenuBuilder _builder;
    private List<GameObject> _windowTransition;

    private BulletDataBase _bulletDataBase;
    private SkillTreeData _skillTreeData;

    void Start()
    {
        _bulletDataBase = DataManager.ReadData<BulletDataBase>();
        _skillTreeData = DataManager.ReadData<SkillTreeData>();

        OpenBuilder();
    }

    public void OpenBuilder()
    {
        _builder = new();
        _windowTransition = new(){_mainWindow};

        _mainWindow.SetActive(true);
    }

    public void BuildTree()
    {
        _builder.isTree = true;

        SwitchWindow(_buildTreeWindow);
    }

    public void BuildChart()
    {
        _builder.isTree = false;

        SwitchWindow(_buildChartWindow);
    }

    public void NewTree()
    {
        _builder.createNew = true;
        var usedIds = _skillTreeData.TreeDatas.Select(tree => tree.Key).ToList();
        var newId = Enumerable.Range(0, _skillTreeData.TreeDatas.Count + 1).Except(usedIds).ToList().First();
        _builder.treeId = newId;

        SwitchWindow(_selectBulletWindow);
    }

    public void ReBuildTree()
    {
        _builder.createNew = false;

        SwitchWindow(_selectTreeWindow);
    }

    public void NewChart()
    {
        _builder.createNew = true;
        var usedIds = _skillTreeData.ChartDatas.Select(chart => chart.Key).ToList();
        var newId = Enumerable.Range(0, _skillTreeData.ChartDatas.Count + 1).Except(usedIds).ToList().First();
        _builder.chartId = newId;

        SwitchWindow(_selectTreeWindow);
    }

    public void ReBuildChart()
    {
        _builder.createNew = false;

        SwitchWindow(_selectChartWindow);
    }

    public void SelectBullet(BulletType type)
    {
        _builder.bulletType = type;
        OpenForge();

        _windowTransition.Last().SetActive(false);
    }

    public void SelectTree(int id)
    {
        _builder.treeId = id;
        OpenForge();

        _windowTransition.Last().SetActive(false);
    }

    public void SelectChart(int id)
    {
        _builder.chartId = id;
        OpenForge();

        _windowTransition.Last().SetActive(false);
    }

    public void OpenForge()
    {
        if(_builder.isTree)
        {
            Debug.Log($"ID{_builder.treeId}番の{_builder.bulletType}用ツリー構築メニューを開きます。");
        }
        else
        {
            Debug.Log($"ID{_builder.chartId}番の{_builder.treeId}番ツリー用チャート構築メニューを開きます。");
        }

        _forgeMenu.SetActive(true);
        _onOpenForge?.Invoke(_builder);
    }

    public void CloseForge()
    {
        _forgeMenu.SetActive(false);
        OpenBuilder();
    }

    public void BackWindow()
    {
        _windowTransition.Last().SetActive(false);
        _windowTransition.RemoveAt(_windowTransition.Count - 1);
        _windowTransition.Last().SetActive(true);
    }

    private void SwitchWindow(GameObject nextWindow)
    {
        _windowTransition.Last().SetActive(false);
        _windowTransition.Add(nextWindow);
        _windowTransition.Last().SetActive(true);
    }

    public struct MenuBuilder
    {
        public bool isTree;
        public bool createNew;
        public BulletType bulletType;
        public int treeId;
        public int chartId;
    }
}

[Serializable]
public class OpenForgeMenuEvent : UnityEvent<ForgingMenuBuilder.MenuBuilder>{}