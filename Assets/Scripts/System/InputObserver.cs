using UnityEngine;
using UnityEngine.InputSystem;
using R3;
using System;
using System.Collections.Generic;

/// <summary>
/// 各入力処理を管理するクラス
/// </summary>
public static class InputObserver
{
    private static Dictionary<string, Dictionary<InputType, ActionContainer>> _inputActions = new();
    public static Dictionary<string, Dictionary<InputType, ActionContainer>> InputActions { get => _inputActions; set => _inputActions = value; }

    private static InputSystem_Actions _inputMap;
    public static InputSystem_Actions InputMap => _inputMap;

    public static void CreateInputs()
    {
        if(_inputActions.Count == 0)
        {
            _inputMap = new InputSystem_Actions();

            foreach(var input in _inputMap)
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
                    });
                }
            }
        }
    }

    /// <summary>
    /// 入力を受け取るメソッドを追加する
    /// </summary>
    public static ListenerBuilder AddListener()
    {
        return new();
    }

    private static void BuildListener(ListenerBuilder builder)
    {
        _inputActions[builder._input.name][builder._inputType].action += builder._inputAction;
        _inputActions[builder._input.name][builder._inputType].SetOutputProcess(builder._output);

        Observable
        .EveryUpdate()
        .Where(_ => builder._listenObject == null)
        .Take(1)
        .Subscribe(_ => _inputActions[builder._input.name][builder._inputType].action -= builder._inputAction);
    }

    public static void SwitchPlayerEnabled(bool enabled)
    {
        SwitchEnabled(enabled, _inputMap.Player.Get());
    }

    private static void SwitchEnabled(bool enabled, InputActionMap map)
    {
        if(enabled)
        {
            map.Enable();
        }
        else
        {
            map.Disable();
        }
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

    public struct ListenerBuilder
    {
        public InputAction _input;
        public InputType _inputType;
        public Action<Vector2> _inputAction;
        public Func<InputAction, Vector2> _output;
        public GameObject _listenObject;

        public ListenerBuilder SetInput(InputAction input)
        {
            _input = input;
            return this;
        }

        public ListenerBuilder SetType(InputType inputType)
        {
            _inputType = inputType;
            return this;
        }

        public ListenerBuilder SetAction(Action<Vector2> inputAction)
        {
            _inputAction = inputAction;
            return this;
        }

        public ListenerBuilder SetOutputProcessing(Func<InputAction, Vector2> output)
        {
            _output = output;
            return this;
        }

        public ListenerBuilder SetListenerObject(GameObject listenObject)
        {
            _listenObject = listenObject;
            return this;
        }

        public readonly void Build()
        {
            BuildListener(this);
        }
    }
}
