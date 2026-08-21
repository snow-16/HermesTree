using UnityEngine;
using UnityEngine.InputSystem;
using R3;
using System;
using System.Collections.Generic;

/// <summary>
/// プレイヤーのプレイヤーの各処理を管理する中核コンポーネント
/// </summary>
public class PlayerCore : MonoBehaviour
{
    private Dictionary<PlayerInputType, Action> _inputActions = new();
    public Dictionary<PlayerInputType, Action> InputActions { get => _inputActions; set => _inputActions = value; }

    void Start()
    {
        var playerInputMap = InputSystem.actions.FindActionMap("Player");
        
        foreach(PlayerInputType actionType in Enum.GetValues(typeof(PlayerInputType)))
        {
            _inputActions.Add(actionType, null);

            Observable.EveryUpdate().Where(_ => playerInputMap[actionType.ToString()].IsPressed()).Subscribe(_ =>
            {
                _inputActions[actionType]?.Invoke();
            }).AddTo(this);
        }

        _inputActions[PlayerInputType.MoveRight] += Move;
    }

    public void Move()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10);
    }
}
