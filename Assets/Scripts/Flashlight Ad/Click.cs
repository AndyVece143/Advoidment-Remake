using UnityEngine;

public class Click : MonoBehaviour
{
    public bool isClicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        isClicked = true;
    }

}
