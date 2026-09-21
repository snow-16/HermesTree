using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSightMover : MonoBehaviour
{
    [SerializeField]
    private GameObject _sight;

    private PlayerDataBase _playerDataBase;
    private SystemData _systemData;
    private PlayerMoveData _playerMoveData;

    void Awake()
    {
        var listenerBuilder = InputObserver.AddListener().SetInput(new InputSystem_Actions().Player.Sight).SetOutputProcessing(action => action.ReadValue<Vector2>()).SetListenerObject(gameObject);
        listenerBuilder.SetType(InputType.IsPressed).SetAction(MoveSight).Build();
        listenerBuilder.SetType(InputType.NowReleaced).SetAction(DisableSight).Build();

        _playerDataBase = DataManager.ReadData<PlayerDataBase>();
        _systemData = DataManager.ReadData<SystemData>();
        _playerMoveData = DataManager.ReadData<PlayerMoveData>();
    }

    void LateUpdate()
    {
        Cursor.visible = false;

        if(_systemData.CurrentDevice is Keyboard)
        {
            var mousePos = Mouse.current.position.ReadValue();
            var screenPlayerPos = Camera.main.WorldToScreenPoint(transform.position);
            var clampedOffset = Vector3.ClampMagnitude(mousePos - (Vector2)screenPlayerPos, _playerDataBase.SightRange);
            ChangeSightOffset(clampedOffset);
        }
    }

    public void MoveSight(Vector2 input)
    {
        if(_systemData.CurrentDevice is Gamepad)
        {
            _sight.SetActive(true);

            ChangeSightOffset(input * _playerDataBase.SightRange);
        }
    }

    public void DisableSight(Vector2 input)
    {
        if(_systemData.CurrentDevice is Gamepad)
        {
            _sight.SetActive(false);
        }
    }

    private void ChangeSightOffset(Vector3 offset)
    {
        var screenPlayerPos = Camera.main.WorldToScreenPoint(transform.position);
        var targetPos = screenPlayerPos + offset;
        var inScreenPos = new Vector3(Mathf.Clamp(targetPos.x, 0, Screen.width), Mathf.Clamp(targetPos.y, 0, Screen.height), targetPos.z);
        _sight.transform.position = inScreenPos;
    }
}
