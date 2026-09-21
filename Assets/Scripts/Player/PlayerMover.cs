using UnityEngine;

/// <summary>
/// プレイヤーの横移動を行うコンポーネント
/// </summary>
public class PlayerMover : MonoBehaviour
{
    private PlayerDataBase _playerDataBase;
    private PlayerMoveData _playerMoveData;
    private PlayerJumpData _playerJumpData;

    private MoveState _preInput = MoveState.None;
    private MoveState _currentInput;
    private MoveState _wallHitDirection;

    private Rigidbody2D _rb2;

    void Start()
    {
        var playerAction = InputObserver.InputMap.Player;

        var listenerBuilder = InputObserver.AddListener().SetType(InputType.IsPressed).SetAction(InputMove).SetListenerObject(gameObject);
        var keyBoardBuilder = listenerBuilder.SetOutputProcessing(action => new(action.ReadValue<float>(), 0));
        keyBoardBuilder.SetInput(playerAction.MoveRight).Build();
        keyBoardBuilder.SetInput(playerAction.MoveLeft).Build();
        listenerBuilder.SetOutputProcessing(action => action.ReadValue<Vector2>()).SetInput(playerAction.Move).Build();

        InputObserver.SwitchPlayerEnabled(true);
        
        _playerDataBase = DataManager.ReadData<PlayerDataBase>();
        _playerMoveData = DataManager.ReadData<PlayerMoveData>();
        _playerJumpData = DataManager.ReadData<PlayerJumpData>();

        _rb2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _playerMoveData.UpdatePosition(transform.position);
    }

    void LateUpdate()
    {
        if(_currentInput != MoveState.None)
        {
            CalcVelocity();
        }
        else if(_rb2.linearVelocityX != 0)
        {
            var damping = _playerJumpData.JumpState == JumpState.OnGround ? _playerDataBase.FrictionDamping : _playerDataBase.AirDamping;
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

        if(_wallHitDirection != MoveState.Both && _wallHitDirection != _playerMoveData.MoveDirection)
        {
            if(_playerJumpData.JumpState == JumpState.OnGround)
            {
                _rb2.linearVelocityX = (int)_playerMoveData.MoveDirection * _playerDataBase.MaxSpeed;
            }
            else
            {
                _rb2.linearVelocityX += (int)_playerMoveData.MoveDirection / (1 + _playerDataBase.ControlAirResistance);
                _rb2.linearVelocityX = Mathf.Sign(_rb2.linearVelocityX) * Mathf.Min(Mathf.Abs(_rb2.linearVelocityX), _playerDataBase.MaxSpeed / (1 + _playerDataBase.BasicAirResistance));
            }
        }
    }

    /// <summary>
    /// プレイヤーの横移動入力
    /// </summary>
    /// <param name="input">入力の値</param>
    public void InputMove(Vector2 input)
    {
        var inputDirection = (MoveState)Mathf.Sign(input.x);

        if(_currentInput == MoveState.None)
        {
            _currentInput = inputDirection;
        }
        else if(_currentInput != inputDirection)
        {
            _currentInput = MoveState.Both;
        }
    }

    public void HitWallRight()
    {
        HitWall(MoveState.Right);
    }

    public void ExitWallRight()
    {
        ExitWall(MoveState.Right);
    }

    public void HitWallLeft()
    {
        HitWall(MoveState.Left);
    }

    public void ExitWallLeft()
    {
        ExitWall(MoveState.Left);
    }

    public void HitWall(MoveState direction)
    {
        _wallHitDirection = _wallHitDirection == MoveState.None || _wallHitDirection == direction ? direction : MoveState.Both;
    }

    public void ExitWall(MoveState direction)
    {
        _wallHitDirection = _wallHitDirection == MoveState.None || _wallHitDirection == direction ? MoveState.None : (MoveState)((int)direction * -1);
    }
}
