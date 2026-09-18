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

    private Rigidbody2D _rb2;

    void Start()
    {
        var core = GetComponent<PlayerCore>();
        core.AddListener(new InputSystem_Actions().Player.Jump, InputType.NowPressed, Jump, action => new(), this);

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
        }
    }

    /// <summary>
    /// プレイヤーのジャンプ
    /// </summary>
    /// <param name="input">入力の値</param>
    public void Jump(Vector2 input)
    {
        if(_playerJumpData.JumpState == JumpState.OnGround)
        {
            _rb2.linearVelocityY = _playerDataBase.JumpPower;
            _playerJumpData.Jump();
        }
    }

    public void Landing()
    {
        _playerJumpData.Landing();
    }
}
