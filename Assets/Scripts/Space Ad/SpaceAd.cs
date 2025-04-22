using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceAd : Advertisement
{
    public GameObject player;
    public GameObject laser;
    private Vector3 scale;
    private bool isAdOver = false;
    private List<GameObject> laserList;
    public GameObject asteroid;
    public List<GameObject> asteroids;
    public int asteroidNumber;
    public GameObject winScreen;

    public bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
        asteroid.GetComponent<Collider2D>().enabled = false;
        laser.GetComponent<SpriteRenderer>().enabled = false;
        asteroid.GetComponent<SpriteRenderer>().enabled = false;
        winScreen.GetComponent<SpriteRenderer>().enabled = false;
        transform.localScale = scale;
        laserList = new List<GameObject>();

        SpawnAsteroids();
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
        if (isDead == false)
        {
            AsteroidMovement();
        }

        LaserMovement();
    }

    private void PlayerMovement()
    {
        if (isDead == false)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 direction = new Vector2(
                mousePosition.x - transform.position.x,
                mousePosition.y - transform.position.y);

            player.transform.up = direction;

            if (Input.GetMouseButtonDown(0))
            {
                SpawnLaser(direction);
            }
        }
    }

    private void SpawnLaser(Vector3 direction)
    {
        GameObject newLaser = Instantiate(laser);
        newLaser.transform.localScale = Vector3.Scale(scale, laser.transform.localScale);
        newLaser.transform.parent = transform;
        newLaser.transform.localPosition = Vector3.zero;
        newLaser.transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards: direction);
        newLaser.GetComponent<SpriteRenderer>().enabled = true;
        laserList.Add(newLaser);
    }

    private void LaserMovement()
    {
        if (isDead == false)
        {
            for (int i = 0; i < laserList.Count; i++)
            {
                laserList[i].transform.position += laserList[i].transform.up * (0.01f * scale.x);

                if (laserList[i].transform.localPosition.x >= 6 || laserList[i].transform.localPosition.x <= -6 || laserList[i].transform.localPosition.y >= 6 || laserList[i].transform.localPosition.y <= -6)
                {
                    Destroy(laserList[i]);
                    laserList.RemoveAt(i);
                }
            }
        }
    }

    private void SpawnAsteroids()
    {
        asteroids.Clear();
        Debug.Log("New droids");


        asteroids = new List<GameObject>(asteroidNumber);

        for (int i = 0; i < asteroidNumber; i++)
        {
            GameObject newAsteroid = Instantiate(asteroid);
            newAsteroid.transform.localScale = Vector3.Scale(scale, asteroid.transform.localScale);
            newAsteroid.transform.parent = transform;
            newAsteroid.transform.localPosition = Vector3.zero;
            newAsteroid.GetComponent<SpriteRenderer>().enabled = true;

            float xValue = Random.Range(-4.0f * scale.x, 4.0f * scale.y);
            float yValue = Random.Range(-4.0f * scale.x, 4.0f * scale.y);

            newAsteroid.transform.position = new Vector2(
                transform.position.x + xValue, transform.position.y + yValue);

            Vector3 playerDirection =  player.transform.position - newAsteroid.transform.position;

            newAsteroid.transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards: playerDirection);

            asteroids.Add(newAsteroid);
        }
        
        isDead = false;
        ColliderMethod(true);
    }

    private void AsteroidMovement()
    {
        if (asteroids.Count > 0)
        {
            for (int i = 0; i < asteroids.Count; i++)
            {
                if (isDead == false)
                {
                    asteroids[i].transform.position += asteroids[i].transform.up * (0.001f * scale.x);
                }

                if (asteroids[i] && asteroids[i].GetComponent<Collider2D>().enabled == true)
                {
                    if (player.GetComponent<Collider2D>().bounds.Intersects(asteroids[i].GetComponent<Collider2D>().bounds) && isDead == false)
                    {
                        ColliderMethod(false);
                        Debug.Log("hit");
                        StartCoroutine(waiterDeath());
                    }
                    if (laserList.Count > 0)
                    {
                        for (int j = 0; j < laserList.Count; j++)
                        {
                            if (asteroids[i].GetComponent<Collider2D>().bounds.Intersects(laserList[j].GetComponent<Collider2D>().bounds))
                            {
                                Destroy(laserList[j]);
                                laserList.RemoveAt(j);

                                Destroy(asteroids[i]);
                                asteroids.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }

        if (asteroids.Count == 0 && isDead == false)
        {
            StartCoroutine(waiter());
        }
    }

    private void ColliderMethod(bool enable)
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            if (enable == true)
            {
                asteroids[i].GetComponent <Collider2D>().enabled = true;
            }
            if (enable == false)
            {
                asteroids[i].GetComponent<Collider2D>().enabled = false;
            }
        }

        for (int i = 0; i < laserList.Count; i++)
        {
            if (enable == true)
            {
                laserList[i].GetComponent<Collider2D>().enabled = true;
            }
            if (enable == false)
            {
                laserList[i].GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public override void CreateAd()
    {
        Instantiate(gameObject);
    }

    private void DeleteEverything()
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            Destroy(asteroids[i]);
        }

        for (int j = 0; j < laserList.Count; j++)
        {
            Destroy(laserList[j]);
        }
        laserList.Clear();
        laserList = new List<GameObject>();

        SpawnAsteroids();
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
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Restarting");
        DeleteEverything();
    }

}
