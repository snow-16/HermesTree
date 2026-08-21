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
    private Dictionary<PlayerKeyBindType, Dictionary<InputType, ActionContainer>> _inputActions = new();
    public Dictionary<PlayerKeyBindType, Dictionary<InputType, ActionContainer>> InputActions { get => _inputActions; set => _inputActions = value; }

    void Awake()
    {
        var playerInputMap = InputSystem.actions.FindActionMap("Player");

        var actionContainerTemplate = new Dictionary<InputType, ActionContainer>
        {
            { InputType.IsPressed, new(action => action.IsPressed()) },
            { InputType.IsReleaced, new(action => !action.IsPressed()) },
            { InputType.NowPressed, new(action => action.WasPressedThisFrame()) },
            { InputType.NowReleaced, new(action => action.WasReleasedThisFrame()) }
        };
        
        foreach(PlayerKeyBindType actionType in Enum.GetValues(typeof(PlayerKeyBindType)))
        {
            _inputActions.Add(actionType, actionContainerTemplate);

            foreach(InputType inputType in Enum.GetValues(typeof(InputType)))
            {
                Observable.EveryUpdate().Where(_ => _inputActions[actionType][inputType].actionTrigger(playerInputMap[actionType.ToString()])).Subscribe(_ =>
                {
                    _inputActions[actionType][inputType].action?.Invoke();
                }).AddTo(this);
            }
        }

        _inputActions[PlayerKeyBindType.MoveRight][InputType.IsPressed].action += Move;
    }

    public void Move()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10);
    }

    public class ActionContainer
    {
        public Action action = null;
        public readonly Func<InputAction, bool> actionTrigger;

        public ActionContainer(Func<InputAction, bool> trigger)
        {
            actionTrigger = trigger;
        }
    }
}
