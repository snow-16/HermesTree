using System;
using UnityEngine;

[Serializable]
public struct SimplePosition : IFormattable
{
    public int x;
    public int y;

    public SimplePosition(int posX, int posY)
    {
        x = posX;
        y = posY;
    }

    public SimplePosition(Vector2 pos)
    {
        x = (int)pos.x;
        y = (int)pos.y;
    }

    public Vector2 ConvertVector()
    {
        return new(x, y);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"({x}, {y})";
    }
}
