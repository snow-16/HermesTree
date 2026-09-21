using UnityEngine;

public class PlayerGunData : IData
{
    private bool _isAiming;
    public bool IsAiming => _isAiming;

    private Vector2 _targetPosition;
    public Vector2 TargetPosition => _targetPosition;

    public void SetAiming(bool isAiming)
    {
        _isAiming = isAiming;
    }
}
