using System.Collections.Generic;
using UnityEngine;

public class BulletData : InstanceData
{
    private List<IBulletProcessor> _remainingProcessors = new();
    public bool HasProcessor => _remainingProcessors.Count > 0;
    
    private float _acceleration;
    public float Acceleration => _acceleration;

    public void ReFillProcessor(List<IBulletProcessor> processors)
    {
        if(!HasProcessor)
        {
            _remainingProcessors = processors;
        }
        else
        {
            Debug.LogError("銃弾の強化処理が未完了です。処理を全て消化してから処理を追加してください。");
        }
    }

    public List<IBulletProcessor> GetProcessors()
    {
        var output = new List<IBulletProcessor>(_remainingProcessors);
        _remainingProcessors.Clear();
        return output;
    }

    public void Accelarate(float addend)
    {
        _acceleration += addend;
    }
}
