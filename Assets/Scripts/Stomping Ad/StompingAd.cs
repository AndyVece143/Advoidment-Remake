using System.Collections;
using UnityEngine;

public class StompingAd : Advertisement
{
    public GameObject player;
    private Vector3 scale;
    private bool isAdOver = false;
    private bool facingLeft = true;
    public float speed;
    private bool goingDown = false;
    private bool goingUp = false;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject floor;

    //Enemy variables
    public GameObject enemy;
    private bool enemyFacingLeft = false;
    private bool enemyGoingUp = false;
    private bool enemyGoingDown = false;
    private bool enemyJump = false;

    public GameObject instructions;
    public SpriteRenderer text;
    private Vector3 textPosition;

    public GameObject winScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;
        textPosition = text.transform.localPosition;
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
            PlayerMovement();

            if (enemy)
            {
                EnemyMovement();
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

    private void PlayerMovement()
    {
        if (facingLeft && goingDown == false && goingUp == false)
        {
            player.transform.position += new Vector3(-speed * scale.x, 0, 0) * Time.deltaTime;
        }
        if (facingLeft == false && goingDown == false && goingUp == false)
        {
            player.transform.position += new Vector3(speed * scale.x, 0, 0) * Time.deltaTime;
        }

        if (goingDown)
        {
            player.transform.position += new Vector3(0, -speed * scale.y, 0) * Time.deltaTime;
            if (player.GetComponent<Collider2D>().bounds.Intersects(floor.GetComponent<Collider2D>().bounds))
            {
                goingDown = false;
                goingUp = true;
            }
        }

        if (goingUp)
        {
            player.transform.position += new Vector3(0, speed * scale.y, 0) * Time.deltaTime;

            if (player.transform.localPosition.y >= 5.5f)
            {
                goingUp = false;
            }
        }

        if (player.GetComponent<Collider2D>().bounds.Intersects(leftWall.GetComponent<Collider2D>().bounds))
        {
            facingLeft = false;
        }

        if (player.GetComponent<Collider2D>().bounds.Intersects(rightWall.GetComponent<Collider2D>().bounds))
        {
            facingLeft = true;
        }

        if (Input.GetMouseButtonDown(0) && goingDown == false)
        {
            goingDown = true;
        }
    }

    private void EnemyMovement()
    {
        if (enemyFacingLeft == true && goingDown == false && goingUp == false)
        {
            enemy.transform.position += new Vector3(-speed * 2 * scale.x, 0, 0) * Time.deltaTime;
        }
        if (enemyFacingLeft == false && goingDown == false && goingUp == false)
        {
            enemy.transform.position += new Vector3(speed * 2 * scale.x, 0, 0) * Time.deltaTime;
        }

        if (enemy.GetComponent<Collider2D>().bounds.Intersects(leftWall.GetComponent<Collider2D>().bounds))
        {
            enemyFacingLeft = false;
        }

        if (enemy.GetComponent<Collider2D>().bounds.Intersects(rightWall.GetComponent<Collider2D>().bounds))
        {
            enemyFacingLeft = true;
        }

        if (goingDown)
        {
            enemy.transform.position += new Vector3(0, 0, 0);
        }

        if (goingUp && enemyJump == false)
        {
            enemyGoingUp = true;
            enemyJump = true;
        }

        if (enemyGoingUp)
        {
            enemy.transform.position += new Vector3(0, speed * scale.y, 0) * Time.deltaTime;
            if (enemy.transform.localPosition.y >= -1 * scale.y)
            {
                enemyGoingUp = false;
                enemyGoingDown = true;
            }
        }

        if (enemyGoingDown)
        {
            enemy.transform.position += new Vector3(0, -speed * scale.y, 0) * Time.deltaTime;

            if (enemy.GetComponent<Collider2D>().bounds.Intersects(floor.GetComponent<Collider2D>().bounds))
            {
                enemy.transform.position = new Vector3(enemy.transform.position.x, floor.transform.position.y + 0.70f * scale.y, 0);
                enemyGoingDown = false;

            }
        }

        if (goingUp == false && goingDown == false)
        {
            enemyJump = false;
        }

        if (enemy.GetComponent<Collider2D>().bounds.Intersects(player.GetComponent<Collider2D>().bounds))
        {
            Destroy(enemy);
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
            text.transform.position += new Vector3(0, -10f, 0) * Time.deltaTime;
        }
    }

    private IEnumerator waitbegin()
    {
        yield return new WaitForSeconds(1.0f);
        moveText = true;
        yield return new WaitForSeconds(textTime);
        beginAd = true;
    }
    protected override IEnumerator waiter()
    {
        movingAd = false;
        scalingAd = false;
        winScreen.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(1);
        isAdOver = true;

        yield return new WaitForSeconds(0.15f);

        Debug.Log("Bye");
        Destroy(gameObject);
    }
    protected override IEnumerator waiterDeath()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
