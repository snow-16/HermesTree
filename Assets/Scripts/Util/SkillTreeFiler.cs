using UnityEngine;
using Newtonsoft.Json; 

public static class SkillTreeFiler
{
    public static void WriteTree(SkillTreeData.TreeData data)
    {
        var j = JsonConvert.SerializeObject(new int[5, 5], Formatting.Indented);
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string filePath = GeneratePath($"Tree/{data.name}");
        Debug.Log(j);
        // File.WriteAllText(filePath, json);
    }

    public static void WriteChart(SkillTreeData.ChartData data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string filePath = GeneratePath($"Chart/{data.name}");
        Debug.Log(json);
        // File.WriteAllText(filePath, json);
    }

    private static string GeneratePath(string fileName)
    {
        return Application.persistentDataPath + $"/Assets/SaveDatas/Bullet/{fileName}.json";
    }
}
