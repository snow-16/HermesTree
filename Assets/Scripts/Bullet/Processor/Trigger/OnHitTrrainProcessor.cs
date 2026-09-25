using System;

[Serializable]
public class OnHitTerrainProcessor : TriggerProcessor
{
    public override bool ProcessingTrigger()
    {
        return true;
    }
}
