using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clicking : MonoBehaviour
{
    public int click = 0;
    public AdManager adManager;
    public TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + click;
    }

    private void OnMouseDown()
    {
        if (adManager.gracePeriod == true)
        {
            click++;
        }

    }
}
