using UnityEngine;

public class maskCheck : MonoBehaviour
{
    public Color passingColour = Color.magenta;
    public bool alarmActive = false;
    private playerStatus player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<playerStatus>();
        GetComponent<Renderer>().material.color = passingColour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when another collider enters this object's collider
    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            if (player.maskColour == passingColour)
            {
                alarmActive = false;
            }
            else
            {
                alarmActive = true;
            }
        }
    }
}
