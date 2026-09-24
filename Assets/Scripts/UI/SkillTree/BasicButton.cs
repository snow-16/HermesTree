using UnityEngine;
using UnityEngine.Events;

public class BasicButton : CustomButton
{
    [SerializeField]
    private UnityEvent _onClicked;

    protected override void OnClick()
    {
        _onClicked?.Invoke();
    }
}
