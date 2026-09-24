using System;
using TMPro;
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

    public void SetChart(SkillTreeData.ChartData data)
    {
        _chartId = data.id;
        GetComponent<TextMeshProUGUI>().text = data.name;
    }
}

[Serializable]
public class SelectChartButtonEvent : UnityEvent<int>{}
