using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 OnlyX(this Vector3 v) => new(v.x, 0f, 0f);

    public static Vector3 OnlyY(this Vector3 v) => new(0f, v.y, 0f);

    public static Vector3 OnlyZ(this Vector3 v) => new(0f, 0f, v.z);

    public static Vector3 WithX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }
}
