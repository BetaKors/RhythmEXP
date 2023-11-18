using UnityEngine;

public class Player : MonoBehaviour
{
    public static bool UseParticles { get; private set; } = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            UseParticles = !UseParticles;
        }
    }
}
