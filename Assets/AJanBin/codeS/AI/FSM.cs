using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,Patrol,Die, React,Attack,
}

[Serializable]
public class Parameter
{
    public GameObject Self;
    public int Hp;
    public float moveSpeed;

    public float idleTime;

    public Transform[] patrolPoints;
    public Animator animator;
    public AnimatorStateInfo info;
    public Collider collider;
}

public class FSM : MonoBehaviour
{

    public Parameter parameter;

    private IState currenState;

    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();


    void Start()
    {
        states.Add(StateType.Idle, new IdeleState(this));
        states.Add(StateType.Patrol, new PatroState(this));
        states.Add(StateType.Die, new DieState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));

        TranitionState(StateType.Idle);

        parameter.animator = GetComponent<Animator>();
    }

   
    void Update()
    {
        currenState.OnUpdate();
    }


    public void TranitionState(StateType type)
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



    public void TurnTO(Transform target)
    {
        // 计算目标物体在当前物体坐标系下的位置
        Vector3 targetPosition = transform.InverseTransformPoint(target.position);

        // 计算目标物体在当前物体坐标系下的旋转角度
        float angle = Mathf.Atan2(targetPosition.x, targetPosition.z) * Mathf.Rad2Deg;

        // 更新物体的旋转角度，只改变Y轴旋转
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
