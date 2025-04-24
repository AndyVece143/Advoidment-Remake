using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using Unity.VisualScripting;

public class AdManager : MonoBehaviour
{
    public WaitingAd waitingAd;
    public List<Advertisement> ads = new List<Advertisement>();
    public List<Advertisement> activeAds = new List<Advertisement>();

    public int maxAds = 1;

    public bool gracePeriod = false;

    private System.Random rand = new System.Random();

    private int previousNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Instantiate(waitingAd);

    }

    // Update is called once per frame
    void Update()
    {
        if (gracePeriod == false && activeAds.Count == 0)
        {
            CreateAd();
        }
        try
        {
            if (activeAds[0] == null)
            {
                activeAds.RemoveAt(0);

                StartCoroutine(GracePeriod());
            }
        }
        catch { }
    }

    private void CreateAd()
    {
        Advertisement ad = Instantiate(ads[RandomNumber()]);
        activeAds.Add(ad);
    }

    private int RandomNumber()
    {
        int i = rand.Next(0, ads.Count);
        if (i == previousNumber)
        {
            return RandomNumber();
        }

        else
        {
            previousNumber = i;
            return i;
        }
    }

    private IEnumerator GracePeriod()
    {
        gracePeriod = true;
        yield return new WaitForSeconds(2f);
        gracePeriod = false;
    }
}
