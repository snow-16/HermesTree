using System;
using UnityEngine;
using UnityEngine.Events;

public class SelectTreeButton : CustomButton
{
    [SerializeField]
    private SelectTreeButtonEvent _onClicked;
    [SerializeField]
    private int _treeId;

    protected override void OnClick()
    {
        _onClicked?.Invoke(_treeId);
    }
}

[Serializable]
public class SelectTreeButtonEvent : UnityEvent<int>{}
