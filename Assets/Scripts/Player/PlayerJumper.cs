using UnityEngine;

/// <summary>
/// プレイヤーのジャンプ処理を行うコンポーネント
/// </summary>
public class PlayerJumper : MonoBehaviour
{
    void Start()
    {
        var core = GetComponent<PlayerCore>();
        core.AddListener(PlayerKeyBindType.Jump, InputType.NowPressed, Jump, action => new(), this);
    }

    /// <summary>
    /// プレイヤーのジャンプ
    /// </summary>
    /// <param name="input">入力の値</param>
    public void Jump(Vector2 input)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }
}
