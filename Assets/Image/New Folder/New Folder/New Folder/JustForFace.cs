using UnityEngine;

public class JustForFace : MonoBehaviour
{

    public GameObject KeLi;
    public void DE()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "face")
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
            GameObject game = GameObject.Instantiate(KeLi, gameObject.transform.position, Quaternion.identity);
        }
    }
}