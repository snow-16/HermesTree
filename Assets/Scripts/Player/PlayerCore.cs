using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーのプレイヤーの各処理を管理する中核コンポーネント
/// </summary>
public class PlayerCore : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Keyboard.current.dKey.isPressed)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10);
        }
    }
}
