using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataBase", menuName = "Scriptable Objects/BulletDataBase")]
public class BulletDataBase : DataBase
{
    [SerializeField]
    private List<BulletSettingData> _bulletList = new();
    public Dictionary<BulletType, BulletSettingData> BulletList => 
    _bulletList
    .Select((item, index) => new{index, item})
    .ToDictionary(elem => (BulletType)elem.index, elem => elem.item);

    public void SetListCount(BulletType[] bulletTypes)
    {
        var difference = bulletTypes.Length - _bulletList.Count;

        if(!Application.isPlaying)
        {
            if(difference > 0)
            {
                _bulletList.AddRange(new BulletSettingData[difference]);
            }
            else if(difference < 0)
            {
                _bulletList.RemoveRange(_bulletList.Count - difference * -1, difference * -1);
            }
        }
        else
        {
            Debug.LogError("このメソッドは実行中に実行できません。");
        }
    }
}
