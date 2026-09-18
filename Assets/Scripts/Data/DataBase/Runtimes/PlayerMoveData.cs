public class PlayerMoveData : IData
{
    private MoveState _moveDirection;
    public MoveState MoveDirection => _moveDirection;

    public void ChangeDirection(MoveState dir)
    {
        _moveDirection = dir;
    }

    public void InvertDirection()
    {
        _moveDirection = (MoveState)((int)_moveDirection * -1);
    }
}
