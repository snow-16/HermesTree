using System;
using UnityEngine;

[Serializable]
public class DamageProcessor : IBulletProcessor
{
    [SerializeField]
    private float _addend;

    public BulletData Processing(BulletData bulletData)
    {
        bulletData.AddDamage(_addend);
        return bulletData;
    }
}
