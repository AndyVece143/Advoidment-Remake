using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MatchingAd : Advertisement
{
    private Vector3 scale;
    private bool isAdOver = false;

    public List<GameObject> boxes;
    public List<GameObject> targets;

    private List<bool> complete;
    private bool isAdDone = false;

    public GameObject instructions;
    public SpriteRenderer text;
    private Vector3 textPosition;

    public GameObject winScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;

        complete = new List<bool> { false, false, false };
        winScreen.GetComponent<SpriteRenderer>().enabled = false;
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
            CheckCollision();
            CheckCompletion();

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

    /// <summary>
    /// Sets the location for the boxes and targets
    /// </summary>
    private void ChangeLocations()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            //boxes[i].transform.position = new Vector3(Random.Range(-4.0f * scale.x, 4.0f * scale.x), Random.Range(-4.0f * scale.y, 4.0f * scale.y), 0);
            targets[i].transform.position = new Vector3(Random.Range(-4.0f * scale.x, 4.0f * scale.x), Random.Range(-4.0f * scale.y, 4.0f * scale.y), 0);
        }
    }

    /// <summary>
    /// Checks the collisions between the boxes and their respective targets
    /// </summary>
    private void CheckCollision()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            if (boxes[i].GetComponent<Collider2D>().bounds.Intersects(targets[i].GetComponent<Collider2D>().bounds) && boxes[i].GetComponent<Dragging>().dragged)
            {
                if (boxes[i].GetComponent<Dragging>().isDragging == false)
                {
                    boxes[i].transform.position = targets[i].transform.position;
                    complete[i] = true;
                    boxes[i].GetComponent<Dragging>().done = true;
                }
            }
        }
    }

    /// <summary>
    /// Checks if all the boxes are in their targets
    /// </summary>
    private void CheckCompletion()
    {
        int done = 0;

        for (int i = 0; i < complete.Count; i++)
        {
            if (complete[i] == true)
            {
                done++;
            }
        }

        if (done == 3 && isAdDone == false)
        {
            StartCoroutine(waiter());
        }
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
                text.transform.position += new Vector3(0.8f, 0, 0);
                break;
            case 1:
                text.transform.position += new Vector3(-0.8f, 0, 0);
                break;
            case 2:
                text.transform.position += new Vector3(0, 0.4f, 0);
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
        ChangeLocations();
    }
    protected override IEnumerator waiter()
    {
        isAdDone = true;
        scalingAd = false;
        movingAd = false;
        winScreen.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(1);
        isAdOver = true;

        yield return new WaitForSeconds(0.15f);

        Debug.Log("Bye");
        Destroy(gameObject);
    }
    protected override IEnumerator waiterDeath()
    {
        yield return new WaitForSeconds(1);
    }
}
