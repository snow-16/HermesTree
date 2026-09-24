using System;

[Serializable]
public class NullProcessor : IBulletProcessor
{
    public BulletData Processing(BulletData bulletData)
    {
        return bulletData;
    }
}
