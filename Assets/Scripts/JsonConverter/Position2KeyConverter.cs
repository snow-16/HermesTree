using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Position2KeyConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericType &&
        objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>) &&
        objectType.GetGenericArguments()[0] == typeof(SimplePosition);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var valueType = objectType.GetGenericArguments()[1];
        var dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(SimplePosition), valueType);
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
                var positionKey = reader.Value.ToString();
                var position = Key2Position(positionKey);

                //Valueを読んで変換
                reader.Read();
                var value = serializer.Deserialize(reader, valueType);

                dictionary.Add(position, value);
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
            var key = (SimplePosition)entry.Key;
            writer.WritePropertyName($"({key.x}, {key.y})");
            serializer.Serialize(writer, entry.Value);
        }

        writer.WriteEndObject();
    }

    private SimplePosition Key2Position(string key)
    {
        key = key.Trim('(', ')', ' ');
        string[] split = key.Split(',');

        if (split.Length == 2 && 
        int.TryParse(split[0], out int x) && 
        int.TryParse(split[1], out int y))
        {
            return new SimplePosition(x, y);
        }
        return new(0, 0);
    }
}
