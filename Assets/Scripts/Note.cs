using UnityEngine;

// time in seconds from spawning to reaching line = 4.665

public class Note : MonoBehaviour
{
    [HideInInspector]
    public Lane lane;

    public float DistanceFromLine => Mathf.Abs(Line.Instance.Z - transform.position.z);
    public float Speed => Mathf.Max(GameManager.MIN_NOTE_SPEED, Mathf.Pow(DistanceFromLine, 0.9f) * Time.deltaTime);

    private float startTime;

    private float reachedLineAt;
    private bool reached;

    void Start() => startTime = Time.realtimeSinceStartup;

    void Update()
    {
        if (DistanceFromLine < 0.1f && !reached)
        {
            reachedLineAt = Time.realtimeSinceStartup;
            reached = true;
        }

        if (transform.position.z < GameManager.NOTE_DISAPPEAR_Z)
        {
            GameManager.Misses++;
            lane.RemoveNote(this, animate: false);
        }
    }

    void FixedUpdate() => transform.position += Vector3.back * Speed * Time.deltaTime * 60f;

    void OnDestroy() => Debug.Log($"Note from lane *{lane.Label.ToUpper()}* took {reachedLineAt - lane.startTime}s to reach the line, {Time.realtimeSinceStartup - startTime}s since its inception to be destroyed and {Time.realtimeSinceStartup - lane.startTime}s since the start of the game to be destroyed.");
}