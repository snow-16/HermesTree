using System;
using UnityEngine;
using UnityEngine.Events;

public class SelectMagazineCaseIndexButton : CustomButton
{
    [SerializeField]
    private SelectMagazineCaseButtonEvent _onClicked;
    [SerializeField]
    private int index;

    protected override void OnClick()
    {
        _onClicked?.Invoke(index);
    }
}

[Serializable]
public class SelectMagazineCaseButtonEvent : UnityEvent<int>{}
