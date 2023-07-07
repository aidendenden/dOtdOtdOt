using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCilck : MonoBehaviour
{
    public Animator _animator;
    public AudioSource _audioSource;


    public void DotCilcked()
    {
        _animator.SetTrigger("click");
        _audioSource.PlayOneShot(_audioSource.clip);
    }

}
