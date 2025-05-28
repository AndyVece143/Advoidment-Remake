using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class FlashlightAd : Advertisement
{
    public GameObject player;
    public GameObject flashlight;
    public GameObject hiddenObject;
    private Vector3 scale;
    private bool isAdOver = false;
    public GameObject winScreen;
    private bool isMoving = true;

    public bool isDead = false;
    public GameObject instructions;
    public SpriteRenderer text;
    private Vector3 textPosition;
    public GameObject spriteMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        textPosition = text.transform.localPosition;
        transform.localScale = scale;
        winScreen.GetComponent<SpriteRenderer>().enabled = false;
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
            LightMovement();

            if (hiddenObject.GetComponent<Click>().isClicked == true)
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

    private void LightMovement()
    {
        if (isMoving)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            player.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            FlashlightMovement(mousePosition);
        }

    }

    private void FlashlightMovement(Vector3 mousePosition)
    {
        Vector2 direction = new Vector2(
            mousePosition.x - flashlight.transform.position.x,
            mousePosition.y - flashlight.transform.position.y);

        flashlight.transform.up = direction;
    }

    private void MoveHiddenObject()
    {
        hiddenObject.transform.position = new Vector3(Random.Range(-2.0f * scale.x, 2.0f * scale.x), Random.Range(-3.0f * scale.y, 4.0f * scale.y), 0);
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
        Destroy(spriteMask);
        MoveHiddenObject();
    }

    public override void CreateAd()
    {
        Instantiate(gameObject);
    }

    protected override IEnumerator waiter()
    {
        isMoving = false;
        winScreen.GetComponent<SpriteRenderer>().enabled = true;
        movingAd = false;
        scalingAd = false;
        yield return new WaitForSeconds(1);
        isAdOver = true;

        yield return new WaitForSeconds(0.15f);

        Debug.Log("Bye");
        Destroy(gameObject);
    }
    protected override IEnumerator waiterDeath()
    {
        isDead = true;
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Restarting");
    }
}
