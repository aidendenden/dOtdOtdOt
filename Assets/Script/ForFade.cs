using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ForFade : MonoBehaviour
{
    public string toScence;


    public int StartMode = 0;

    public bool a = false;

    Animator animator;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        CheckStartMode();
    }

    private void Update()
    {
        if (a)
        {
            a = false;

            ChangeScence();
        }
    }


    public void ChangeScence()
    {
        if (toScence != null)
        {
            SceneManager.LoadScene(toScence);
        }
    }

    
    private void CheckStartMode()
    {
        if(StartMode == 1)
        {
            animator.SetTrigger("Out");
        }
        if (StartMode == 2)
        {
            animator.SetTrigger("In");
        }
        if (StartMode == 3)
        {
            animator.SetTrigger("Both");
        }
        if (StartMode == 4)
        {
            animator.SetTrigger("Change");
        }
        if (StartMode == 5)
        {
            animator.Play("omggg");
        }
    }
}
