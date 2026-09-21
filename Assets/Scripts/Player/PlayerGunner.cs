using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunner : MonoBehaviour
{
    private PlayerDataBase _playerDataBase;
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

        _playerDataBase = DataManager.ReadData<PlayerDataBase>();
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
            
        }
    }

    private void Teleport(Vector2 input)
    {
        if(_systemData.CurrentDevice is Gamepad ? _playerGunData.IsAiming : !_playerGunData.IsAiming)
        {
            var targetPos = Camera.main.ScreenToWorldPoint((Vector3)_playerGunData.TargetPosition + new Vector3(0,0,-10));
            _playerMoveData.SetTeleportTarget(targetPos);
        }
    }
}
