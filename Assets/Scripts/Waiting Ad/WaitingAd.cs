using UnityEngine;
using System.Collections;

public class WaitingAd : Advertisement
{
    public GameObject closeButton;
    SpriteRenderer sprender;
    private bool isAdOver;
    private Vector3 scale;

    public GameObject instructions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprender = closeButton.GetComponent<SpriteRenderer>();
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;

        StartCoroutine(waitbegin());
    }

    // Update is called once per frame
    void Update()
    {
        //Scale up at the beginning
        if (transform.localScale.x <= 1 && isAdOver == false && beginScale == false)
        {
            ChangeScale(true);
        }
        //Scale down at the end
        if (transform.localScale.x >= 0.1f && isAdOver == true)
        {
            ChangeScale(false);
        }
        scale = transform.localScale;

        if (beginAd)
        {
            Destroy(instructions);
            if (closeButton.GetComponent<CloseButton>().isClicked == true)
            {
                StartCoroutine(waiter());
            }

            if (movingAd)
            {
                MoveAd();
            }

            if (scalingAd)
            {
                ScaleAd();
            }
        }
    }

    private void SpawnButton()
    {
        float[] pointsX = { -2.5f * scale.x, 2.5f * scale.x };
        float[] pointsY = { -4f * scale.y, 4f * scale.y };

        Vector3 originalPosition = closeButton.transform.position;
        closeButton.transform.position = new Vector3(originalPosition.x + pointsX[Random.Range(0, 2)], originalPosition.y + pointsY[Random.Range(0, 2)], 0);
    }

    public override void CreateAd()
    {
        Instantiate(gameObject);
    }

    private IEnumerator waitbegin()
    {
        yield return new WaitForSeconds(1.5f);
        beginAd = true;
        SpawnButton();
    }

    protected override IEnumerator waiter()
    {
        scalingAd = false;
        movingAd = false;
        yield return new WaitForSeconds(0.1f);
        isAdOver = true;

        yield return new WaitForSeconds(0.15f);

        Debug.Log("Bye");
        Destroy(gameObject);
    }

    protected override IEnumerator waiterDeath()
    {
        throw new System.NotImplementedException();
    }
}
