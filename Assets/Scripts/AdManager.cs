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

    public Clicking clicking;

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
        Advertisement ad = Instantiate(ads[RandomNumberAd()]);

        //Difficulty up, 50% chance of moving ad
        if (clicking.click >= 20 && clicking.click < 100 && RandomNumber(0,2) == 1)
        {
            ad.movingAd = true;
            ad.xSpeed = 0.0005f;
        }

        //Difficulty up 100% chance of moving ad
        if (clicking.click >= 100)
        {
            ad.movingAd = true;
            ad.xSpeed = Random.Range(0.001f, 0.003f);

            //50% chance of negative values
            if (RandomNumber(0,2) == 1)
            {
                ad.xSpeed *= -1;
            }
            if (RandomNumber(0, 2) == 1)
            {
                ad.ySpeed *= -1;
            }
            ad.ySpeed = Random.Range(0.0005f, 0.001f);
        }

        //Difficulty up, 50% chance of scaling ad
        activeAds.Add(ad);
    }

    private int RandomNumber(int lower, int higher)
    {
        return rand.Next(lower, higher);
    }

    private int RandomNumberAd()
    {
        int i = rand.Next(0, ads.Count);
        if (i == previousNumber)
        {
            return RandomNumberAd();
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
        yield return new WaitForSeconds(RandomNumber(3, 6));
        gracePeriod = false;
    }
}
