using System.Diagnostics;
using UnityEngine;

public class Line : MonoBehaviour
{
    public static Line Instance => instance ??= GameObject.FindGameObjectWithTag("Line").GetComponent<Line>();

    [field: SerializeField]
    public float Length { get; set; } = 5f;

    [field: SerializeField]
    public float Height { get; set; } = 0.25f;

    [field: SerializeField]
    public float Z { get; set; } = -6f;

    [SerializeField]
    private LineRenderer lineRenderer;

    private static Line instance;

    [Conditional("UNITY_EDITOR")]
    void OnValidate()
    {
        if (lineRenderer == null) return;

        lineRenderer.SetPositions(new Vector3[]
        {
            new(Length, Height, Z),
            new(-Length, Height, Z),
        });
    }
}
