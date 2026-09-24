using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CustomButton : MonoBehaviour, 
IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private List<ColorSet> _colorSets = new();
    [SerializeField]
    protected float _pressedScaleMultiplier;

    private int _colorSelected;
    private Vector2 _baseScale;
    protected bool _isFocused;
    protected bool _isPressed;
    private bool _canPressing = true;
    public bool CanPressing => _canPressing;

    protected Image _buttonImage;

    void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _baseScale = ((RectTransform)transform).sizeDelta;
    }

    void OnDisable()
    {
        _isFocused = false;
        _isPressed = false;
        DrawButton();
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
            ((RectTransform)transform).sizeDelta = _baseScale;
            _buttonImage.color = _colorSets[_colorSelected].focusedColor;
        }
        else if(_isPressed)
        {
            var rect = (RectTransform)transform;
            var offset = (rect.sizeDelta.x < rect.sizeDelta.y ? rect.sizeDelta.x : rect.sizeDelta.y) * (1 - _pressedScaleMultiplier);
            rect.sizeDelta = _baseScale - new Vector2(offset, offset);
            _buttonImage.color = _colorSets[_colorSelected].pressedColor;
        }
        else
        {
            ((RectTransform)transform).sizeDelta = _baseScale;
            _buttonImage.color = _isFocused ? _colorSets[_colorSelected].focusedColor : _colorSets[_colorSelected].baseColor;
        }
    }

    protected void ChangeColorSet(int index)
    {
        _colorSelected = index;
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

    [Serializable]
    public struct ColorSet
    {
        public Color baseColor;
        public Color focusedColor;
        public Color pressedColor;
    }
}
