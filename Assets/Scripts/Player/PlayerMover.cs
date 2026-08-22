using UnityEngine;

/// <summary>
/// プレイヤーの横移動を行うコンポーネント
/// </summary>
public class PlayerMover : MonoBehaviour
{
    void Start()
    {
        var core = GetComponent<PlayerCore>();
        core.AddListener(PlayerKeyBindType.MoveRight, InputType.IsPressed, Move, action => new(action.ReadValue<float>(), 0), this);
        core.AddListener(PlayerKeyBindType.MoveLeft, InputType.IsPressed, Move, action => new(action.ReadValue<float>(), 0), this);
    }

    /// <summary>
    /// プレイヤーの横移動
    /// </summary>
    /// <param name="input">入力の値</param>
    public void Move(Vector2 input)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * input.x * 10);
    }
}
