using System;
using UnityEngine;

public struct Size2 {
    public int x, y;

    public Size2(int v1, int v2)
    {
        this.x = v1;
        this.y = v2;
    }

    static public Size2 Convert(string value)
    {
        string[] size = value.Split(',');
        return new Size2(System.Convert.ToInt32(size[0]),
            System.Convert.ToInt32(size[1]));
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    static public Size2 operator+(Size2 A, Size2 B)
    {
        return new Size2(A.x + B.x, A.y + B.y);
    }
}