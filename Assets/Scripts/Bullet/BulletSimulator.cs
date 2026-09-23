using System.Collections.Generic;
using UnityEngine;

public class BulletSimulator : MonoBehaviour
{
    private BulletData _bulletData = new();

    private BulletType _ownType;
    private BulletSettingData _bulletSettingData;

    private TriggerProcessor _triggerProcessor;

    private Rigidbody2D _rb2;

    public void Spawn(BulletType type, Vector2 position, Quaternion rotation, List<IBulletProcessor> processors)
    {
        _rb2 = GetComponent<Rigidbody2D>();

        _ownType = type;
        _bulletSettingData = DataManager.ReadData<BulletDataBase>().BulletList[_ownType];
        _bulletData.ReFillProcessor(processors);

        transform.SetPositionAndRotation(position, rotation);

        DataManager.AddData(_bulletData, gameObject);
    }

    void Update()
    {
        RunningProcessor();

        _rb2.linearVelocity = transform.up * (_bulletSettingData.BaseSpeed + _bulletData.Acceleration);
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
        Destroy(gameObject);
    }
}
