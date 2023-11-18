using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static readonly Color transparentColor = new(0f, 0f, 0f, 0f);

    public static float Remap(float n, float start1, float stop1, float start2, float stop2) => (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;

    public static Vector3 Direction(Vector3 from, Vector3 to) => (to - from).normalized;

    public static float SquaredDistance(this Vector3 v1, Vector3 v2)
    {
        float X = v1.x - v2.x;
        float Y = v1.y - v2.y;
        float Z = v1.z - v2.z;

        return X * X + Y * Y + Z * Z;
    }

    public static Vector3 RandomPositionInsideBounds(Bounds bounds) =>
        new(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );

    public static void DestroyChildren(Transform transform)
    {
        // from: https://answers.unity.com/questions/678069/destroyimmediate-on-children-not-working-correctly.html
        var tempList = transform.Cast<Transform>().ToList();

        foreach (var child in tempList)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

    public static IEnumerator LerpTransformScale(Transform transform, Vector3 targetScale, float time)
    {
        Vector3 unnormalizedDirection = targetScale - transform.localScale;

        Vector3 direction = unnormalizedDirection.normalized;

        float totalDistance = unnormalizedDirection.magnitude;

        float amtPerSecond = totalDistance / time;

        while (transform.localScale != targetScale)
        {
            float amtPerFrame = amtPerSecond * Time.deltaTime;

            float currentDistance = Vector3.Distance(transform.localScale, targetScale);

            transform.localScale += Vector3.ClampMagnitude(direction * amtPerFrame, currentDistance);

            yield return null;
        }
    }

    public static IEnumerator LerpColorFromTo(Graphic graphic, Color from, Color to, float time)
    {
        graphic.color = from;

        float amtPerSecond = 1f / time;

        for (float t = 0f; t < 1f; t += amtPerSecond * Time.deltaTime)
        {
            graphic.color = Color.Lerp(from, to, t);
            yield return null;
        }

        graphic.color = to;
    }

    public static IEnumerator LerpPropertyFromTo(object target, string propertyName, System.Func<object, object, float, object> lerpFunction, object from, object to, float time)
    {
        var targetType = target.GetType();

        void Set(object val) => targetType.GetProperty(propertyName).SetValue(target, val);

        Set(from);

        float amtPerSecond = 1f / time;

        for (float t = 0f; t < 1f; t += amtPerSecond * Time.deltaTime)
        {
            Set(lerpFunction(from, to, t));
            yield return null;
        }

        Set(to);
    }
}
