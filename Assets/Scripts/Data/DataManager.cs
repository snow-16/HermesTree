using System.Collections.Generic;
using R3;
using UnityEngine;

/// <summary>
/// 各種データベースを管理するクラス
/// </summary>
public static class DataManager
{
    private static Dictionary<string, IData> _dataBases = new();
    /// <summary> データベースを保持するディクショナリー </summary>
    public static Dictionary<string, IData> DataBases => _dataBases;

    /// <summary>
    /// データを追加する
    /// </summary>
    /// <typeparam name="T">データの型</typeparam>
    /// <param name="data">データの値</param>
    public static void AddData<T>(T data, GameObject connectedObject = null) where T : IData
    {
        var dataType = data.GetType();
        var key = dataType.ToString();

        if(connectedObject)
        {
            key += $"_{connectedObject.GetEntityId()}";
            var objectName = connectedObject.name;
            
            Observable
            .EveryUpdate()
            .Where(_ => connectedObject == null)
            .Take(1)
            .Subscribe(_ => 
            {
                _dataBases.Remove(key);
                Debug.Log($"破棄された{objectName}の{dataType}を除去しました。");
            });
        }

        if(_dataBases.ContainsKey(key))
        {
            Debug.LogError($"{(connectedObject ? $"{connectedObject.name}の" : "")}{dataType}は既に登録されています。データベースを二重に登録することはできません。");
            return;
        }

        _dataBases.Add(key, data);
        Debug.Log($"{(connectedObject ? $"{connectedObject.name}に" : "")}{dataType}をデータベースに登録しました。");
    }

    /// <summary>
    /// データを読み取る
    /// </summary>
    /// <typeparam name="T">データの型</typeparam>
    /// <returns>データの値</returns>
    public static T ReadData<T>(GameObject connectedObject = null) where T : IData, new()
    {
        var dataType = typeof(T);
        var key = dataType.ToString();

        if(connectedObject)
        {
            key += $"_{connectedObject.GetEntityId()}";
        }

        if(!_dataBases.ContainsKey(key))
        {
            Debug.LogError($"{(connectedObject ? $"{connectedObject.name}の" : "")}{dataType}はデータベースに登録されていません。");
            return new T();
        }

        return (T)_dataBases[key];
    }
}
