using UnityEngine;
using UnityEngine.InputSystem;

public class unlockDoor : MonoBehaviour
{
    private bool isTriggered = false;
    public GameObject parentToDeactivate;
    private playerStatus player;
    private maskCheck alarmScript;
    public float holdDuration = 5f; // Duration in seconds that E must be held
    private float holdTimer = 0f;
    private AudioSource audioSource;
    private bool levelPassed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<playerStatus>();
        alarmScript = FindObjectOfType<maskCheck>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the object is triggered and the player is holding E
        if (isTriggered && Keyboard.current.eKey.isPressed)
        {
            holdTimer += Time.deltaTime;

            // Play audio while holding E
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // Check if the key has been held long enough
            if (holdTimer >= holdDuration)
            {
                if (parentToDeactivate != null)
                {   
                    if (alarmScript != null)
                    {
                        alarmScript.alarmActive = false;
                    }
                    parentToDeactivate.SetActive(false);
                }
                levelPassed = true;
                holdTimer = 0f; // Reset the timer
            }
        }
        else
        {
            holdTimer = 0f; // Reset timer if key is not pressed
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    // Called when another collider enters this object's collider
    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
    }

    // Called when another collider exits this object's collider
    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
        holdTimer = 0f; // Reset timer when exiting trigger
    }

    // Draws GUI prompt on screen
    private void OnGUI()
    {
        // Display fullscreen level passed screen
        if (levelPassed)
        {
            // Draw black background
            GUI.backgroundColor = Color.black;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            // Draw "Level Passed" text
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            GUIStyle largeStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 60 };
            GUI.Label(new Rect(0, Screen.height / 2 - 100, Screen.width, 200), "YOU HAVE PASSED THE LEVEL", largeStyle);

            // Reset content color
            GUI.contentColor = Color.white;
            return;
        }

        if (isTriggered)
        {
            // Display prompt in the center of the screen
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 50), "hold e to unlock door");

            // Draw circular progress indicator
            float circleX = Screen.width / 2f;
            float circleY = Screen.height / 2f - 50f;
            float circleRadius = 50f;

            // Draw background circle
            DrawCircle(circleX, circleY, circleRadius, Color.gray);

            // Draw progress circle based on hold time
            float progress = Mathf.Clamp01(holdTimer / holdDuration);
            DrawFilledCircle(circleX, circleY, circleRadius, progress, Color.green);
        }
    }

    // Helper method to draw a circle outline
    private void DrawCircle(float x, float y, float radius, Color color)
    {
        int segments = 30;
        float angleStep = 360f / segments;

        Texture2D circleTex = new Texture2D(1, 1);
        circleTex.SetPixel(0, 0, color);
        circleTex.Apply();

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;

            float x1 = x + radius * Mathf.Cos(angle1);
            float y1 = y + radius * Mathf.Sin(angle1);
            float x2 = x + radius * Mathf.Cos(angle2);
            float y2 = y + radius * Mathf.Sin(angle2);

            GUI.DrawTexture(new Rect(x1, y1, 2, 2), circleTex);
        }
    }

    // Helper method to draw a filled arc/circle based on progress
    private void DrawFilledCircle(float x, float y, float radius, float progress, Color color)
    {
        int segments = Mathf.Max(1, (int)(30 * progress));
        float maxAngle = 360f * progress;
        float angleStep = maxAngle / segments;

        Texture2D circleTex = new Texture2D(1, 1);
        circleTex.SetPixel(0, 0, color);
        circleTex.Apply();

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float px = x + radius * Mathf.Cos(angle);
            float py = y + radius * Mathf.Sin(angle);

            GUI.DrawTexture(new Rect(px - 2, py - 2, 4, 4), circleTex);
        }

        // Draw line to center for filled effect
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float px = x + radius * Mathf.Cos(angle);
            float py = y + radius * Mathf.Sin(angle);
            GUI.DrawTexture(new Rect(Mathf.Lerp(x, px, 0.1f), Mathf.Lerp(y, py, 0.1f), 2, 2), circleTex);
        }
    }
}
