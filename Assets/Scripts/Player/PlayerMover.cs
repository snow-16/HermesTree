using UnityEngine;

/// <summary>
/// プレイヤーの横移動を行うコンポーネント
/// </summary>
public class PlayerMover : MonoBehaviour
{
    private PlayerDataBase _playerDataBase;
    private PlayerMoveData _playerMoveData = new();

    private MoveState _preInput = MoveState.None;
    private MoveState _currentInput;

    private Rigidbody2D _rb2;

    void Start()
    {
        var playerAction = new InputSystem_Actions().Player;

        var core = GetComponent<PlayerCore>();
        core.AddListener(playerAction.MoveRight, InputType.IsPressed, InputMove, action => new(action.ReadValue<float>(), 0), this);
        core.AddListener(playerAction.MoveLeft, InputType.IsPressed, InputMove, action => new(action.ReadValue<float>(), 0), this);
        
        _playerDataBase = DataManager.ReadData<PlayerDataBase>();
        DataManager.AddData(_playerMoveData);

        _rb2 = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if(_currentInput != MoveState.None)
        {
            CalcVelocity();
        }
        else if(_rb2.linearVelocityX != 0)
        {
            var playerJumpData = DataManager.ReadData<PlayerJumpData>();
            var damping = playerJumpData.JumpState == JumpState.OnGround ? _playerDataBase.FrictionDamping : _playerDataBase.AirDamping;
            _rb2.linearVelocityX = Mathf.MoveTowards(_rb2.linearVelocityX, 0, 1 / (1 + damping));
        }

        _preInput = _currentInput;
        _currentInput = MoveState.None;
    }

    private void CalcVelocity()
    {
        if(_preInput != _currentInput)
        {
            if(_preInput == MoveState.Both || _preInput == MoveState.None)
            {
                _playerMoveData.ChangeDirection(_currentInput);
            }
            else
            {
                _playerMoveData.InvertDirection();
            }
        }

        var playerJumpData = DataManager.ReadData<PlayerJumpData>();

        if(playerJumpData.JumpState == JumpState.OnGround)
        {
            _rb2.linearVelocityX = (int)_playerMoveData.MoveDirection * _playerDataBase.MaxSpeed;
        }
        else
        {
            _rb2.linearVelocityX += (int)_playerMoveData.MoveDirection / (1 + _playerDataBase.ControlAirResistance);
            _rb2.linearVelocityX = Mathf.Sign(_rb2.linearVelocityX) * Mathf.Min(Mathf.Abs(_rb2.linearVelocityX), _playerDataBase.MaxSpeed / (1 + _playerDataBase.BasicAirResistance));
        }
    }

    /// <summary>
    /// プレイヤーの横移動入力
    /// </summary>
    /// <param name="input">入力の値</param>
    public void InputMove(Vector2 input)
    {
        var inputDirection = (MoveState)input.x;

        if(_currentInput == MoveState.None)
        {
            _currentInput = inputDirection;
        }
        else if(_currentInput != inputDirection)
        {
            _currentInput = MoveState.Both;
        }
    }
}
