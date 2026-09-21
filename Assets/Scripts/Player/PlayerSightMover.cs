using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSightMover : MonoBehaviour
{
    [SerializeField]
    private Image _sight;

    private PlayerDataBase _playerDataBase;
    private PlayerGunData _playerGunData;
    private SystemData _systemData;

    void Awake()
    {
        var listenerBuilder = InputObserver.AddListener().SetInput(InputObserver.InputMap.Player.Sight).SetOutputProcessing(action => action.ReadValue<Vector2>()).SetListenerObject(gameObject);
        listenerBuilder.SetType(InputType.IsPressed).SetAction(MoveSight).Build();
        listenerBuilder.SetType(InputType.IsReleaced).SetAction(DisableSight).Build();

        _playerDataBase = DataManager.ReadData<PlayerDataBase>();
        _playerGunData = DataManager.ReadData<PlayerGunData>();
        _systemData = DataManager.ReadData<SystemData>();
    }

    void LateUpdate()
    {
        Cursor.visible = false;

        var connectedGamepad = _systemData.CurrentDevice is Gamepad;

        var color = _sight.color;
        color.a = connectedGamepad || _playerGunData.IsAiming ? 1f : 0.5f;
        _sight.color = color;

        if(!connectedGamepad)
        {
            var mousePos = Mouse.current.position.ReadValue();
            var screenPlayerPos = Camera.main.WorldToScreenPoint(transform.position);
            var clampedOffset = Vector3.ClampMagnitude(mousePos - (Vector2)screenPlayerPos, _playerDataBase.SightRange);
            ChangeSightOffset(clampedOffset);
        }
    }

    private void MoveSight(Vector2 input)
    {
        if(_systemData.CurrentDevice is Gamepad)
        {
            _sight.gameObject.SetActive(true);
            _playerGunData.SetAiming(true);

            ChangeSightOffset(input * _playerDataBase.SightRange);
        }
    }

    private void DisableSight(Vector2 input)
    {
        if(_systemData.CurrentDevice is Gamepad)
        {
            _sight.gameObject.SetActive(false);
            _playerGunData.SetAiming(false);
        }
    }

    private void ChangeSightOffset(Vector3 offset)
    {
        var screenPlayerPos = Camera.main.WorldToScreenPoint(transform.position);
        var targetPos = screenPlayerPos + offset;
        var inScreenPos = new Vector3(Mathf.Clamp(targetPos.x, 0, Screen.width), Mathf.Clamp(targetPos.y, 0, Screen.height), targetPos.z);
        _sight.transform.position = inScreenPos;
        _playerGunData.UpdatePosition(inScreenPos);
    }
}
