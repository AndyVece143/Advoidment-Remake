using UnityEngine;

public class AdManager : MonoBehaviour
{
    public WaitingAd waitingAd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(waitingAd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
