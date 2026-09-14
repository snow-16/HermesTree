using UnityEngine;

/// <summary>
/// プレイヤーのジャンプ処理を行うコンポーネント
/// </summary>
public class PlayerJumper : MonoBehaviour
{
    private PlayerJumpData _playerJumpData = new();

    void Start()
    {
        var core = GetComponent<PlayerCore>();
        core.AddListener(new InputSystem_Actions().Player.Jump, InputType.NowPressed, Jump, action => new(), this);

        DataManager.AddData(_playerJumpData);
    }

    /// <summary>
    /// プレイヤーのジャンプ
    /// </summary>
    /// <param name="input">入力の値</param>
    public void Jump(Vector2 input)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        _playerJumpData.Jump();
    }
}
