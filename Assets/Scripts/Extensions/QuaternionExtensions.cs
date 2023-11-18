using UnityEngine;

public static class QuaternionExtensions
{
    public static Quaternion OnlyX(this Quaternion q) => Quaternion.Euler(q.eulerAngles.x, 0f, 0f);

    public static Quaternion OnlyY(this Quaternion q) => Quaternion.Euler(0f, q.eulerAngles.y, 0f);

    public static Quaternion OnlyZ(this Quaternion q) => Quaternion.Euler(0f, 0f, q.eulerAngles.z);

    public static Quaternion WithEulerX(this Quaternion q, float x)
    {
        q.eulerAngles = q.eulerAngles.WithX(x);
        return q;
    }

    public static Quaternion WithEulerY(this Quaternion q, float y)
    {
        q.eulerAngles = q.eulerAngles.WithY(y);
        return q;
    }

    public static Quaternion WithEulerZ(this Quaternion q, float z)
    {
        q.eulerAngles = q.eulerAngles.WithZ(z);
        return q;
    }

    public static Vector3 GetForward(this Quaternion q) => q * Vector3.forward;

    public static Vector3 GetBack(this Quaternion q) => q * Vector3.back;

    public static Vector3 GetRight(this Quaternion q) => q * Vector3.right;

    public static Vector3 GetLeft(this Quaternion q) => q * Vector3.left;

    public static Vector3 GetUp(this Quaternion q) => q * Vector3.up;

    public static Vector3 GetDown(this Quaternion q) => q * Vector3.down;
}
