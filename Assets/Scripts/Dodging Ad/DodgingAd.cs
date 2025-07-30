using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class DodgingAd : Advertisement
{
    public GameObject player;
    private Vector3 scale;
    private bool isAdOver = false;
    public Vector3 originalPosition;
    public Vector3 playerOriginalPosition;
    public GameObject enemy;
    public List<GameObject> enemies;
    public int enemyNumber;
    private bool isMoving = true;
    private bool isDead = false;
    public GameObject winScreen;
    public int enemySpeed;
    public GameObject referencePoint;
    public GameObject instructions;
    public SpriteRenderer text;
    private Vector3 textPosition;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;
        originalPosition = player.transform.position;
        playerOriginalPosition = player.transform.localPosition;
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
        originalPosition = referencePoint.transform.position;

        if (moveText && beginAd == false)
        {
            MoveText();
        }

        if (beginAd)
        {
            Destroy(instructions);
            PlayerMovement();
            EnemyMovement();

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
    /// Moves the player
    /// </summary>
    void PlayerMovement()
    {
        if (isMoving)
        {
            if (enemies.Count > 0)
            {
                player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, player.transform.position.y, 0);
                player.transform.rotation = new Quaternion(0, 0, 0, 0);

                if (player.transform.localPosition.x >= 2.4f)
                {
                    player.transform.position = new Vector3(originalPosition.x + (2.4f * scale.x), player.transform.position.y, 0);
                }
                if (player.transform.localPosition.x <= -2.4f)
                {
                    player.transform.position = new Vector3(originalPosition.x + (-2.4f * scale.x), player.transform.position.y, 0);
                }
            }
            else
            {
                animator.SetBool("isWin", true);
            }

        }
        else
        {
            player.transform.position += new Vector3(4.0f, 10.0f, 0) * Time.deltaTime;
            player.transform.Rotate(Vector3.forward * (180 * Time.deltaTime));
          
        }

        if (isDead == false)
        {
            animator.SetBool("isHit", false);
        }

        else
        {
            animator.SetBool("isHit", true);
        }
    }

    private void SpawnEnemies()
    {
        isMoving = true;
        player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, (originalPosition.y - 3.2f) * scale.y ,0);

        enemies.Clear();
        Debug.Log("Spawning enemies!");
        float yPosition = 45 * scale.y;
        float[] points = { -1.9f * scale.x, 0, 1.9f * scale.x };

        enemies = new List<GameObject>(enemyNumber);

        for (int i = 0; i < enemyNumber; i ++)
        {
            GameObject newEnemy = Instantiate(enemy);
            newEnemy.transform.localScale = Vector3.Scale(scale, enemy.transform.localScale);
            newEnemy.transform.parent = transform;
            newEnemy.transform.localPosition = Vector3.zero;

            newEnemy.transform.position = new Vector2(
                transform.position.x + points[Random.Range(0, 3)], transform.position.y + yPosition);

            enemies.Add(newEnemy);
            yPosition -= (10f * scale.y);
        }

        isDead = false;

    }

    private void DeleteEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i]);
        }
        SpawnEnemies();
    }

    private void EnemyMovement()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0;i < enemies.Count;i++)
            {
                if (isMoving)
                {
                    enemies[i].transform.position += new Vector3(0, (enemySpeed * scale.y), 0) * Time.deltaTime;
                }

                if (enemies[i])
                {
                    if (player.GetComponent<Collider2D>().bounds.Intersects(enemies[i].GetComponent<Collider2D>().bounds) && isDead == false)
                    {
                        Debug.Log("OW!");
                        StartCoroutine(waiterDeath());
                    }
                    if (enemies[i].transform.localPosition.y <= -5.375f)
                    {
                        Destroy(enemies[i]);
                        enemies.RemoveAt(i);
                    }
                }
                
            }
        }

        if (enemies.Count == 0 && isDead == false)
        {
            StartCoroutine(waiter());
        }
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

    public override void CreateAd()
    {
        Instantiate(gameObject);
    }
    private IEnumerator waitbegin()
    {
        yield return new WaitForSeconds(1.0f);
        moveText = true;
        yield return new WaitForSeconds(textTime);
        beginAd = true;
        SpawnEnemies();
    }

    protected override IEnumerator waiter()
    {
        winScreen.GetComponent<SpriteRenderer>().enabled = true;
        scalingAd = false;
        movingAd = false;
        yield return new WaitForSeconds(1);
        isAdOver = true;

        yield return new WaitForSeconds(0.15f);

        Debug.Log("Bye");
        Destroy(gameObject);
    }
    protected override IEnumerator waiterDeath()
    {
        isDead = true;
        isMoving = false;
        yield return new WaitForSeconds(1);
        Debug.Log("Restarting!");
        DeleteEnemies();
    }
}
