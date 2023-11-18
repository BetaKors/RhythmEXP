using UnityEngine;
using TMPro;

public class SMText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    void Update() => text.text = $"Score: {FormatFloat(GameManager.Score)}\nMisses: {GameManager.Misses}";

    private static string FormatFloat(float n, uint places=1)
    {
        string o = n.ToString($"F{places}").Replace(',', '.');
        return o.EndsWith(".0") ? o.Substring(0, o.Length - 2) : o;
    }
}
