using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class DodgingAd : Advertisement
{
    public GameObject player;
    private Vector3 scale;
    private bool isAdOver = false;
    private Vector3 originalPosition;
    public GameObject enemy;
    public List<GameObject> enemies;
    public int enemyNumber;
    private bool isMoving = true;
    private bool isDead = false;
    public GameObject winScreen;
    public int enemySpeed;
    public GameObject referencePoint;
    public GameObject instructions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;
        originalPosition = player.transform.position;
        winScreen.GetComponent<SpriteRenderer>().enabled = false;

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
    }

    private void SpawnEnemies()
    {
        enemies.Clear();
        Debug.Log("Spawning enemies!");
        float yPosition = 30 * scale.y;
        float[] points = { -1.25f * scale.x, 0, 1.25f * scale.x };

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
            yPosition -= (10F * scale.y);
        }

        isDead = false;
        isMoving = true;
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
                    enemies[i].transform.position += new Vector3(0, (enemySpeed * scale.y) * Time.deltaTime, 0);
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

    public override void CreateAd()
    {
        Instantiate(gameObject);
    }
    private IEnumerator waitbegin()
    {
        yield return new WaitForSeconds(1.5f);
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
