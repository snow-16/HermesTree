using System;
using TMPro;
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

    public void SetTree(SkillTreeData.TreeData data)
    {
        _treeId = data.id;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.name;
    }
}

[Serializable]
public class SelectTreeButtonEvent : UnityEvent<int>{}
