using UnityEngine;

public class playerStatus : MonoBehaviour
{
    public bool hasMask = false;
    public Color maskColour = Color.clear;
    public double hp = 100.0;
    public GameObject maskObject;

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
    }
}