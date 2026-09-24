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

    [SerializeField]
    private float _swipeSpeed;
    public float SwipeSpeed => _swipeSpeed;

    [SerializeField]
    private float _swipeStickSensitivity;
    public float SwipeStickSensitivity => _swipeStickSensitivity;

    [SerializeField]
    private float _cursorStickSensitivity;
    public float CursorStickSensitivity => _cursorStickSensitivity;
}
