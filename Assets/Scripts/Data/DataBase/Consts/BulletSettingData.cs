using UnityEngine;

[CreateAssetMenu(fileName = "BulletSettingData", menuName = "Scriptable Objects/BulletSettingData")]
public class BulletSettingData : DataBase
{
    [SerializeField]
    private float _baseSpeed;
    public float BaseSpeed => _baseSpeed;
}
