using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAnser : MonoBehaviour
{

    public Animator animator;

    private MangeManger mangeManger;

    void Start()
    {
        mangeManger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mangeManger.numberOne == 1 && mangeManger.numberTwo == 1)
        {
            animator.SetInteger("Mode", 11);
        }

        if (mangeManger.numberOne == 1 && mangeManger.numberTwo == 2 || mangeManger.numberOne == 2 && mangeManger.numberTwo == 1)
        {
            animator.SetInteger("Mode", 12);
        }

        if (mangeManger.numberOne == 1 && mangeManger.numberTwo == 3 || mangeManger.numberOne == 3 && mangeManger.numberTwo == 1)
        {
            animator.SetInteger("Mode", 13);
        }

        if (mangeManger.numberOne == 2 && mangeManger.numberTwo == 2)
        {
            animator.SetInteger("Mode", 22);
        }

        if (mangeManger.numberOne == 2 && mangeManger.numberTwo == 3 || mangeManger.numberOne == 3 && mangeManger.numberTwo == 2)
        {
            animator.SetInteger("Mode", 23);
        }

        if (mangeManger.numberOne == 3 && mangeManger.numberTwo == 3)
        {
            animator.SetInteger("Mode", 33);
        }
    }

    
}