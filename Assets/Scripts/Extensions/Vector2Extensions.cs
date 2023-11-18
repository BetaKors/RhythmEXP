using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 OnlyX(this Vector2 v) => new(v.x, 0f);

    public static Vector2 OnlyY(this Vector2 v) => new(0f, v.y);

    public static Vector2 WithX(this Vector2 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector2 WithY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 WithZ(this Vector2 v, float z) => new(v.x, v.y, z);
}
