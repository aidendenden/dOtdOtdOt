using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TouZiStateType
{
    Idle,Dieing
}

[Serializable]
public class TouZiParameter
{
    public bool Life = true;
    public float ReBackTop = 10;
    public float ReBack = 0;
    public float ReBackDown = 5;

    public float moveSpeed;

    public float idleTime;

    public Transform[] patrolPoints;
    public Animator animator;
    public Rigidbody rigidbody;
    public Collider collider;
    public int Hp = 3;
    public AudioSource ReBackAudio;
    public AudioSource DieAudio;
}

public class FSMTouZi : MonoBehaviour
{

    public TouZiParameter parameter;

    private IState currenState;

    private Dictionary<TouZiStateType, IState> states = new Dictionary<TouZiStateType, IState>();


    void Start()
    {
        states.Add(TouZiStateType.Idle, new TouZiIdleState(this));
        states.Add(TouZiStateType.Dieing, new TouZiDieingState(this));


        TranitionState(TouZiStateType.Idle);

        parameter.animator = GetComponent<Animator>();
    }

   
    void Update()
    {
        currenState.OnUpdate();
    }


    public void TranitionState(TouZiStateType type)
    {
        if(currenState! != null)
        {
            currenState.OnExit();
        }
        currenState = states[type];
        currenState.OnEnter();
    }

    public void Flipto(Transform target)
    {
        if (target != null)
        {
            if(transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currenState== states[TouZiStateType.Dieing]&&other.tag=="Hand")
        {
            parameter.ReBack++;
        }
    }
}
