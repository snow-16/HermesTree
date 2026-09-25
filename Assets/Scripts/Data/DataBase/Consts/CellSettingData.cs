using UnityEngine;

[CreateAssetMenu(fileName = "CellSettingData", menuName = "Scriptable Objects/CellSettingData")]
public class CellSettingData : DataBase
{
    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeField]
    private Sprite _image;
    public Sprite Image => _image;

    [SerializeReference, SubclassSelector]
    private IBulletProcessor _processor;
    public IBulletProcessor Processor => _processor;

    [SerializeField]
    private string _text;
    public string Text => new(_text);
}
