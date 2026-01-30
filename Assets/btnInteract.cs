using UnityEngine;
using UnityEngine.InputSystem;

public class btnInteract : MonoBehaviour
{
    private bool isTriggered = false;
    public GameObject parentToDeactivate;
    private playerStatus player;
    private maskCheck alarmScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<playerStatus>();
        alarmScript = FindObjectOfType<maskCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the object is triggered and the player presses E
        if (isTriggered && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (parentToDeactivate != null && alarmScript.alarmActive == true && alarmScript != null)
            {   
                alarmScript.alarmActive = false;
                parentToDeactivate.SetActive(false);
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
    }

    // Draws GUI prompt on screen
    private void OnGUI()
    {
        if (isTriggered)
        {
            // Display prompt in the center of the screen
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 50), "press e to press button");
        }
    }
}
