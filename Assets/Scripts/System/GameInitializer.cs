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

            var testTree = new SkillTreeData.TreeData(0)
            .SetName("Test")
            .AddCell(0, DataManager.ReadData<CellDataBase>().CellList[CellType.SpeedUp])
            .AddUnlocked(0);
            DataManager.ReadData<SkillTreeData>().SetTree(testTree);

            var testChart = new SkillTreeData.ChartData(0)
            .SetName("Test")
            .SetPerentTree(testTree)
            .AddCell(new SkillTreeData.BranchCell(0));
            DataManager.ReadData<SkillTreeData>().SetChart(testChart);

            DataManager.ReadData<PlayerGunData>().SetMagazine(0, testChart, testChart, testChart);
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
        .Where(type => typeof(IData).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
        .Where(type => !typeof(DataBase).IsAssignableFrom(type) && !typeof(InstanceData).IsAssignableFrom(type))
        .ToList();

        runtimeDatas.ForEach(data =>
        {
            DataManager.AddData((IData)Activator.CreateInstance(data));
        });
    }
}
