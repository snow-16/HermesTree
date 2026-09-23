using UnityEngine;
using MackySoft;

[CreateAssetMenu(fileName = "CellSettingData", menuName = "Scriptable Objects/CellSettingData")]
public class CellSettingData : DataBase
{
    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeReference, SubclassSelector]
    private IBulletProcessor _processor;
    public IBulletProcessor Processor => _processor;
}
