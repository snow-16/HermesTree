using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunner : MonoBehaviour
{
    [SerializeField]
    private BulletSimulator _bulletPrefab;

    private PlayerGunData _playerGunData;
    private PlayerMoveData _playerMoveData;
    private SystemData _systemData;

    void Awake()
    {
        var playerAction = InputObserver.InputMap.Player;

        var listenerBuilder = InputObserver.AddListener().SetOutputProcessing(action => new()).SetListenerObject(gameObject);
        var aimBuilder = listenerBuilder.SetInput(playerAction.Aim);
        aimBuilder.SetType(InputType.NowPressed).SetAction(EnableAim).Build();
        aimBuilder.SetType(InputType.NowReleaced).SetAction(DisableAim).Build();
        listenerBuilder.SetInput(playerAction.Shoot).SetType(InputType.NowPressed).SetAction(Shoot).Build();
        listenerBuilder.SetInput(playerAction.Teleport).SetType(InputType.NowPressed).SetAction(Teleport).Build();

        _playerGunData = DataManager.ReadData<PlayerGunData>();
        _playerMoveData = DataManager.ReadData<PlayerMoveData>();
        _systemData = DataManager.ReadData<SystemData>();
    }

    private void EnableAim(Vector2 input)
    {
        if(_systemData.CurrentDevice is Keyboard)
        {
            _playerGunData.SetAiming(true);
        }
    }

    private void DisableAim(Vector2 input)
    {
        if(_systemData.CurrentDevice is Keyboard)
        {
            _playerGunData.SetAiming(false);
        }
    }

    private void Shoot(Vector2 input)
    {
        if(_playerGunData.IsAiming)
        {
            var bulletRotation = Quaternion.FromToRotation(Vector2.up, _playerGunData.WorldTargetPosition - (Vector2)transform.position);
            Instantiate(_bulletPrefab).Spawn(BulletType.Normal, transform.position, bulletRotation, new(){new AccelerateProcessor(), new AccelerateProcessor()});
        }
    }

    private void Teleport(Vector2 input)
    {
        if(_systemData.CurrentDevice is Gamepad ? _playerGunData.IsAiming : !_playerGunData.IsAiming)
        {
            var targetPos = _playerGunData.WorldTargetPosition;
            _playerMoveData.SetTeleportTarget(targetPos);
        }
    }
}
