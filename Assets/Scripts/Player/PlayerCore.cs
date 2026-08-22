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
        
        foreach(PlayerKeyBindType actionType in Enum.GetValues(typeof(PlayerKeyBindType)))
        {
            var actionContainerTemplate = new Dictionary<InputType, ActionContainer>
            {
                { InputType.IsPressed, new(action => action.IsPressed()) },
                { InputType.IsReleaced, new(action => !action.IsPressed()) },
                { InputType.NowPressed, new(action => action.WasPressedThisFrame()) },
                { InputType.NowReleaced, new(action => action.WasReleasedThisFrame()) }
            };
            _inputActions.Add(actionType, actionContainerTemplate);

            foreach(InputType inputType in Enum.GetValues(typeof(InputType)))
            {
                var actionContainer = _inputActions[actionType][inputType];
                var action = playerInputMap[actionType.ToString()];
                Observable.EveryUpdate().Where(_ => actionContainer.action != null).Where(_ => actionContainer.actionTrigger(action)).Subscribe(_ =>
                {
                    actionContainer.action.Invoke(actionContainer.actionOutput(action));
                }).AddTo(this);
            }
        }
    }

    /// <summary>
    /// 入力を受け取るメソッドを追加する
    /// </summary>
    /// <param name="keyBindType">キーバインドの種類</param>
    /// <param name="inputType">入力の種類</param>
    /// <param name="inputAction">購読メソッド</param>
    /// <param name="listenObject">メソッドの持ち主</param>
    public void AddListener(PlayerKeyBindType keyBindType, InputType inputType, Action<Vector2> inputAction, Func<InputAction, Vector2> output, MonoBehaviour listenObject)
    {
        _inputActions[keyBindType][inputType].action += inputAction;
        _inputActions[keyBindType][inputType].SetOutputProcess(output);
        Observable.EveryUpdate().Where(_ => listenObject == null).Take(1).Subscribe(_ => _inputActions[keyBindType][inputType].action -= inputAction).AddTo(this);
    }

    /// <summary>
    /// 入力の種類ごとに処理を保持するクラス
    /// </summary>
    public class ActionContainer
    {
        public Action<Vector2> action = null;
        public readonly Func<InputAction, bool> actionTrigger;
        public Func<InputAction, Vector2> actionOutput;

        public ActionContainer(Func<InputAction, bool> trigger)
        {
            actionTrigger = trigger;
        }

        public void SetOutputProcess(Func<InputAction, Vector2> output)
        {
            actionOutput = output;
        }
    }
}
