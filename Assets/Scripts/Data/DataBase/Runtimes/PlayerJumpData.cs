public class PlayerJumpData : IData
{
    private JumpState _jumpState;
    public JumpState JumpState => _jumpState;

    public void Jump()
    {
        _jumpState = JumpState.Rise;
    }
}
