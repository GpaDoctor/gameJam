using UnityEngine;

public class playerStatus : MonoBehaviour
{
    public bool hasMask = false;
    public Color maskColour = Color.clear;
    public double hp = 100.0;
    public GameObject maskObject;
    public Transform colorChangeTransform; // Transform whose color will change
    private float hpDamageTimer = 0f;
    private float hpDamageInterval = 1f; // Damage every 1 second
    private double hpDamageAmount = 20.0; // Damage amount per interval

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (maskObject != null)
        {
            maskObject.SetActive(hasMask);
        }

        // Change color of the transform if assigned
        if (colorChangeTransform != null && hasMask)
        {
            Renderer renderer = colorChangeTransform.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = maskColour;
            }
        }

        // Apply damage from alarm
        ApplyAlarmDamage();
    }

    private void ApplyAlarmDamage()
    {
        maskCheck alarmScript = FindObjectOfType<maskCheck>();
        if (alarmScript != null && alarmScript.alarmActive)
        {
            hpDamageTimer += Time.deltaTime;

            if (hpDamageTimer >= hpDamageInterval)
            {
                hp -= hpDamageAmount;
                hpDamageTimer = 0f;

                // Clamp HP to not go below 0
                if (hp < 0)
                {
                    hp = 0;
                }

                Debug.Log("Alarm damage! HP: " + hp);
            }
        }
        else
        {
            // Reset timer when alarm is not active
            hpDamageTimer = 0f;
        }
    }
}