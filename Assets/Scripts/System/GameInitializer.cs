using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private List<DataBase> _defaultDataBases;

    private SystemData _systemData = new();
    
    private static bool _isInitialized;
    
    void Awake()
    {
        if(!_isInitialized)
        {
            _isInitialized = true;

            _defaultDataBases.ForEach(dataBase =>
            {
                DataManager.AddData(dataBase);
            });
            DataManager.AddData(_systemData);

            _systemData.CheckingGamepad();
            InputSystem.onDeviceChange += _systemData.OnDeviceChanged;
        }
    }
}
