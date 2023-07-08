using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCilck : MonoBehaviour
{
    public Animator _animator;
    public AudioSource _audioSource;

    public TypingSpeedCalculator _typingSpeedCalculator;

    public float DianJiJinDu = 0;
    public Transform _transform;
    


    public void DotCilcked()
    {
        _animator.SetTrigger("click");
        _audioSource.PlayOneShot(_audioSource.clip);
    }

    private void Update()
    {
        
            dotdotcheck();
        
    }

    public void HuaDongDotL()
    {
        _transform.Rotate(0,0,5);
        
        
    }

    public void HuaDongDotR()
    {
        _transform.Rotate(0,0,-5);


    }

    public void dotdotcheck()
    {
       
        
            if (Input.GetKeyDown(KeyCode.G))
            {
                DotCilcked();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                DotCilcked();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                DotCilcked();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                DotCilcked();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                DotCilcked();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                DotCilcked();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                DotCilcked();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                DotCilcked();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                DotCilcked();
            }
        
    }
}
