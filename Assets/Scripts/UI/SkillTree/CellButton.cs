using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CellButton : CustomButton
{
    [SerializeField]
    private CellButtonEvent _onClicked;
    [SerializeField]
    private Vector2 _cellPosition;
    [SerializeField]
    private CellType _cellType;
    [SerializeField]
    private List<Vector2> _connectedCells = new();
    [SerializeField]
    private GameObject _cellConnecterPrefab;

    private SkillTreeDataBase _skillTreeDataBase;

    void Start()
    {
        _skillTreeDataBase = DataManager.ReadData<SkillTreeDataBase>();
        ((RectTransform)transform).anchoredPosition = _cellPosition * _skillTreeDataBase.LayerMargin;

        _connectedCells.ForEach(cellPosition =>
        {
            var distance = cellPosition - _cellPosition;
            var joint = CreateConnecter(transform, new Vector2(distance.x, 0), (int)distance.x);
            CreateConnecter(joint, new Vector2(0, distance.y), (int)distance.y);
        });
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
        _onClicked?.Invoke(_cellPosition, new(_cellType, _connectedCells));
    }

    public void Initialize(Vector2 pos, SkillTreeData.CellData data)
    {
        _cellPosition = pos;
        _cellType = data.cellType;
        _connectedCells = data.connectedCells;
    }

    public void AddClickAction(Action<Vector2, SkillTreeData.CellData> action)
    {
        _onClicked.AddListener(new(action));
    }
}

[Serializable]
public class CellButtonEvent : UnityEvent<Vector2, SkillTreeData.CellData>{}
