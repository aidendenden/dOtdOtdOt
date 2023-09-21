using UnityEngine;

public class CollisionSound : MonoBehaviour
{

    public string Tag;
    public AudioClip collisionSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = collisionSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            audioSource.Play();
        }
    }
}