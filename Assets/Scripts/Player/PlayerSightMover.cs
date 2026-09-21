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

            _sight.transform.position = screenPlayerPos + clampedOffset;
        }
    }

    public void MoveSight(Vector2 input)
    {
        if(_systemData.CurrentDevice is Gamepad)
        {
            _sight.SetActive(true);

            var screenPlayerPos = Camera.main.WorldToScreenPoint(transform.position);
            _sight.transform.position = screenPlayerPos + (Vector3)input * _playerDataBase.SightRange;
        }
    }

    public void DisableSight(Vector2 input)
    {
        if(_systemData.CurrentDevice is Gamepad)
        {
            _sight.SetActive(false);
        }
    }
}
