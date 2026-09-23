using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CellDataBase", menuName = "Scriptable Objects/CellDataBase")]
public class CellDataBase : DataBase
{
    [SerializeField]
    private List<CellSettingData> _cellList = new();
    public Dictionary<CellType, CellSettingData> CellList => 
    _cellList
    .Select((item, index) => new{index, item})
    .ToDictionary(elem => (CellType)elem.index, elem => elem.item);

    public void SetListCount(CellType[] bulletTypes)
    {
        var difference = bulletTypes.Length - _cellList.Count;

        if(!Application.isPlaying)
        {
            if(difference > 0)
            {
                _cellList.AddRange(new CellSettingData[difference]);
            }
            else if(difference < 0)
            {
                _cellList.RemoveRange(_cellList.Count - difference * -1, difference * -1);
            }
        }
        else
        {
            Debug.LogError("このメソッドは実行中に実行できません。");
        }
    }
}
