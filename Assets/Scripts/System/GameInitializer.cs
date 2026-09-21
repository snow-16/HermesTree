using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private List<DataBase> _defaultDataBases;

    private SystemData _systemData;
    
    private static bool _isInitialized;
    
    void Awake()
    {
        if(!_isInitialized)
        {
            _isInitialized = true;

            ResetData();
            _systemData = DataManager.ReadData<SystemData>();

            InputObserver.CreateInputs();
            _systemData.CheckingGamepad();
            InputSystem.onDeviceChange += _systemData.OnDeviceChanged;
        }
    }

    private void ResetData()
    {
        _defaultDataBases.ForEach(dataBase =>
        {
            DataManager.AddData(dataBase);
        });
        
        var runtimeDatas = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => typeof(IData).IsAssignableFrom(type) && !typeof(DataBase).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
        .ToList();

        runtimeDatas.ForEach(data =>
        {
            DataManager.AddData((IData)Activator.CreateInstance(data));
        });
    }
}
