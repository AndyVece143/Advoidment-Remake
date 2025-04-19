using UnityEngine;
using System.Collections;

public class DodgingAd : Advertisement
{
    public GameObject player;
    private Vector3 scale;
    private bool isAdOver = false;
    private Vector3 originalPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;
        originalPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Scale up at the beginning
        if (transform.localScale.x <= 1 && isAdOver == false)
        {
            ChangeScale(true);
        }
        //Scale down at the end
        if (transform.localScale.x >= 0.1f && isAdOver == true)
        {
            ChangeScale(false);
        }

        scale = transform.localScale;

        PlayerMovement();
    }

    /// <summary>
    /// Moves the player
    /// </summary>
    void PlayerMovement()
    {
        player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, player.transform.position.y, 0);

        if (player.transform.localPosition.x >= 2.5f)
        {
            player.transform.position = new Vector3(originalPosition.x + (2.5f * scale.x), player.transform.position.y, 0);
        }
        if (player.transform.localPosition.x <= -2.5f)
        {
            player.transform.position = new Vector3(originalPosition.x + (-2.5f * scale.x), player.transform.position.y, 0);
        }
    }

    public override void CreateAd()
    {
        Instantiate(gameObject);
    }
    protected override IEnumerator waiter()
    {
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
