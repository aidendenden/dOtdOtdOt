using UnityEngine;

public class DragAndThrow : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Rigidbody2D rb;
    private Vector3 lastMousePosition;
    private Vector3 throwVelocity;

    public float throwForce = 10f;
    public float throwDamping = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        lastMousePosition = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(throwVelocity * throwForce, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            rb.MovePosition(new Vector2(newPosition.x, newPosition.y));

            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            throwVelocity = mouseDelta / Time.deltaTime;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            throwVelocity *= 1f - throwDamping * Time.deltaTime;
        }
    }
}
