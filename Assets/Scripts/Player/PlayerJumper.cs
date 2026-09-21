using System;
using R3;
using UnityEngine;

/// <summary>
/// プレイヤーのジャンプ処理を行うコンポーネント
/// </summary>
public class PlayerJumper : MonoBehaviour
{
    [SerializeField]
    private LinearHitDetection _underDetection;

    private PlayerDataBase _playerDataBase;
    private PlayerJumpData _playerJumpData = new();

    private bool _canJumping;
    private float _totalPersevere;
    private bool _isPersevere;

    private Rigidbody2D _rb2;

    void Start()
    {
        var listenerBuilder = InputObserver.AddListener().SetInput(new InputSystem_Actions().Player.Jump).SetOutputProcessing(action => new()).SetListenerObject(gameObject);
        listenerBuilder.SetType(InputType.NowPressed).SetAction(Jump).Build();
        listenerBuilder.SetType(InputType.IsPressed).SetAction(Persevere).Build();
        listenerBuilder.SetType(InputType.NowReleaced).SetAction(Relax).Build();

        _playerDataBase = DataManager.ReadData<PlayerDataBase>();
        DataManager.AddData(_playerJumpData);

        _rb2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(_rb2.linearVelocityY > 0)
        {
            _underDetection.enabled = false;
        }
        else
        {
            _underDetection.enabled = true;
            _playerJumpData.Fall();

            if(_rb2.gravityScale == _playerDataBase.GravityOnRise)
            {
                _rb2.gravityScale = _playerDataBase.GravityOnHover;
            }
            else if(_rb2.gravityScale == _playerDataBase.GravityOnHover && _rb2.linearVelocityY < _playerDataBase.HorveringBorder)
            {
                _rb2.gravityScale = _playerDataBase.GravityOnFall;
            }
        }
    }

    /// <summary>
    /// プレイヤーのジャンプ
    /// </summary>
    /// <param name="input">入力の値</param>
    public void Jump(Vector2 input)
    {
        if(_canJumping)
        {
            _rb2.linearVelocityY = _playerDataBase.JumpPower;
            _rb2.gravityScale = _playerDataBase.GravityOnRise;
            _totalPersevere = 0;
            _isPersevere = true;
            _playerJumpData.Jump();
            _canJumping = false;
        }
    }

    public void Persevere(Vector2 input)
    {
        if(_isPersevere && _totalPersevere < _playerDataBase.MaxPersevere)
        {
            _totalPersevere += 1;
            _rb2.linearVelocityY = Mathf.Min(_rb2.linearVelocityY + _playerDataBase.PerseverePower, _playerDataBase.MaxJumpRise);
        }
        else if(_rb2.gravityScale == _playerDataBase.GravityOnRise)
        {
            _rb2.gravityScale = _playerDataBase.GravityOnHover;
        }
    }

    public void Relax(Vector2 input)
    {
        _isPersevere = false;

        if(_rb2.gravityScale == _playerDataBase.GravityOnRise)
        {
            _rb2.gravityScale = _playerDataBase.GravityOnHover;
        }
    }

    public void Landing()
    {
        if(_playerJumpData.JumpState == JumpState.Fall)
        {
            _playerJumpData.Landing();
            _canJumping = true;
        }
    }

    public void Slipping()
    {
        if(_playerJumpData.JumpState == JumpState.OnGround)
        {
            _playerJumpData.Fall();

            Observable
            .Timer(TimeSpan.FromSeconds(_playerDataBase.CoyoteTime))
            .TakeUntil(Observable.EveryUpdate().Where(_ => !_canJumping))
            .Subscribe(_ => _canJumping = false);
        }
    }
}
