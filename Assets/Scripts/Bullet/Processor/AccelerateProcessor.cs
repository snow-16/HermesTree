using System;

[Serializable]
public class AccelerateProcessor : IBulletProcessor
{
    public BulletData Processing(BulletData bulletData)
    {
        bulletData.Accelarate(10);
        return bulletData;
    }
}
