public class PlayerMoveData : IData
{
    private MoveState _moveDirection;
    public MoveState MoveDirection => _moveDirection;

    private float _speed;
    public float Speed => _speed;

    public void ChangeDirection(MoveState dir)
    {
        _moveDirection = dir;
    }

    public void Invert()
    {
        _speed *= -1;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
