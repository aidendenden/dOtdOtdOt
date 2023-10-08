using UnityEngine;

public class LaunchOnActivate : MonoBehaviour
{
    public float launchSpeed = 10f;
    public float delay = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }

    private void OnEnable()
    {
        Vector2 launchDirection = Random.insideUnitCircle.normalized;
        rb.velocity = launchDirection * launchSpeed;
    }
   

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}