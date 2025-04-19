using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public abstract class Advertisement : MonoBehaviour
{
    protected Vector3 scaleChange = new Vector3(0.01f, 0.01f, 0.01f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public abstract void CreateAd();

    public void ChangeScale(bool rate)
    {
        if (rate == true)
        {
            transform.localScale += scaleChange;
        }
        else
        {
            transform.localScale -= scaleChange;
        }
    }

    protected abstract IEnumerator waiter();
    protected abstract IEnumerator waiterDeath();
}
