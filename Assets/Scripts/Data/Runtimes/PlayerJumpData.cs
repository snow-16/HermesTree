public class PlayerJumpData : IData
{
    private JumpState _jumpState = JumpState.Fall;
    public JumpState JumpState => _jumpState;

    public void Jump()
    {
        _jumpState = JumpState.Rise;
    }

    public void Fall()
    {
        _jumpState = JumpState.Fall;
    }

    public void Landing()
    {
        _jumpState = JumpState.OnGround;
    }
}
