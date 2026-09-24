using UnityEngine;

[CreateAssetMenu(fileName = "SkillTreeDataBase", menuName = "Scriptable Objects/SkillTreeDataBase")]
public class SkillTreeDataBase : DataBase
{
    [SerializeField]
    private int _layerCount;
    public int LayerCount => _layerCount;

    [SerializeField]
    private float _layerMargin;
    public float LayerMargin => _layerMargin;
}
