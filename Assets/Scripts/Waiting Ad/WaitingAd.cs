using UnityEngine;
using System.Collections;

public class WaitingAd : Advertisement
{
    public GameObject closeButton;
    SpriteRenderer sprender;
    private bool isAdOver;
    private Vector3 scale;

    public GameObject instructions;
    public SpriteRenderer text;
    private Vector3 textPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprender = closeButton.GetComponent<SpriteRenderer>();
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;
        textPosition = text.transform.localPosition;
        DisplaceText();

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

        if (moveText && !beginAd)
        {
            MoveText();
        }

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

    private void DisplaceText()
    {
        int i = Random.Range(0, 3);

        switch (i)
        {
            case 0:
                text.transform.position += new Vector3(0.6f, 0, 0);
                break;
            case 1:
                text.transform.position += new Vector3(-0.6f, 0, 0);
                break;
            case 2:
                text.transform.position += new Vector3(0, 0.2f, 0);
                break;
        }
    }
    private void MoveText()
    {
        if (text.transform.position.x > textPosition.x)
        {
            text.transform.position += new Vector3(-10f, 0, 0) * Time.deltaTime;
        }

        if (text.transform.position.x < textPosition.x)
        {
            text.transform.position += new Vector3(10f, 0, 0) * Time.deltaTime;
        }

        if (text.transform.localPosition.y > textPosition.y)
        {
            text.transform.position += new Vector3(0, -5f, 0) * Time.deltaTime;
        }
    }

    private IEnumerator waitbegin()
    {
        yield return new WaitForSeconds(1.0f);
        moveText = true;
        yield return new WaitForSeconds(textTime);
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
