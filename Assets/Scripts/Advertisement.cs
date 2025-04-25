using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public abstract class Advertisement : MonoBehaviour
{
    protected Vector3 scaleChange = new Vector3(0.01f, 0.01f, 0.01f);
    public float xSpeed;
    public float ySpeed;
    public List<GameObject> bounds;
    private bool TopBottomCheck = false;
    private bool LeftRightCheck = false;
    public bool movingAd;
    
    void Awake()
    {
        Debug.Log("WALLS");
        bounds = new List<GameObject>();
        bounds.Add(GameObject.Find("RightBounds"));
        bounds.Add(GameObject.Find("LeftBounds"));
        bounds.Add(GameObject.Find("TopBounds"));
        bounds.Add(GameObject.Find("BottomBounds"));
        xSpeed = 0.001f;
        ySpeed = 0.001f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hello");

    }

    public abstract void CreateAd();

    public void ChangeScale(bool rate)
    {
        if (rate == true)
        {
            transform.localScale += scaleChange;
        }
        else
        {
            transform.localScale -= scaleChange;
        }
    }

    public void MoveAd()
    {
        transform.position += new Vector3(xSpeed, ySpeed, 0);
        BoundsCollision();
    }

    private void BoundsCollision()
    {
        for (int i = 0; i < bounds.Count; i++)
        {
            if (gameObject.GetComponent<Collider2D>().bounds.Intersects(bounds[i].GetComponent<Collider2D>().bounds))
            {
                ChangeDirection(bounds[i]);
            }
        }
    }

    private void ChangeDirection(GameObject wall)
    {
        Debug.Log("Change Directions");
        if (wall.tag == "LeftRightBounds" && LeftRightCheck == false)
        {
            xSpeed *= -1;
            StartCoroutine(CollisionWait(wall.tag));
        }
        if (wall.tag == "TopBottomBounds" && TopBottomCheck == false)
        {
            ySpeed *= -1;
            StartCoroutine(CollisionWait(wall.tag));
        }
    }

    private IEnumerator CollisionWait(string tag)
    {
        if (tag == "LeftRightBounds")
        {
            LeftRightCheck = true;
            yield return new WaitForSeconds(0.1f);
            LeftRightCheck = false;
        }

        else
        {
            TopBottomCheck = true;
            yield return new WaitForSeconds(0.1f);
            TopBottomCheck = false;
        }
        
    }

    protected abstract IEnumerator waiter();
    protected abstract IEnumerator waiterDeath();
}
