using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Vector2KeyConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericType &&
        objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>) &&
        objectType.GetGenericArguments()[0] == typeof(Vector2);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var valueType = objectType.GetGenericArguments()[1];
        var dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(Vector2), valueType);
        var dictionary = (IDictionary)Activator.CreateInstance(dictionaryType);

        if(reader.TokenType == JsonToken.StartObject)
        {
            //Dictionaryを見ている間ループ。最初にKeyを読む
            while(reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }

                //Keyを変換
                var vectorKey = reader.Value.ToString();
                var vector = Key2Vector2(vectorKey);

                //Valueを読んで変換
                reader.Read();
                var value = serializer.Deserialize(reader, valueType);

                dictionary.Add(vector, value);
            }
        }

        return dictionary;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var dictionary = (IDictionary)value;
        writer.WriteStartObject();

        foreach(DictionaryEntry entry in dictionary)
        {
            var key = (Vector2)entry.Key;
            writer.WritePropertyName($"({key.x:F2}, {key.y:F2})");
            serializer.Serialize(writer, entry.Value);
        }

        writer.WriteEndObject();
    }

    private Vector2 Key2Vector2(string key)
    {
        key = key.Trim('(', ')', ' ');
        string[] split = key.Split(',');

        if (split.Length == 2 && 
        float.TryParse(split[0], out float x) && 
        float.TryParse(split[1], out float y))
        {
            return new Vector2(x, y);
        }
        return Vector2.zero;
    }
}
