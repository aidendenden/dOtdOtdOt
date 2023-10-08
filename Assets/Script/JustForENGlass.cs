using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustForENGlass : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    private bool _b;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerGalss")
        {
            animator.SetTrigger("Change");
            if (_b)
            {
                audioSource1.PlayOneShot(audioSource1.clip);
            }
            else
            {
                audioSource2.PlayOneShot(audioSource2.clip);
            }
            _b = !_b;
            //Debug.Log("aa");
        }
        Debug.Log(collision.gameObject.tag);
    }
}
