using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CellButton : CustomButton
{
    [SerializeField]
    private CellButtonEvent _onStarted;
    [SerializeField]
    private CellButtonEvent _onClicked;
    [SerializeField]
    private int _cellNumber;
    [SerializeField]
    private CellType _cellType;
    [SerializeField]
    private List<int> _connectedCells = new();

    void Start()
    {
        _onStarted?.Invoke(_cellNumber, new(_cellType, _connectedCells));
    }

    protected override void OnClick()
    {
        _onClicked?.Invoke(_cellNumber, new(_cellType, _connectedCells));
    }
}

[Serializable]
public class CellButtonEvent : UnityEvent<int, SkillTreeData.CellData>{}
