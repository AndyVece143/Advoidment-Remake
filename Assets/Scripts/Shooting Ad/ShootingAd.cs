using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingAd : Advertisement
{
    public GameObject player;
    private Vector3 scale;
    private bool isAdOver = false;
    private bool facingLeft = false;
    public float speed;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject laser;
    private List<GameObject> laserList;
    public GameObject enemy;
    public List<GameObject> enemies;
    private bool canShoot = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = scale;
        laser.GetComponent<SpriteRenderer>().enabled = false;
        laserList = new List<GameObject>();
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

        if (laserList.Count > 0)
        {
            LaserMovement();
            EnemyCollision();
        }

        if (movingAd)
        {
            MoveAd();
        }
    }

    /// <summary>
    /// Moves the player left and right. Also can shoot lasers
    /// </summary>
    private void PlayerMovement()
    {
        if (facingLeft)
        {
            player.transform.position += new Vector3(-speed * scale.x, 0, 0);
        }

        if (facingLeft == false)
        {
            player.transform.position += new Vector3(speed * scale.x, 0, 0);
        }

        if (player.GetComponent<Collider2D>().bounds.Intersects(leftWall.GetComponent<Collider2D>().bounds))
        {
            facingLeft = false;
        }

        if (player.GetComponent<Collider2D>().bounds.Intersects(rightWall.GetComponent<Collider2D>().bounds))
        {
            facingLeft = true;
        }
        if (Input.GetMouseButtonDown(0) && canShoot == true)
        {
            SpawnLaser();
            StartCoroutine(waiterShoot());
        }
    }

    /// <summary>
    /// Spawns the laser object
    /// </summary>
    private void SpawnLaser()
    {
        GameObject newLaser = Instantiate(laser);
        newLaser.transform.localScale = Vector3.Scale(scale, laser.transform.localScale);
        newLaser.transform.parent = transform;
        newLaser.transform.localPosition = player.transform.localPosition;
        newLaser.GetComponent<SpriteRenderer>().enabled = true;
        laserList.Add(newLaser);
    }

    /// <summary>
    /// Moves the laser in a straight line
    /// </summary>
    private void LaserMovement()
    {
        for (int i = 0; i < laserList.Count; i++)
        {
            laserList[i].transform.position += new Vector3(0, speed * scale.y, 0);

            if (laserList[i].transform.localPosition.y >= 6)
            {
                Destroy(laserList[i]);
                laserList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Checks if a laser touches an enemy, and destroys both
    /// </summary>
    private void EnemyCollision()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i])
            {
                if (laserList.Count > 0)
                {
                    for (int j = 0; j < laserList.Count; j++)
                    {
                        if (laserList[j])
                        {
                            if (enemies[i].GetComponent<Collider2D>().bounds.Intersects(laserList[j].GetComponent<Collider2D>().bounds))
                            {
                                Destroy(laserList[j]);
                                laserList.RemoveAt(j);

                                Destroy(enemies[i]);
                                enemies.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
        if (enemies.Count == 0)
        {
            StartCoroutine(waiter());
        }
    }
    public override void CreateAd()
    {
        Instantiate(gameObject);
    }
    protected override IEnumerator waiter()
    {
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

    /// <summary>
    /// Time it takes between shooting
    /// </summary>
    /// <returns>Shooting time</returns>
    private IEnumerator waiterShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.7f);
        canShoot = true;
    }

}
