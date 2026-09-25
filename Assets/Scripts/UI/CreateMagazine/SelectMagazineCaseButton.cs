using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SelectMagazineCaseIndexButton : CustomButton
{
    [SerializeField]
    private SelectMagazineCaseButtonEvent _onClicked;
    [SerializeField]
    private int index;

    private TextMeshProUGUI _bulletText;

    private SkillTreeData.ChartData[] _magazine;

    void Start()
    {
        _bulletText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        var bullet = _magazine[index];
        _bulletText.text = bullet != null ? bullet.name : "NoBullet";
    }

    public void SetMagazine(SkillTreeData.ChartData[] magazine)
    {
        _magazine = magazine;
    }

    protected override void OnClick()
    {
        _onClicked?.Invoke(index);
    }
}

[Serializable]
public class SelectMagazineCaseButtonEvent : UnityEvent<int>{}
