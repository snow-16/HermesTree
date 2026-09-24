using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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

    public void SetType(BulletType type)
    {
        _bulletType = type;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = type.ToString();
    }
}

[Serializable]
public class SelectBulletButtonEvent : UnityEvent<BulletType>{}
