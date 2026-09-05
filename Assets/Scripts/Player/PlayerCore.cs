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
    private Dictionary<string, Dictionary<InputType, ActionContainer>> _inputActions = new();
    public Dictionary<string, Dictionary<InputType, ActionContainer>> InputActions { get => _inputActions; set => _inputActions = value; }

    private InputSystem_Actions _inputMap;

    void OnEnable()
    {
        _inputMap.Enable();
    }

    void OnDisable()
    {
        _inputMap.Disable();
    }

    void Awake()
    {
        _inputMap = new InputSystem_Actions();
        var playerInputMap = _inputMap.Player;

        foreach(var input in playerInputMap.Get())
        {
            var actionContainerTemplate = new Dictionary<InputType, ActionContainer>
            {
                { InputType.IsPressed, new(action => action.IsPressed()) },
                { InputType.IsReleaced, new(action => !action.IsPressed()) },
                { InputType.NowPressed, new(action => action.WasPressedThisFrame()) },
                { InputType.NowReleaced, new(action => action.WasReleasedThisFrame()) }
            };
            
            _inputActions.Add(input.name, actionContainerTemplate);

            foreach(InputType inputType in Enum.GetValues(typeof(InputType)))
            {
                var actionContainer = _inputActions[input.name][inputType];
                Observable.EveryUpdate().Where(_ => actionContainer.action != null).Where(_ => actionContainer.actionTrigger(input)).Subscribe(_ =>
                {
                    actionContainer.action.Invoke(actionContainer.actionOutput(input));
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
    public void AddListener(InputAction input, InputType inputType, Action<Vector2> inputAction, Func<InputAction, Vector2> output, MonoBehaviour listenObject)
    {
        _inputActions[input.name][inputType].action += inputAction;
        _inputActions[input.name][inputType].SetOutputProcess(output);
        Observable.EveryUpdate().Where(_ => listenObject == null).Take(1).Subscribe(_ => _inputActions[input.name][inputType].action -= inputAction).AddTo(this);
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
