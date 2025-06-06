using Unity.VisualScripting;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    public bool isDragging = false;
    public bool dragged;
    private Vector3 scale;
    private Vector3 offset;
    public bool done = false;
    public GameObject referencePoint;
    public Advertisement advertisement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragged = false;
    }

    // Update is called once per frame
    void Update()
    {
        scale = transform.localScale;
        if (isDragging)
        {
            dragged = true;
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }

        if (transform.localPosition.x >= 4)
        {
            transform.position = new Vector3(referencePoint.transform.position.x + (4 * advertisement.transform.localScale.x), transform.position.y, transform.position.z);
        }

        if (transform.localPosition.x <= -4)
        {
            transform.position = new Vector3(referencePoint.transform.position.x + (-4 * advertisement.transform.localScale.x), transform.position.y, transform.position.z);
        }

        if (transform.localPosition.y >= 4)
        {
            transform.position = new Vector3(transform.position.x, referencePoint.transform.position.y + (4 * advertisement.transform.localScale.y), transform.position.z);
        }

        if (transform.localPosition.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, referencePoint.transform.position.y + (-4 * advertisement.transform.localScale.y), transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicking");
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void OnMouseEnter()
    {
        Debug.Log("Hovering");
    }
}
