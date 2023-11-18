using System.Diagnostics;
using UnityEngine;

public class NoteCollectionAreaDrawer : MonoBehaviour
{
    [Conditional("UNITY_EDITOR")]
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red * 2f;
        
        float lowerZ = Line.Instance.Z - GameManager.MISS_LEEWAY;
        float upperZ = Line.Instance.Z + GameManager.MISS_DISTANCE;

        Vector3 line = new() { x = Line.Instance.Length, y = Line.Instance.Height };

        Vector3 lowerLine = line.WithZ(lowerZ);
        Vector3 upperLine = line.WithZ(upperZ);

        Gizmos.DrawLine(lowerLine, lowerLine.WithX(-lowerLine.x));
        Gizmos.DrawLine(upperLine, upperLine.WithX(-upperLine.x));
    }
}
