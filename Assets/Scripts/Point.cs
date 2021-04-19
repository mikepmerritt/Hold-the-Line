using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    public int X, Y;

    public Point(int x, int y) {
        X = x;
        Y = y;
    }

    public override int GetHashCode()
    {
        return X ^ Y;
    }

    public override bool Equals(object obj)
    {
        return obj is Point && (((Point) obj).X == X && ((Point) obj).Y == Y);
    }
}