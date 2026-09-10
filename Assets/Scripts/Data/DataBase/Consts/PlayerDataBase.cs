using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "Scriptable Objects/PlayerDataBase")]
public class PlayerDataBase : DataBase
{
    [SerializeField]
    private float _speed;
    public float Speed => _speed;
}
