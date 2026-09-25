using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class SkillTreeFiler
{
    private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Converters = new List<JsonConverter>(){new Position2KeyConverter()},
        TypeNameHandling = TypeNameHandling.Auto
    };
    
    public static void WriteTree(SkillTreeData.TreeData data)
    {
        var saveDirectory = Path.Combine(Application.persistentDataPath, "Tree");
        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        var json = JsonConvert.SerializeObject(data, Formatting.Indented, _serializerSettings);
        string filePath = Path.Combine(saveDirectory, $"{data.id}.json");
        File.WriteAllText(filePath, json);
        Debug.Log($"{filePath}に{data.name}をスキルツリーファイルとして保存しました。");
    }

    public static SkillTreeData.TreeData ReadTreeFromText(string text)
    {
        var treeData = JsonConvert.DeserializeObject<SkillTreeData.TreeData>(text, _serializerSettings);
        Debug.Log($"{treeData.name}をデシリアライズ");
        return treeData;
    }

    public static SkillTreeData.TreeData ReadTree(string file)
    {
        var data = ReadTreeFromText(File.ReadAllText(file));
        Debug.Log($"{file}から{data.name}をスキルツリーファイルとしてロードしました。");
        return data;
    }

    public static List<SkillTreeData.TreeData> ReadAllTrees()
    {
        var saveDirectory = Path.Combine(Application.persistentDataPath, "Tree");
        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        var allTrees = Directory.GetFiles(saveDirectory, "*", SearchOption.TopDirectoryOnly).Where(file => Path.GetExtension(file) == ".json");
        return allTrees.ToList().Select(ReadTree).ToList();
    }

    public static void WriteChart(SkillTreeData.ChartData data)
    {
        var saveDirectory = Path.Combine(Application.persistentDataPath, "Chart");
        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        var json = JsonConvert.SerializeObject(data, Formatting.Indented, _serializerSettings);
        string filePath = Path.Combine(saveDirectory, $"{data.id}.json");
        File.WriteAllText(filePath, json);
        Debug.Log($"{filePath}に{data.name}をスキルチャートファイルとして保存しました。");
    }

    public static SkillTreeData.ChartData ReadChartFromText(string text)
    {
        var chartData = JsonConvert.DeserializeObject<SkillTreeData.ChartData>(text, _serializerSettings);
        Debug.Log($"{chartData.name}をデシリアライズ");
        return chartData;
    }

    public static SkillTreeData.ChartData ReadChart(string file)
    {
        var data = ReadChartFromText(File.ReadAllText(file));
        Debug.Log($"{file}から{data.name}をスキルチャートファイルとしてロードしました。");
        return data;
    }

    public static List<SkillTreeData.ChartData> ReadAllCharts()
    {
        var saveDirectory = Path.Combine(Application.persistentDataPath, "Chart");
        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        var allTrees = Directory.GetFiles(saveDirectory, "*", SearchOption.TopDirectoryOnly).Where(file => Path.GetExtension(file) == ".json");
        return allTrees.ToList().Select(ReadChart).ToList();
    }
}
