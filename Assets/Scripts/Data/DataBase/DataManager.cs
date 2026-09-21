using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各種データベースを管理するクラス
/// </summary>
public static class DataManager
{
    private static Dictionary<Type, IData> _dataBases = new();
    /// <summary> データベースを保持するディクショナリー </summary>
    public static Dictionary<Type, IData> DataBases => _dataBases;

    /// <summary>
    /// データを追加する
    /// </summary>
    /// <typeparam name="T">データの型</typeparam>
    /// <param name="data">データの値</param>
    public static void AddData<T>(T data) where T : IData
    {
        var dataType = data.GetType();
        if(_dataBases.ContainsKey(dataType))
        {
            Debug.LogError($"{dataType}は既に登録されています。データベースを二重に登録することはできません。");
            return;
        }

        _dataBases.Add(dataType, data);
        Debug.Log($"{dataType}をデータベースに登録しました。");
    }

    /// <summary>
    /// データを読み取る
    /// </summary>
    /// <typeparam name="T">データの型</typeparam>
    /// <returns>データの値</returns>
    public static T ReadData<T>() where T : IData, new()
    {
        var dataType = typeof(T);
        if(!_dataBases.ContainsKey(dataType))
        {
            Debug.LogError($"{dataType}はデータベースに登録されていません。");
            return new T();
        }

        return (T)_dataBases[dataType];
    }
}
