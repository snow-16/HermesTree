using System;
using UnityEngine;

[Serializable]
public class AddPenetrationProcessor : IBulletProcessor
{
    [SerializeField]
    private int _addend;

    public BulletData Processing(BulletData bulletData)
    {
        bulletData.AddPenetrable(_addend);
        return bulletData;
    }
}
