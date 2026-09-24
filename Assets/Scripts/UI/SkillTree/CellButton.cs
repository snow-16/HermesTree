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

    void Start()
    {
        ((RectTransform)transform).anchoredPosition = _cellPosition * 100;

        _connectedCells.ForEach(cellNumber =>
        {
            
        });
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
