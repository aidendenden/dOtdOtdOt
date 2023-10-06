using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class TouZiIdleState : IState
{

    private FSMTouZi manager;

    private TouZiParameter parameter;

    

    public TouZiIdleState(FSMTouZi manger)
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
       

        if(parameter.Hp <= 0)
        {
            manager.TranitionState(TouZiStateType.Dieing);
        }
    }

    public void OnExit()
    {

        
    }
}


public class TouZiDieingState : IState
{

    private FSMTouZi manager;

    private TouZiParameter parameter;

    private int patrolPostion;
    private Rigidbody rigidbody;
    public TouZiDieingState(FSMTouZi manger)
    {
        this.manager = manger;
        this.parameter = manger.parameter;
        this.rigidbody = parameter.rigidbody;
    }
    public void OnEnter()
    {
        parameter.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        parameter.Life = false;
        parameter.rigidbody.useGravity = false;
        parameter.collider.isTrigger = true;
        parameter.animator.Play("Dieing");
        parameter.DieAudio.PlayOneShot(parameter.DieAudio.clip);



    }

    public void OnUpdate()
    {
        if (parameter.ReBack >= parameter.ReBackTop)
        {
            manager.TranitionState(TouZiStateType.Idle);

        }

        if (parameter.ReBack > 0)
        {
            parameter.ReBack -= parameter.ReBackDown * Time.deltaTime;
        }
        
    }

    public void OnExit()
    {
        parameter.Hp = 3;
        parameter.ReBack = 0f;
        parameter.Life = true;
        parameter.rigidbody.useGravity = true;
        parameter.rigidbody.constraints = RigidbodyConstraints.None;
        parameter.collider.isTrigger = false;
        parameter.ReBackAudio.PlayOneShot(parameter.ReBackAudio.clip);
    }
}

