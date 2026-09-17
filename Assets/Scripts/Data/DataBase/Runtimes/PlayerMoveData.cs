public class PlayerMoveData : IData
{
    private float _speed;
    public float Speed => _speed;

    public void Invert()
    {
        _speed *= -1;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
