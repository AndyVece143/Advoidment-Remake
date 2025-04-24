using Unity.VisualScripting;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    public bool isDragging = false;
    public bool dragged;
    private Vector3 scale;
    private Vector3 offset;
    public bool done = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragged = false;
    }

    // Update is called once per frame
    void Update()
    {
        scale = transform.localScale;
        if (isDragging && done == false)
        {
            dragged = true;
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }

        if (transform.localPosition.x >= 4)
        {
            transform.position = new Vector3((4 * scale.x), transform.position.y, transform.position.z);
        }

        if (transform.localPosition.x <= -4)
        {
            transform.position = new Vector3((-4 * scale.x), transform.position.y, transform.position.z);
        }

        if (transform.localPosition.y >= 4)
        {
            transform.position = new Vector3(transform.position.x, (4 * scale.y), transform.position.z);
        }

        if (transform.localPosition.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, (-4 * scale.y), transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
