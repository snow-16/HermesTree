using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CustomButton : MonoBehaviour, 
IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Color _focusedColorOffset = Color.white;
    [SerializeField]
    protected Color _pressedColorOffset = Color.white;
    [SerializeField]
    protected Vector2 _pressedScaleOffset;

    private Color _baseColor;
    private Vector2 _baseScale;
    protected bool _isFocused;
    protected bool _isPressed;
    private bool _canPressing = true;
    public bool CanPressing => _canPressing;

    protected Image _buttonImage;

    void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _baseColor = _buttonImage.color;
        _baseScale = ((RectTransform)transform).sizeDelta;
    }

    protected abstract void OnClick();

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_canPressing)
        {
            OnClick();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        DrawButton();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        DrawButton();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isFocused = true;
        DrawButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isFocused = false;
        _isPressed = false;
        DrawButton();
    }

    private void DrawButton()
    {
        if(!_canPressing)
        {
            ((RectTransform)transform).sizeDelta = _isPressed ? _baseScale - _pressedScaleOffset : _baseScale;
            _buttonImage.color = _isPressed ? _pressedColorOffset : _focusedColorOffset;
        }
        else if(_isPressed)
        {
            _buttonImage.color = _pressedColorOffset;
            ((RectTransform)transform).sizeDelta = _baseScale - _pressedScaleOffset;
        }
        else
        {
            ((RectTransform)transform).sizeDelta = _baseScale;
            _buttonImage.color = _isFocused ? _focusedColorOffset : _baseColor;
        }
    }

    public void EnablePress()
    {
        _canPressing = true;
        DrawButton();
    }

    public void DisablePress()
    {
        _canPressing = false;
        DrawButton();
    }
}
