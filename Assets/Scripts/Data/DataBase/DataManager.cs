using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各種データベースを管理するクラス
/// </summary>
public class DataManager : MonoBehaviour
{
    [SerializeField]
    private List<DataBase> _defaultDataBases;

    private static Dictionary<Type, IData> _dataBases = new();
    /// <summary> データベースを保持するディクショナリー </summary>
    public static Dictionary<Type, IData> DataBases => _dataBases;

    void Awake()
    {
        _defaultDataBases.ForEach(dataBase =>
        {
            AddData(dataBase);
        });
    }

    /// <summary>
    /// データを追加する
    /// </summary>
    /// <typeparam name="T">データの型</typeparam>
    /// <param name="dataBase">データの値</param>
    public static void AddData<T>(ref T dataBase) where T : IData
    {
        var dataType = dataBase.GetType();
        if(_dataBases.ContainsKey(dataType))
        {
            Debug.LogError($"{dataType}は既に登録されています。データベースを二重に登録することはできません。");
            return;
        }

        _dataBases.Add(dataType, dataBase);
        Debug.Log($"{dataType}をデータベースに登録しました。");
    }

    /// <summary>
    /// データベースを追加する
    /// </summary>
    /// <typeparam name="T">データベースの型</typeparam>
    /// <param name="dataBase">データベースの値</param>
    public static void AddData<T>(T dataBase) where T : DataBase
    {
        AddData(ref dataBase);
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
