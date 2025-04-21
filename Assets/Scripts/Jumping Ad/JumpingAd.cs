using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpingAd : Advertisement
{
    public GameObject player;
    public GameObject ground;

    //Player variables
    public bool isGrounded;
    private float jumpForce;
    private float gravity;
    private float velocity;
    private bool canJump;

    //Enemy variables
    public GameObject enemy;
    public List<GameObject> enemies;
    public int enemyNumber;
    private float yValue;

    private bool isDead = false;
    private bool isMoving = true;
    private bool isAdOver = false;
    private Vector3 scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scale = new Vector3(0.1f, 0.1f, 0.1f);
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

        PlayerMovement();
    }
    private void PlayerMovement()
    {

    }
    public override void CreateAd()
    {
        Instantiate(gameObject);
        gameObject.SetActive(true);
    }

    protected override IEnumerator waiter()
    {
        yield return new WaitForSeconds(1);

        yield return new WaitForSeconds(0.15f);
    }
    protected override IEnumerator waiterDeath()
    {
        isDead = true;
        isMoving = false;
        yield return new WaitForSeconds(1);
    }
}
