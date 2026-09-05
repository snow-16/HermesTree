using UnityEngine;

/// <summary>
/// プレイヤーの横移動を行うコンポーネント
/// </summary>
public class PlayerMover : MonoBehaviour
{
    private PlayerDataBase _playerDataBase;

    void Start()
    {
        var playerAction = new InputSystem_Actions().Player;

        var core = GetComponent<PlayerCore>();
        core.AddListener(playerAction.MoveRight, InputType.IsPressed, Move, action => new(action.ReadValue<float>(), 0), this);
        core.AddListener(playerAction.MoveLeft, InputType.IsPressed, Move, action => new(action.ReadValue<float>(), 0), this);
        
        _playerDataBase = DataManager.ReadData<PlayerDataBase>();
    }

    /// <summary>
    /// プレイヤーの横移動
    /// </summary>
    /// <param name="input">入力の値</param>
    public void Move(Vector2 input)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * input.x * _playerDataBase.Speed);
    }
}
