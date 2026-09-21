using UnityEngine;

public class PlayerMoveData : IData
{
    private MoveState _moveDirection;
    public MoveState MoveDirection => _moveDirection;

    private Vector2 _position;
    public Vector2 Position => _position;

    public void ChangeDirection(MoveState dir)
    {
        _moveDirection = dir;
    }

    public void InvertDirection()
    {
        _moveDirection = (MoveState)((int)_moveDirection * -1);
    }

    public void UpdatePosition(Vector2 pos)
    {
        _position = pos;
    }
}
