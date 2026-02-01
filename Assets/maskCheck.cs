using UnityEngine;

public class maskCheck : MonoBehaviour
{
    public Color passingColour = Color.magenta;
    public bool alarmActive = false;
    private playerStatus player;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<playerStatus>();
        audioSource = GetComponent<AudioSource>();
        GetComponent<Renderer>().material.color = passingColour;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alarmActive){
            audioSource.mute = true;
        }
    }

    // Called when another collider enters this object's collider
    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            if (player.maskColour == passingColour && player.hasMask == true)
            {
                alarmActive = false;
                audioSource.mute = true;
            }
            else
            {
                alarmActive = true;
                audioSource.mute = false;
            }
        }
    }
}
