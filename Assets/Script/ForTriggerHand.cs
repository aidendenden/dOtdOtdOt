using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTriggerHand : MonoBehaviour
{
    public Animator animator;
    public Animator animatorT;
    public AudioSource audioSource;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerHand"|| collision.gameObject.tag == "playerGalss"&&gameObject.tag=="enHand")
        {
            animatorT.SetTrigger("Jump");
            animator.SetTrigger("Change");
            audioSource.PlayOneShot(audioSource.clip);
            Debug.Log("aa");
        }
        Debug.Log(collision.gameObject.tag);
    }
}
