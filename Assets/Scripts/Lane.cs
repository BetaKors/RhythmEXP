using Debug = UnityEngine.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Lane : MonoBehaviour
{
    public List<Note> CurrentNotes { get; } = new();

    public string Key => key;

    public string Label => IsKeyValid() ? key : "?";

    public bool IsEmpty => CurrentNotes.Count == 0;

    [Header("Required Stuffers")]

    [SerializeField]
    private GameObject notePrefab;

    [SerializeField]
    private GameObject explosionParticleSystemPrefab;

    [SerializeField]
    private TextMeshPro text;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private string key = string.Empty;

    [Header("Animation Settings")]

    [SerializeField]
    private float animationTime = 0.1f;

    [Header("Line Renderer Settings")]

    [SerializeField, Range(0f, 1f)]
    private float thickness = 0.1f;

    [SerializeField, FormerlySerializedAs("color")]
    private Color normalColor = Color.white;

    [SerializeField]
    private Color highlightColor = Color.white;

    private static GUIStyle style;

    private Material material;

    private bool animating;

    [HideInInspector]
    public float startTime;

    void Awake()
    {
        material = new(Shader.Find("Universal Render Pipeline/Lit")) { color = normalColor, enableInstancing = true };

        if (!IsKeyValid())
        {
            Debug.LogWarning($"GameObject {gameObject.name} is missing a key!");
            return;
        }

        GameManager.Lanes.Add(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            StartCoroutine(AnimateLaneActivation());

            if (!IsEmpty)
            {
                CollectClosestNote();
            }
        }
    }

    public void StartPlaying(List<float> times)
    {
        times.Sort();

        DateTime start = DateTime.Now;

        IEnumerator _Play()
        {
            foreach (var time in times)
            {
                var seconds = (float) (start + TimeSpan.FromSeconds(time) - DateTime.Now).TotalSeconds;

                yield return new WaitForSeconds(seconds);

                AddNote();
            }
        }

        StartCoroutine(_Play());

        startTime = Time.realtimeSinceStartup;
    }

    private void CollectClosestNote()
    {
        Note note = CurrentNotes[0];

        float noteZ = note.transform.position.z;

        float distance = note.DistanceFromLine;

        if (Line.Instance.Z - GameManager.MISS_LEEWAY > noteZ || distance >= GameManager.MISS_DISTANCE)
        {
            GameManager.Misses++;
        }
        else
        {
            GameManager.Score += GameManager.MISS_DISTANCE - distance;
        }

        RemoveNote(note, animate: true);

        if (Player.UseParticles)
        {
            var obj = Instantiate(explosionParticleSystemPrefab, note.transform.position, Quaternion.identity);
            obj.GetComponent<ParticleSystemRenderer>().material = note.GetComponent<Renderer>().material;
        }
    }

    private void AddNote()
    {
        var obj = Instantiate(notePrefab, transform.position, Quaternion.identity);

        obj.GetComponent<Renderer>().material = material;

        var note = obj.GetComponent<Note>();
        note.lane = this;

        CurrentNotes.Add(note);
    }

    public void RemoveNote(Note note, bool animate)
    {
        CurrentNotes.Remove(note);

        if (animate)
        {
            StartCoroutine(AnimateNoteCollection(note));
        }
        else
        {
            Destroy(note.gameObject);
        }
    }

    private IEnumerator AnimateLaneActivation()
    {
        if (animating) yield break;

        animating = true;

        StartCoroutine(Utils.LerpTransformScale(text.transform, Vector3.one * 1.5f, animationTime));
        StartCoroutine(Utils.LerpColorFromTo(text, normalColor, highlightColor, animationTime));
        StartCoroutine(LerpLineRendererColor(normalColor, highlightColor, animationTime));
        LerpLineRendererWidth(thickness, thickness * 1.5f, animationTime);

        yield return new WaitForSeconds(animationTime);

        StartCoroutine(Utils.LerpTransformScale(text.transform, Vector3.one, animationTime));
        StartCoroutine(Utils.LerpColorFromTo(text, highlightColor, normalColor, animationTime));
        StartCoroutine(LerpLineRendererColor(highlightColor, normalColor, animationTime));
        LerpLineRendererWidth(thickness * 1.5f, thickness, animationTime);

        yield return new WaitForSeconds(animationTime);

        animating = false;
    }

    private IEnumerator AnimateNoteCollection(Note note)
    {
        yield return StartCoroutine(Utils.LerpTransformScale(note.transform, note.transform.localScale * 1.2f, 0.05f));
        yield return StartCoroutine(Utils.LerpTransformScale(note.transform, Vector3.zero, 0.15f));

        Destroy(note.gameObject);
    }

    private bool IsKeyValid()
    {
        try
        {
            Input.GetKey(key);
        }
        catch
        {
            return false;
        }

        return true;
    }

    private void LerpLineRendererWidth(float from, float to, float time)
    {
        System.Func<object, object, float, object> lerp = (v1, v2, t) => Mathf.Lerp((float) v1, (float) v2, t);

        StartCoroutine(Utils.LerpPropertyFromTo(lineRenderer, "startWidth", lerp, from, to, time));
        StartCoroutine(Utils.LerpPropertyFromTo(lineRenderer, "endWidth", lerp, from, to, time));
    }

    private IEnumerator LerpLineRendererColor(Color from, Color to, float time)
    {
        void SetLRColor(Color c)
        {
            lineRenderer.startColor = c;
            lineRenderer.endColor = c;
        }

        SetLRColor(from);

        float amtPerSecond = 1f / time;

        for (float t = 0f; t < 1f; t += amtPerSecond * Time.deltaTime)
        {
            SetLRColor(Color.Lerp(from, to, t));
            yield return null;
        }

        SetLRColor(to);
    }

    [Conditional("UNITY_EDITOR")]
    void OnDrawGizmos()
    {
        style ??= new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = false,
            fontSize = 32,            
        };

        style.normal.textColor = normalColor;

        Handles.Label(transform.position, Label, style);

        if (transform.hasChanged)
        {
            lineRenderer.SetPositions(new Vector3[]
            {
                new(transform.position.x, Line.Instance.Height - 0.1f, transform.position.z),
                new(transform.position.x, Line.Instance.Height - 0.1f, Line.Instance.Z),
            });

            transform.hasChanged = false;
        }
    }

    [Conditional("UNITY_EDITOR")]
    void OnValidate()
    {
        if (lineRenderer == null) return;

        lineRenderer.startColor = normalColor;
        lineRenderer.endColor = normalColor;

        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;

        if (text == null) return;

        text.text = Label;
        text.color = normalColor;
    }
}
