using System.Collections.Generic;
using UnityEngine;

public class BulletData : InstanceData
{
    private List<IBulletProcessor> _remainingProcessors = new();
    public bool HasProcessor => _remainingProcessors.Count > 0;
    
    private float _acceleration;
    public float Acceleration => _acceleration;

    private float _damage;
    public float Damage => _damage;

    private int _boundableCount;
    public int BoundableCount => _boundableCount;

    /// <summary> 貫通可能数 </summary>
    private int _penetrableCount;
    /// <summary> 貫通可能数 </summary>
    public int PenetrableCount => _penetrableCount;

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

    public void AddDamage(float addend)
    {
        _damage += addend;
    }

    public void AddBoundable(int count)
    {
        _boundableCount += count;
    }

    public void Bound()
    {
        _boundableCount--;
    }

    public void AddPenetrable(int count)
    {
        _penetrableCount += count;
    }

    public void Penetration()
    {
        _penetrableCount--;
    }
}
