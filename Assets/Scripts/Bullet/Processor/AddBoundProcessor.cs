using System;
using UnityEngine;

[Serializable]
public class AddBoundProcessor : IBulletProcessor
{
    [SerializeField]
    private int _addend;

    public BulletData Processing(BulletData bulletData)
    {
        bulletData.AddBoundable(_addend);
        return bulletData;
    }
}
