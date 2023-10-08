using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustForShouJi : MonoBehaviour
{
    public bool a = false;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (a)
        {
            a = false;
            animator.SetTrigger("Change");
        }
    }

  
}
