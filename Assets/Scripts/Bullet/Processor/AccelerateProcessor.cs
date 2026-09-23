using System;
using UnityEngine;

[Serializable]
public class AccelerateProcessor : IBulletProcessor
{
    [SerializeField]
    private float _addend;

    public BulletData Processing(BulletData bulletData)
    {
        bulletData.Accelarate(_addend);
        return bulletData;
    }
}
