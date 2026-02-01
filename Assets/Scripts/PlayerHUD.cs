using UnityEngine;
using UnityEngine.UI;
using TMPro;                      // if using TextMeshPro
using UnityEngine.InputSystem;  // ← Add this!

public class PlayerHUD : MonoBehaviour
{
    [Header("Health")]
    public Slider healthSlider;       // drag the Slider here
    public float maxHealth = 100f;
    private float currentHealth;
    private playerStatus playerStatus;
    private bool levelFailed = false;

    [Header("Timer")]
    public TextMeshProUGUI timerText; // drag the TMP text here (or use UnityEngine.UI.Text)
    private float gameTime = 0f;

    public InputAction debugDamageAction;     // Drag or reference here
    void Awake()
    {
        // Start full health
        currentHealth = maxHealth;
        UpdateHealthUI();

        debugDamageAction = new InputAction("DebugDamage", binding: "<Keyboard>/q");
        debugDamageAction.performed += ctx => TakeDamage(25f);
    }

    void Start()
    {
        // Find the player status script
        playerStatus = FindObjectOfType<playerStatus>();
    }
    void OnEnable()
    {
        debugDamageAction?.Enable();
    }

    void OnDisable()
    {
        debugDamageAction?.Disable();
    }

    void Update()
    {
        // Sync health from playerStatus
        if (playerStatus != null)
        {
            currentHealth = (float)playerStatus.hp;
            UpdateHealthUI();

            // Check if player has died
            if (currentHealth <= 0 && !levelFailed)
            {
                levelFailed = true;
                Debug.Log("Player died! Level failed.");
            }
        }

        // Simple timer counting up (you can change to countdown)
        gameTime += Time.deltaTime;
        UpdateTimerUI();

        // For testing: press Q to take damage
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     TakeDamage(25f);
        // }
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Player died!");
            // Game over logic here...
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        // Alternative countdown style:
        // float timeLeft = 300f - gameTime; ...
    }

    // Optional: public method so other scripts can call it
    public void SetHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    private void OnGUI()
    {
        if (levelFailed)
        {
            // Draw black background
            GUI.backgroundColor = Color.black;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            // Draw "Level Failed" text
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.red;
            GUIStyle largeStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 60 };
            GUI.Label(new Rect(0, Screen.height / 2 - 100, Screen.width, 200), "YOU HAVE FAILED THIS LEVEL", largeStyle);

            // Reset content color
            GUI.contentColor = Color.white;
        }
    }
}