using System.Collections.Generic;

public abstract class SwitchProcessor : IBulletProcessor
{
    private List<IBulletProcessor> _connectedTrueProcessors;
    private List<IBulletProcessor> _connectedFalseProcessors;

    protected abstract bool ProcessingSwitch();

    public BulletData Processing(BulletData bulletData)
    {
        bulletData.ReFillProcessor(ProcessingSwitch() ? _connectedTrueProcessors : _connectedFalseProcessors);
        return bulletData;
    }
}
