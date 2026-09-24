using System;
using UnityEngine;
using UnityEngine.Events;

public class SelectChartButton : CustomButton
{
    [SerializeField]
    private SelectTreeButtonEvent _onClicked;
    [SerializeField]
    private int _chartId;

    protected override void OnClick()
    {
        _onClicked?.Invoke(_chartId);
    }
}

[Serializable]
public class SelectChartButtonEvent : UnityEvent<int>{}
