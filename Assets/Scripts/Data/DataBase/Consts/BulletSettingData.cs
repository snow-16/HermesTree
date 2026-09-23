using UnityEngine;

[CreateAssetMenu(fileName = "BulletSettingData", menuName = "Scriptable Objects/BulletSettingData")]
public class BulletSettingData : DataBase
{
    [SerializeField]
    private float _baseSpeed;
    public float BaseSpeed => _baseSpeed;

    [SerializeField]
    private float _baseDamage;
    public float BaseDamage => _baseDamage;

    [SerializeField]
    private int _baseBoundable;
    public int BaseBoundable => _baseBoundable;

    [SerializeField]
    private int _basePenetrable;
    public int BasePenetrable => _basePenetrable;
}
