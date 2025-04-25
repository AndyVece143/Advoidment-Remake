using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpingAd : Advertisement
{
    public GameObject player;
    public GameObject ground;

    //Player variables
    private float jumpForce;
    private float gravity;
    private float velocity;
    public bool canJump;

    //Enemy variables
    public GameObject enemy;
    public List<GameObject> enemies;
    public int enemyNumber;
    public int enemySpeed;

    private bool isDead = false;
    private bool isMoving = true;
    private bool isAdOver = false;
    private Vector3 scale;

    public GameObject winScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;
        gravity = -9.8f * scale.y;
        jumpForce = 12f * (scale.y / 2.0f);
        winScreen.GetComponent<SpriteRenderer>().enabled = false;

        SpawnEnemies();
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
        gravity = -9.8f * scale.y;
        jumpForce = 12f * (scale.y / 2.0f);

        PlayerMovement();
        EnemyMovement();

        if (movingAd)
        {
            MoveAd();
        }
    }
    private void PlayerMovement()
    {
        //Ground Collision
        if (player.GetComponent<Collider2D>().bounds.Intersects(ground.GetComponent<Collider2D>().bounds) && velocity <= 0)
        {
            player.transform.position = new Vector2(player.transform.position.x, ground.transform.position.y + 1f * scale.y);
            velocity = 0;
            canJump = true;
        }

        if (canJump == false)
        {
            velocity += gravity * Time.deltaTime;
        }


        if (Input.GetMouseButtonDown(0) && canJump && isMoving)
        {
            velocity = jumpForce;
            canJump = false;
        }

        player.transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);

        if (player.transform.localPosition.y >= 2)
        {
            velocity = 0;
        }
    }
    public override void CreateAd()
    {
        Instantiate(gameObject);
        gameObject.SetActive(true);
    }

    private void SpawnEnemies()
    {
        enemies.Clear();
        float xPosition = 5f * scale.x;
        float yPosition = ground.transform.position.y + 1 * scale.y;

        enemies = new List<GameObject>(enemyNumber);

        for (int i = 0; i < enemyNumber; i++)
        {
            GameObject newEnemy = Instantiate(enemy);
            newEnemy.transform.localScale = Vector3.Scale(scale, enemy.transform.localScale);
            newEnemy.transform.parent = transform;
            newEnemy.transform.localPosition = Vector3.zero;

            newEnemy.transform.position = new Vector2(
                transform.position.x + xPosition, yPosition);

            enemies.Add(newEnemy);
            xPosition += (10f * scale.x);
        }

        isDead = false;
        isMoving = true;
    }

    private void EnemyMovement()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (isMoving)
                {
                    enemies[i].transform.position += new Vector3((enemySpeed * scale.x) * Time.deltaTime, 0, 0);
                }

                if (enemies[i])
                {
                    if (player.GetComponent<Collider2D>().bounds.Intersects(enemies[i].GetComponent<Collider2D>().bounds) && isDead == false)
                    {
                        Debug.Log("OW!");
                        StartCoroutine(waiterDeath());
                    }

                    if (enemies[i].transform.localPosition.x <= -5f)
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

    private void DeleteEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i]);
        }
        SpawnEnemies();
    }

    protected override IEnumerator waiter()
    {
        winScreen.GetComponent<SpriteRenderer>().enabled = true;
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
