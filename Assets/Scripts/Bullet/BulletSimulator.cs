using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class BulletSimulator : MonoBehaviour
{
    private BulletData _bulletData = new();

    private BulletType _ownType;
    private BulletSettingData _bulletSettingData;

    private List<Collider2D> _hitEntities = new();
    private TriggerProcessor _triggerProcessor;

    private Rigidbody2D _rb2;
    private Collider2D _collider;

    public void Spawn(BulletType type, Vector2 position, Quaternion rotation, List<IBulletProcessor> processors)
    {
        _rb2 = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _ownType = type;
        _bulletSettingData = DataManager.ReadData<BulletDataBase>().BulletList[_ownType];
        _bulletData.ReFillProcessor(processors);
        _bulletData.AddBoundable(_bulletSettingData.BaseBoundable);

        transform.SetPositionAndRotation(position, rotation);

        DataManager.AddData(_bulletData, gameObject);
    }

    void Update()
    {
        RunningProcessor();

        _rb2.linearVelocity = transform.up * (_bulletSettingData.BaseSpeed + _bulletData.Acceleration);

        _hitEntities.RemoveAll(collider => collider == null);
    }

    private void RunningProcessor()
    {
        if(_bulletData.HasProcessor)
        {
            _bulletData.GetProcessors().ForEach(processor => 
            {
                if(processor is TriggerProcessor triggerProcessor)
                {
                    _triggerProcessor = triggerProcessor;
                }
                else
                {
                    _bulletData = processor.Processing(_bulletData);
                }
            });
        }
        else if(_triggerProcessor != null && _triggerProcessor.ProcessingTrigger())
        {
            _bulletData = _triggerProcessor.Processing(_bulletData);
            _triggerProcessor = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out IDamagable entity))
        {
            HitEntity(entity, collision);
        }
        else
        {
            HitTrrain(collision);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(_hitEntities.Contains(collision.collider))
        {
            _hitEntities.Remove(collision.collider);

            Observable
            .Timer(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ => Physics2D.IgnoreCollision(_collider, collision.collider, false))
            .AddTo(this).AddTo(collision.collider);
        }
    }

    private void HitEntity(IDamagable entity, Collision2D collision)
    {
        entity.Damage(_bulletSettingData.BaseDamage + _bulletData.Damage);

        if(_bulletData.PenetrableCount > 0)
        {
            _bulletData.Penetration();
            _hitEntities.Add(collision.collider);
            Physics2D.IgnoreCollision(_collider, collision.collider);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HitTrrain(Collision2D collision)
    {
        if(_bulletData.BoundableCount > 0)
        {
            _bulletData.Bound();
            
            var normal = collision.contacts[0].normal;
            var reflectVector = Vector2.Reflect(transform.up, normal);
            transform.rotation = Quaternion.FromToRotation(transform.up, (Vector2)transform.position + reflectVector);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
