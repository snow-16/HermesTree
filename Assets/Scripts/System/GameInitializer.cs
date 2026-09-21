using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private List<DataBase> _defaultDataBases;
    
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
        }
    }
}
