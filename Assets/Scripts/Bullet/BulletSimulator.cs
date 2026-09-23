using UnityEngine;

public class BulletSimulator : MonoBehaviour
{
    private BulletData _bulletData = new();

    private BulletType _ownType;
    private BulletSettingData _bulletSettingData;

    private Rigidbody2D _rb2;

    public void Spawn(BulletType type, Vector2 position, Quaternion rotation)
    {
        _rb2 = GetComponent<Rigidbody2D>();

        _ownType = type;
        _bulletSettingData = DataManager.ReadData<BulletDataBase>().BulletList[_ownType];

        transform.position = position;
        transform.rotation = rotation;
        _rb2.AddForce(transform.up * _bulletSettingData.BaseSpeed, ForceMode2D.Impulse);

        DataManager.AddData(_bulletData, gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
