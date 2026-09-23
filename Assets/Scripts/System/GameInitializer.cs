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

            var a = new SkillTreeData.ChartData().SetName("Test");
            var c = new SkillTreeData.BranchCell();
            a = a.AddCell(c);
            var s = new SkillTreeData.BranchSwitch().AddCell(new SkillTreeData.BranchCell(), true).AddCell(new SkillTreeData.BranchCell(), false);
            a = a.AddCell(s);
            SkillTreeFiler.WriteChart(a);
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
