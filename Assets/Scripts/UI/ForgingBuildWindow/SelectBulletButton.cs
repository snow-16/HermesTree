using System;
using UnityEngine;
using UnityEngine.Events;

public class SelectBulletButton : CustomButton
{
    [SerializeField]
    private SelectBulletButtonEvent _onClicked;
    [SerializeField]
    private BulletType _bulletType;

    protected override void OnClick()
    {
        _onClicked?.Invoke(_bulletType);
    }
}

[Serializable]
public class SelectBulletButtonEvent : UnityEvent<BulletType>{}
