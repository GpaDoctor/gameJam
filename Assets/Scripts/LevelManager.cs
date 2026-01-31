using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("UI")]
    public Button nextLevelButton;         // Drag NextLevelButton here
    public GameObject completionPanel;     // Optional: Victory overlay panel

    [Header("Level Detection (Drag from unlockDoor Inspector!)")]
    public GameObject levelDoor;           // ← Drag unlockDoor's "Parent To Deactivate" here!

    [Header("Level")]
    public int nextSceneBuildIndex = -1;   // Auto: current +1

    [Header("Effects")]
    public AudioSource victorySound;       // Optional win SFX
    public float pauseTimeScale = 0f;      // 0 = full pause

    private bool levelComplete = false;
    private unlockDoor doorUnlocker;       // Auto-find to disable OnGUI

    void Start()
    {
        // Hide button/UI initially
        if (nextLevelButton != null)
            nextLevelButton.gameObject.SetActive(false);
        if (completionPanel != null)
            completionPanel.SetActive(false);

        // Auto-set next scene
        if (nextSceneBuildIndex < 0)
            nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Auto-find unlockDoor script (for disabling later)
        doorUnlocker = FindObjectOfType<unlockDoor>();
    }

    void Update()
    {
        // AUTO-DETECT: When door deactivates (unlock success!) → trigger completion
        if (!levelComplete && 
            levelDoor != null && 
            !levelDoor.activeInHierarchy)
        {
            OnLevelComplete();
        }
    }

    public void OnLevelComplete()
    {
        if (levelComplete) return;
        levelComplete = true;

        Debug.Log("Level Complete Detected! (Door unlocked)");

        // Show Next Level button + panel
        if (nextLevelButton != null)
            nextLevelButton.gameObject.SetActive(true);
        if (completionPanel != null)
            completionPanel.SetActive(true);

        // Victory effects
        if (victorySound != null)
            victorySound.Play();
        Time.timeScale = pauseTimeScale;  // Pause game

        // **CRUCIAL**: Disable unlockDoor to HIDE its fullscreen black "PASSED" screen!
        if (doorUnlocker != null)
        {
            doorUnlocker.enabled = false;  // Stops OnGUI + Update immediately
            Debug.Log("Disabled unlockDoor OnGUI overlay.");
        }
    }

    // Button OnClick (assign in Inspector)
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneBuildIndex);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}