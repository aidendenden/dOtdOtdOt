using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCilck : MonoBehaviour
{
    public Animator _animator;
    public AudioSource _audioSource;

    public TypingSpeedCalculator _typingSpeedCalculator;

    public float DianJiJinDu = 1;
    public Transform _transform;

    public AudioClip[] acs;


    public void DotCilcked()
    {
        _animator.SetTrigger("click");
        int a = Random.Range(0, 4);
        float b = Random.Range(0.75f, 1f);
        _audioSource.PlayOneShot(acs[a], b);
    }

    public void DotShuned()
    {
        _animator.SetTrigger("Shun");
        //_audioSource.PlayOneShot(_audioSource.clip);
    }

    private void Update()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1))
        {
            DotCilcked();
        }
    }

    public void HuaDongDotL()
    {
        DotShuned();
    }

    public void HuaDongDotR()
    {
        DotShuned();
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