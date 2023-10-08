using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 5f;
    public float speedX = 5f;

    public float maxY = 10f;

    public float maxX = 10f;

    public Transform _transform;//ÉÏÏÂ
    public Transform __transform;//×óÓÒ

    void Update()
    {
        

        if (transform.position.y >= maxY + _transform.position.y)
        {   
            transform.Translate(Vector3.up * -speed * Time.deltaTime);
        }
        else if (transform.position.y <= maxY + _transform.position.y)
        {
            transform.Translate(Vector3.up * speed*1.5f * Time.deltaTime);
        }


        if (transform.position.x >= maxX + __transform.position.x)
        {
            transform.Translate(Vector3.left * speedX * Time.deltaTime);
        }
        else if (transform.position.x <= maxX + __transform.position.x)
        {
            transform.Translate(Vector3.left * -speedX * Time.deltaTime);
        }
    }
}