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
        Converters = new List<JsonConverter>(){new Vector2KeyConverter()}
    };
    
    public static void WriteTree(SkillTreeData.TreeData data)
    {
        var saveDirectory = Path.Combine(Application.persistentDataPath, "Tree");
        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        var json = JsonConvert.SerializeObject(data, Formatting.Indented, _serializerSettings);
        string filePath = Path.Combine(saveDirectory, $"{data.name}.json");
        File.WriteAllText(filePath, json);
        Debug.Log($"{filePath}に{data.name}をスキルツリーファイルとして保存しました。");
    }

    public static List<SkillTreeData.TreeData> ReadTree()
    {
        var saveDirectory = Path.Combine(Application.persistentDataPath, "Tree");
        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        var allTrees = Directory.GetFiles(saveDirectory, "*", SearchOption.TopDirectoryOnly).Where(file => Path.GetExtension(file) == ".json");
        return allTrees.ToList().Select(tree =>
        {
            var treeText = File.ReadAllText(tree);
            var treeData = JsonConvert.DeserializeObject<SkillTreeData.TreeData>(treeText, _serializerSettings);
            Debug.Log($"{tree}から{treeData.name}をスキルツリーファイルとしてロードしました。");
            return treeData;
        }).ToList();
    }

    public static void WriteChart(SkillTreeData.ChartData data)
    {
        // var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        // string filePath = GeneratePath($"Chart/{data.name}");
        // Debug.Log(json);
        // File.WriteAllText(filePath, json);
    }
}
