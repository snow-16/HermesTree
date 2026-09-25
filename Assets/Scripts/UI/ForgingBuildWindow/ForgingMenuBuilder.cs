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
    private OpenForgeMenuEvent _onOpenMagazine;
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
    [SerializeField]
    private GameObject _fillMagazineWindow;
    
    private MenuBuilder _builder;
    private List<GameObject> _windowTransition;

    private BulletDataBase _bulletDataBase;
    private SkillTreeData _skillTreeData;
    private PlayerGunData _playerGunData;

    void Start()
    {
        _bulletDataBase = DataManager.ReadData<BulletDataBase>();
        _skillTreeData = DataManager.ReadData<SkillTreeData>();
        _playerGunData = DataManager.ReadData<PlayerGunData>();

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
        _builder.menuType = ForgeType.Tree;

        SwitchWindow(_buildTreeWindow);
    }

    public void BuildChart()
    {
        _builder.menuType = ForgeType.Chart;

        SwitchWindow(_buildChartWindow);
    }

    public void BuildMagazine()
    {
        _builder.menuType = ForgeType.Magazine;
        _builder.magazine = new SkillTreeData.ChartData[_playerGunData.MagazineBases[0].bullets.Count()];
        _builder.magazineCaseIndex = 0;

        SwitchWindow(_fillMagazineWindow);
        _onOpenMagazine?.Invoke(_builder);
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
        if(_builder.menuType == ForgeType.Chart)
        {
            _builder.chartId = id;
            OpenForge();

            _windowTransition.Last().SetActive(false);
        }
        else if(_builder.menuType == ForgeType.Magazine)
        {
            var chart = _skillTreeData.ChartDatas[id];
            _builder.magazine[_builder.magazineCaseIndex] = chart;
            Debug.Log($"弾倉{_builder.magazineCaseIndex}番に{chart.name}を装填しました。");
        }
    }

    public void SelectInMagazineIndex(int index)
    {
        if(index < _builder.magazine.Length)
        {
            _builder.magazineCaseIndex = index;
        }
        else
        {
            Debug.LogError($"装弾番号{index}はマガジンサイズを超過しています。");
        }
    }

    public void SaveMagazine()
    {
        if(!_builder.magazine.Contains(null))
        {
            _playerGunData.SetMagazine(0, _builder.magazine);
            Debug.Log($"装弾数{_builder.magazine.Length}のマガジンを保存しました。");
        }
        else
        {
            Debug.LogError($"装弾数が足りません。");
        }
    }

    public void OpenForge()
    {
        if(_builder.menuType == ForgeType.Tree)
        {
            Debug.Log($"ID{_builder.treeId}番の{_builder.bulletType}用ツリー構築メニューを開きます。");
        }
        else if(_builder.menuType == ForgeType.Chart)
        {
            Debug.Log($"ID{_builder.chartId}番の{_builder.treeId}番ツリー用チャート構築メニューを開きます。");
        }
        else
        {
            Debug.LogError("マガジン装填メニューでツリーを展開することはできません。");
            return;
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
        public ForgeType menuType;
        public bool createNew;
        public BulletType bulletType;
        public int treeId;
        public int chartId;
        public int magazineCaseIndex;
        public SkillTreeData.ChartData[] magazine;
    }
}

[Serializable]
public class OpenForgeMenuEvent : UnityEvent<ForgingMenuBuilder.MenuBuilder>{}