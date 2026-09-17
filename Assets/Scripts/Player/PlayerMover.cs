using UnityEngine;

/// <summary>
/// プレイヤーの横移動を行うコンポーネント
/// </summary>
public class PlayerMover : MonoBehaviour
{
    private PlayerDataBase _playerDataBase;
    private PlayerMoveData _playerMoveData = new();

    private MoveState _currentDirection;

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
        if(_currentDirection != MoveState.None)
        {
            CalcVelocity();
        }
        else if(_rb2.linearVelocityX != 0)
        {
            var playerJumpData = DataManager.ReadData<PlayerJumpData>();

            _rb2.linearVelocityX = Mathf.MoveTowards(_rb2.linearVelocityX, 0, playerJumpData.JumpState == JumpState.OnGround ? 1 : 0.03f);
        }

        _currentDirection = MoveState.None;
    }

    private void CalcVelocity()
    {
        var playerJumpData = DataManager.ReadData<PlayerJumpData>();

        if(playerJumpData.JumpState == JumpState.OnGround)
        {
            _rb2.linearVelocityX = (int)_currentDirection * _playerDataBase.Speed;
        }
        else
        {
            _rb2.linearVelocityX += (int)_currentDirection * 0.1f;
            _rb2.linearVelocityX = Mathf.Sign(_rb2.linearVelocityX) * Mathf.Min(Mathf.Abs(_rb2.linearVelocityX), _playerDataBase.Speed * 0.8f);
        }
    }

    /// <summary>
    /// プレイヤーの横移動入力
    /// </summary>
    /// <param name="input">入力の値</param>
    public void InputMove(Vector2 input)
    {
        var inputDirection = (MoveState)input.x;

        if(_currentDirection != inputDirection)
        {
            _currentDirection = inputDirection;
        }
    }
}
