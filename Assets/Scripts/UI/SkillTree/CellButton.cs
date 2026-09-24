using System;
using UnityEngine;
using UnityEngine.Events;

public class CellButton : CustomButton
{
    [SerializeField]
    private CellButtonEvent _onClicked;
    [SerializeField]
    private GameObject _cellConnecterPrefab;

    private int _treeId;
    private Vector2 _cellPosition;
    private SkillTreeData.CellData _cellData;

    private SkillTreeDataBase _skillTreeDataBase;
    private SkillTreeData _skillTreeData;

    void Start()
    {
        _skillTreeDataBase = DataManager.ReadData<SkillTreeDataBase>();
        _skillTreeData = DataManager.ReadData<SkillTreeData>();

        ((RectTransform)transform).anchoredPosition = _cellPosition * _skillTreeDataBase.LayerMargin;

        _cellData.connectedCells.ForEach(cellPosition =>
        {
            var distance = cellPosition - _cellPosition;
            var joint = CreateConnecter(transform, new Vector2(distance.x, 0), (int)distance.x);
            CreateConnecter(joint, new Vector2(0, distance.y), (int)distance.y);
        });
    }

    void Update()
    {
        if(_skillTreeData.TreeDatas[_treeId].unlocked.Contains(_cellPosition))
        {
            DisablePress();
        }
        else
        {
            EnablePress();
        }
    }

    private Transform CreateConnecter(Transform joint, Vector2 direction, int length)
    {
        for(int i = 0; i < length; i++)
        {
            var connecter = Instantiate(_cellConnecterPrefab);
            var rect = (RectTransform)connecter.transform;
            rect.SetParent(joint);
            rect.anchoredPosition = Vector2.zero;
            var size = rect.sizeDelta;
            size.x = _skillTreeDataBase.LayerMargin - ((RectTransform)transform).sizeDelta.x / 2;
            rect.sizeDelta = size;

            if(i == 0)
            {
                rect.rotation = Quaternion.FromToRotation(Vector2.right, direction);
            }

            joint = rect;
        }

        return joint;
    }

    protected override void OnClick()
    {
        _onClicked?.Invoke(_cellPosition, _cellData);
    }

    public void Initialize(int treeId, Vector2 pos, SkillTreeData.CellData data)
    {
        _treeId = treeId;
        _cellPosition = pos;
        _cellData = data;
    }

    public void AddClickAction(Action<Vector2, SkillTreeData.CellData> action)
    {
        _onClicked.AddListener(new(action));
    }
}

[Serializable]
public class CellButtonEvent : UnityEvent<Vector2, SkillTreeData.CellData>{}
