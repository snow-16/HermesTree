using UnityEngine;
using UnityEngine.InputSystem;

public class SystemData : IData
{
    private InputDevice _currentDevice;
    public InputDevice CurrentDevice => _currentDevice;

    public void CheckingGamepad()
    {
        foreach(var device in InputSystem.devices)
        {
            if(device is Gamepad)
            {
                Debug.Log("コントローラー接続済み");
                _currentDevice = device;
                return;
            }
        }

        Debug.Log("コントローラー未接続");
        _currentDevice = new Keyboard();
    }

    public void OnDeviceChanged(InputDevice device, InputDeviceChange deviceChange)
    {
        if(device is Gamepad)
        {
            switch(deviceChange)
            {
                case InputDeviceChange.Added:
                {
                    Debug.Log("コントローラー接続");
                    _currentDevice = device;
                    break;
                }
                case InputDeviceChange.Removed:
                {
                    Debug.Log("コントローラー切断");
                    _currentDevice = new Keyboard();
                    break;
                }
            }
        }
    }
}
