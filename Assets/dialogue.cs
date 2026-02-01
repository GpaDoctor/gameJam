using UnityEngine;

public class dialogue : MonoBehaviour
{
    private playerStatus playerStatus;
    private bool playerInTrigger = false;
    private bool dialogueShown = false;

    [Header("Dialogue Settings")]
    public Color correctMaskColour = Color.magenta;
    public string failureDialogue = "You need to wear the correct mask!";
    public string successDialogue = "Great! You have the correct mask. Welcome!";
    public Transform successLocation; // Location to move player to if mask is correct

    void Start()
    {
        playerStatus = FindObjectOfType<playerStatus>();
    }

    void Update()
    {
        // Only show dialogue once when player is in trigger
        if (playerInTrigger && !dialogueShown)
        {
            CheckPlayerStatus();
            dialogueShown = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            dialogueShown = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            dialogueShown = false;
        }
    }

    private void CheckPlayerStatus()
    {
        if (playerStatus == null) return;

        // Check if player has mask and correct colour
        if (playerStatus.hasMask && playerStatus.maskColour == correctMaskColour)
        {
            // Success: Show success dialogue and move player
            DisplayDialogue(successDialogue);
            
            if (successLocation != null)
            {
                // Teleport the player (playerStatus gameobject) to success location
                playerStatus.gameObject.transform.position = successLocation.position;
                playerStatus.gameObject.transform.rotation = successLocation.rotation;
            }
        }
        else
        {
            // Failure: Show failure dialogue
            DisplayDialogue(failureDialogue);
        }
    }

    private void DisplayDialogue(string message)
    {
        Debug.Log("Dialogue: " + message);
        // This will be displayed in OnGUI below
    }

    private void OnGUI()
    {
        if (playerInTrigger && dialogueShown)
        {
            // Draw dialogue box in center of screen
            GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
            GUI.Box(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 75, 400, 150), "");

            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;

            // Determine which dialogue to show
            string dialogueText = "";
            if (playerStatus != null && playerStatus.hasMask && playerStatus.maskColour == correctMaskColour)
            {
                dialogueText = successDialogue;
            }
            else
            {
                dialogueText = failureDialogue;
            }

            GUI.Label(new Rect(Screen.width / 2 - 180, Screen.height / 2 - 50, 360, 120), dialogueText, 
                new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true, fontSize = 16 });

            GUI.contentColor = Color.white;
        }
    }
}
