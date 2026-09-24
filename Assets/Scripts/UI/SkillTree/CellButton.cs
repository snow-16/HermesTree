using System;
using UnityEngine;
using UnityEngine.Events;

public class CellButton : CustomButton
{
    [SerializeField]
    private CellButtonEvent _onClicked;
    [SerializeField]
    private GameObject _cellConnecterPrefab;

    private bool _isTreeBuilding;
    private SkillTreeData.TreeData _treeData;
    private SkillTreeData.ChartData _chartData;
    private Vector2 _cellPosition;
    private SkillTreeData.CellData _cellData;
    private Transform _connectersPerent;

    private SkillTreeDataBase _skillTreeDataBase;
    private SkillTreeData _skillTreeData;

    void Start()
    {
        _skillTreeDataBase = DataManager.ReadData<SkillTreeDataBase>();
        _skillTreeData = DataManager.ReadData<SkillTreeData>();

        ((RectTransform)transform).anchoredPosition = _cellPosition * _skillTreeDataBase.LayerMargin;

        _cellData.connectedCells.ForEach(cellPosition =>
        {
            var distance = cellPosition.ConvertVector() - _cellPosition;
            var joint = CreateConnecter(((RectTransform)transform).anchoredPosition, new Vector2(distance.x, 0), (int)distance.x);
            CreateConnecter(joint, new Vector2(0, distance.y), (int)distance.y);
        });
    }

    void Update()
    {
        if(_isTreeBuilding)
        {
            UpdateTreeBuild();
        }
        else
        {
            UpdateChartBuild();
        }
    }

    private void UpdateTreeBuild()
    {
        var isUnlocked = _treeData.IsCellUnlocked(new(_cellPosition));
        var isUnlockable = _treeData.IsCellUnlocked(_cellData.connectFrom);

        if(isUnlocked || !isUnlockable)
        {
            DisablePress();
        }
        else
        {
            EnablePress();
        }

        if(isUnlocked)
        {
            ChangeColorSet(1);
        }
        else if(isUnlockable)
        {
            ChangeColorSet(3);
        }
        else
        {
            ChangeColorSet(2);
        }
    }

    private void UpdateChartBuild()
    {
        var isSelected = _chartData.allSelecteds.Contains(new(_cellPosition));
        var isUnlocked = _treeData.IsCellUnlocked(new(_cellPosition));
        var isSelectable = _chartData.CanSelecting(new(_cellPosition));

        if(!isSelectable)
        {
            DisablePress();
        }
        else
        {
            EnablePress();
        }

        if(isSelected)
        {
            ChangeColorSet(1);
        }
        else if(isSelectable)
        {
            ChangeColorSet(3);
        }
        else if(isUnlocked)
        {
            ChangeColorSet(4);
        }
        else
        {
            ChangeColorSet(2);
        }
    }

    private Vector2 CreateConnecter(Vector2 startPoint, Vector2 direction, int length)
    {
        var connecter = Instantiate(_cellConnecterPrefab);
        connecter.transform.SetParent(_connectersPerent);
        var rect = (RectTransform)connecter.transform;
        rect.anchoredPosition = startPoint;
        var size = rect.sizeDelta;
        size.x = Mathf.Abs(_skillTreeDataBase.LayerMargin * length);
        rect.sizeDelta = size;
        rect.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        return startPoint + direction.normalized * rect.sizeDelta.x;
    }

    protected override void OnClick()
    {
        _onClicked?.Invoke(new(_cellPosition), _cellData);
    }

    public void Initialize(bool isTreeBuilding, SkillTreeData.TreeData treeData, SkillTreeData.ChartData chartData, SimplePosition pos, SkillTreeData.CellData data, Transform connectersPerent)
    {
        _isTreeBuilding = isTreeBuilding;
        _treeData = treeData;
        _chartData = chartData;
        _cellPosition = pos.ConvertVector();
        _cellData = data;
        _connectersPerent = connectersPerent;
    }

    public void AddClickAction(Action<SimplePosition, SkillTreeData.CellData> action)
    {
        _onClicked.AddListener(new(action));
    }
}

[Serializable]
public class CellButtonEvent : UnityEvent<SimplePosition, SkillTreeData.CellData>{}
