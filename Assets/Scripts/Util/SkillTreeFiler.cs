using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class SkillTreeFiler
{
    public static void WriteTree(SkillTreeData.TreeData data)
    {
        var saveDirectory = Path.Combine(Application.persistentDataPath, "Tree");
        if(!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string filePath = Path.Combine(saveDirectory, $"{data.name}.json");
        File.WriteAllText(filePath, json);
        Debug.Log($"{filePath}に{data.name}をスキルツリーファイルとして保存しました。");
    }

    public static void WriteChart(SkillTreeData.ChartData data)
    {
        // var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        // string filePath = GeneratePath($"Chart/{data.name}");
        // Debug.Log(json);
        // File.WriteAllText(filePath, json);
    }
}
