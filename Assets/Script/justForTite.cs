using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class justForTite : MonoBehaviour
{
    public bool a = false;
    public Animator animator;//�������ã�

    private void Update()
    {
        if (a)
        {
            a = false;

            animator.SetTrigger("Change");
            

        }
    }
}
