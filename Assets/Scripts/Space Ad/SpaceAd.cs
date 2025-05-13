using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

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
    public float speed;

    public bool isDead = false;
    public GameObject instructions;
    public SpriteRenderer text;
    private Vector3 textPosition;

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
            PlayerMovement();
            if (isDead == false)
            {
                AsteroidMovement();
            }

            LaserMovement();

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
    /// Rotates the player in the direction of the mouse and can shoot in that direction
    /// </summary>
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

    /// <summary>
    /// Spawns the laser object
    /// </summary>
    /// <param name="direction">The direction the player is facing</param>
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

    /// <summary>
    /// Allows the lasers to move in a straight line, and destroys them off screen
    /// </summary>
    private void LaserMovement()
    {
        if (isDead == false)
        {
            for (int i = 0; i < laserList.Count; i++)
            {
                laserList[i].transform.position += (laserList[i].transform.up * (speed * scale.x)) * Time.deltaTime;

                if (laserList[i].transform.localPosition.x >= 6 || laserList[i].transform.localPosition.x <= -6 || laserList[i].transform.localPosition.y >= 6 || laserList[i].transform.localPosition.y <= -6)
                {
                    Destroy(laserList[i]);
                    laserList.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// Spawns a certain number of asteroids
    /// </summary>
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

            List<float> coordinates = GetCoordinates(scale);

            newAsteroid.transform.position = new Vector2(
                transform.position.x + coordinates[0], transform.position.y + coordinates[1]);

            Vector3 playerDirection =  player.transform.position - newAsteroid.transform.position;

            newAsteroid.transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards: playerDirection);

            asteroids.Add(newAsteroid);
        }
        
        isDead = false;
        ColliderMethod(true);
    }

    /// <summary>
    /// Gets x and y values, and returns them in a list of floats
    /// </summary>
    /// <param name="scale">The current scale of the game ad</param>
    /// <returns>A list of 2 floats</returns>
    private List<float> GetCoordinates(Vector3 scale)
    {
        float xValue = Random.Range(-4.0f * scale.x, 4.0f * scale.x);
        float yValue =  Random.Range(-4.0f * scale.y, 4.0f * scale.y);

        if (xValue > -1.5f * scale.x && xValue < 1.5f * scale.x && yValue > -1.5f * scale.y && yValue < 1.5f * scale.y)
        {
            return GetCoordinates(scale);
        }

        else
        {
            List<float> coordinates = new List<float>();

            coordinates.Add(xValue);
            coordinates.Add(yValue);

            return coordinates;
        }
    }

    /// <summary>
    /// Asteroids rotate towards the player, and then slowly move towards it. Asteroid/laser collision is here
    /// </summary>
    private void AsteroidMovement()
    {
        if (asteroids.Count > 0)
        {
            for (int i = 0; i < asteroids.Count; i++)
            {
                if (isDead == false)
                {
                    asteroids[i].transform.position += (asteroids[i].transform.up * (speed/10 * scale.x)) * Time.deltaTime;
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

    /// <summary>
    /// Enables and disables colliders of lasers and asteroids
    /// </summary>
    /// <param name="enable">Determines if we are enabling or disabling colliders</param>
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

    /// <summary>
    /// Deletes all lasers and asteroids, and begins to spawn more
    /// </summary>
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
        SpawnAsteroids();
    }

    protected override IEnumerator waiter()
    {
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
        DeleteEverything();
    }

}
