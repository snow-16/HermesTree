using UnityEngine;

public class PlayerGunData : IData
{
    private bool _isAiming;
    public bool IsAiming => _isAiming;

    private Vector2 _targetPosition;
    public Vector2 TargetPosition => _targetPosition;
    public Vector2 WorldTargetPosition => Camera.main.ScreenToWorldPoint((Vector3)_targetPosition + new Vector3(0,0,-10));

    public void SetAiming(bool isAiming)
    {
        _isAiming = isAiming;
    }

    public void UpdatePosition(Vector2 pos)
    {
        _targetPosition = pos;
    }
}
