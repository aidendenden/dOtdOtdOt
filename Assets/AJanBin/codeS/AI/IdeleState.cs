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

        if (parameter.Hp <= 0)
        {
            manager.TranitionState(StateType.Die);
        }

        if (timer >= parameter.idleTime)
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

        if (parameter.Hp <= 0)
        {
            manager.TranitionState(StateType.Die);
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

public class DieState : IState
{

    private FSM manager;

    private Parameter parameter;

    public DieState(FSM manger)
    {
        this.manager = manger;
        this.parameter = manger.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("DiRenDie");
        parameter.collider.enabled = false;
    }

    public void OnUpdate()
    {
        parameter.info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (parameter.info.normalizedTime >= 1)
        {
            GameObject.Destroy(parameter.Self);
        }
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