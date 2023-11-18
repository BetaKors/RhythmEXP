using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public static class GameManager
{
    public static List<Lane> Lanes { get; } = new();
    public static float Score { get; set; }
    public static int Misses { get; set; }

    public const float NOTE_DISAPPEAR_Z = -8f;
    public const float MIN_NOTE_SPEED = .1f;

    public const float MISS_DISTANCE = 3f;
    public const float MISS_LEEWAY = 0.35f;

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        var text = File.ReadAllText("Assets/Songs/Test1.json");
        var song = JsonConvert.DeserializeObject<Dictionary<string, List<float>>>(text);

        foreach (var (label, times) in song)
        {
            var lane = Lanes.Where(l => l.Label == label).First();

            if (lane == null)
            {
                Debug.LogWarning($"Lane with label {label} was not found. Skipping...");
                continue;
            }

            lane.StartPlaying(times);
        }
    }
}
