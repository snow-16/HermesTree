using System.Collections.Generic;

public abstract class TriggerProcessor : IBulletProcessor
{
    private List<IBulletProcessor> _connectedProcessors;

    public abstract bool ProcessingTrigger();

    public BulletData Processing(BulletData bulletData)
    {
        bulletData.ReFillProcessor(_connectedProcessors);
        return bulletData;
    }
}
