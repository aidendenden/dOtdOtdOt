using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeleState :IState
{

    private FSM manager;

    private Parameter parameter;

    private float timer;

    public IdeleState(FSM manger)
    {
        this.manager = manger;
        this.parameter = manger.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if(timer >= parameter.idleTime)
        {
            manager.TranitionState(StateType.Patrol);
        }
    }

    public void OnExit()
    {

        timer = 0; 
    }
}

public class PatroState : IState
{

    private FSM manager;

    private Parameter parameter;

    private int patrolPostion;
    public PatroState(FSM manger)
    {
        this.manager = manger;
        this.parameter = manger.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("Walk");
    }

    public void OnUpdate()
    {
        manager.Flipto(parameter.patrolPoints[patrolPostion]);

        manager.transform.position = Vector3.MoveTowards(manager.transform.position, parameter.patrolPoints[patrolPostion].position, parameter.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(manager.transform.position, parameter.patrolPoints[patrolPostion].position) < 2f)
        {
            manager.TranitionState(StateType.Idle);
        }
    }

    public void OnExit()
    {
        patrolPostion++;

        if( patrolPostion >= parameter.patrolPoints.Length)
        {
            patrolPostion = 0;
        }

    }
}

public class ChaseState : IState
{

    private FSM manager;

    private Parameter parameter;

    public ChaseState(FSM manger)
    {
        this.manager = manger;
        this.parameter = manger.parameter;
    }
    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {


    }
}

public class ReactState : IState
{

    private FSM manager;

    private Parameter parameter;

    public ReactState(FSM manger)
    {
        this.manager = manger;
        this.parameter = manger.parameter;
    }
    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {


    }
}
public class AttackState : IState
{

    private FSM manager;

    private Parameter parameter;

    public AttackState(FSM manger)
    {
        this.manager = manger;
        this.parameter = manger.parameter;
    }
    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {


    }
}