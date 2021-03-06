﻿using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StayInPoseXtreme : AnimationBehaviour {

    private StayInPoseState stayInPoseState;
    private StayInPoseState _StayInPoseState
    {
        get
        {

            if (this.IsCentralNode)
                return stayInPoseState;
            else
                return CentralNode.stayInPoseState;
        }
        set
        {

            if (this.IsCentralNode)
                stayInPoseState = value;
            else
                CentralNode.stayInPoseState = value;

        }
    }

    protected override bool HasCentralNode { get { return true; } }
    private bool holdingPose = false;
    /// <summary>
    /// Esta variable se utiliza para comprobar si es primera vez que se entra al este behaviour
    /// </summary>
    [HideInInspector]
    private event EventHandler LerpRoundTripEnd;
    private bool hasEnteredBefore = false;

    private bool _isRewinding = false;
    private bool IsRewinding
    {
        get
        {

            if (this.IsCentralNode)
                return _isRewinding;
            else
                return CentralNode._isRewinding;
        }
        set
        {

            if (this.IsCentralNode)
                _isRewinding = value;
            else
                CentralNode.IsRewinding = value;

        }
    }

    public new StayInPoseXtreme CentralNode
    {
        get
        {
            if (_centralNode == null)
                _centralNode = AnimationBehaviour.GetCentralBehaviour(this.movement, this.limb);
            return (StayInPoseXtreme)_centralNode;
        }
    }
    /// <summary>
    /// Se utiliza para enviar datos a ExerciseDataGenerator en intervalos de tiempo determinados.
    /// El tiempo que ha pasado desde que se hizo la última captura de datos.
    /// </summary>
    private float timeSinceCapture = 0;
    protected void OnLerpRoundTripEnd()
    {

        if (!IsCentralNode)
        {
            this.CentralNode.OnLerpRoundTripEnd();
        }
        else
        {
            EventHandler eh = LerpRoundTripEnd;
            if (eh != null)
            {
                eh(this, new EventArgs());
            }
        }
    }

    void LerpBehaviour_LerpRoundTripEnd(object sender, EventArgs e)
    {
        if (_BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT)
        {
            this.CentralNode.endRepTime = DateTime.Now;

        }
    }

    public BehaviourParams GetParams()
    {
        return this._currentParams;
    }

    //
    //==== Controlar tiempos de repeticiones
    //


    public ClockBehaviour _clockBehaviour;
    public ClockBehaviour clockBehaviour
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._clockBehaviour;
            }
            else
            {
                return CentralNode._clockBehaviour;
            }

        }
        set
        {
            if (this.IsCentralNode)
            {
                this._clockBehaviour = value;
            }
            else
            {
                CentralNode._clockBehaviour = value;
            }
        }
    }

    public float secondsInPose
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._realParams.SecondsInPose;
            }
            else
            {
                return CentralNode._realParams.SecondsInPose;
            }

        }
    }



    //
    //==== FIN Controlar tiempos de repeticiones
    //


    //===== FIN RELOJ
    protected override AnimationBehaviourState _BehaviourState
    {
        get
        {

            if (this.IsCentralNode)
                return this._behaviourState;
            else
                return this.CentralNode._BehaviourState;
        }
        set
        {
            if (this.IsCentralNode)
                this._behaviourState = value;
            else
                this.CentralNode._BehaviourState = value;
            switch (value)
            {
                case AnimationBehaviourState.RUNNING_DEFAULT:

                case AnimationBehaviourState.RUNNING_WITH_PARAMS:
                    animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
                    break;
                case AnimationBehaviourState.PREPARING_WITH_PARAMS:
                    break;
            }
        }
    }

    private void executionTimerStart()
    {
        //Debug.Log("comienza ejecucion| HORA: " + DateTime.Now.ToString());
        //Debug.Log(" INICIO manteniendo pose pose ");
    }

    private void executionTimerFinish()
    {
 //       Debug.Log("termina ejecucion| HORA: " + DateTime.Now.ToString());
        //Debug.Log("FIN manteniendo pose pose ");
        this.CentralNode.holdingPose = true;
        clockBehaviour.stopExecutionTimer();
    }


    /**
    Cuando se acaba el tiempo de pausa entre repeticiones
    **/
    private void pauseBetweenRepetitionsFinish()
    {
        clockBehaviour.stopTimeBetweenRepetitionsTimer();
        //Debug.Log("termina pausa entre repeticiones| HORA: " + DateTime.Now.ToString());

    }

    private void pauseBetweenRepetitionsStart()
    {
        //Debug.Log("comienza pausa entre repeticiones| HORA: " + DateTime.Now.ToString());
    }

    override public void Prepare(BehaviourParams bp)
    {
        BehaviourParams lp = (BehaviourParams)bp;
        this.CentralNode._RealParams = lp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
        this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));
        int i = 0;
        foreach (Movement x in this.CentralNode.randomAnimations)
            Debug.Log(i++ + " " +  x);
	}	
	
	private void _Opposite_RepetitionEnd(object sender, EventArgs e)
	{
		OnRepetitionEnd();
	}
	override protected void PrepareWebInternal()
    {
        this.CentralNode._RealParams = new BehaviourParams();
        this._BehaviourState = AnimationBehaviourState.PREPARING_WEB;
	}
	override public void Run()
	{
        //if (this.CentralNode.actualRandomAnimationIndex == 0)
        Debug.Log("Run : " + (int)this.CentralNode.actualRandomAnimationIndex % this.CentralNode.randomAnimations.Count + " " + this.CentralNode.randomAnimations[(int)this.CentralNode.actualRandomAnimationIndex % this.CentralNode.randomAnimations.Count]);
        AnimatorScript.instance.CurrentExercise.Movement = this.CentralNode.randomAnimations[(int)this.CentralNode.actualRandomAnimationIndex % this.CentralNode.randomAnimations.Count];
		this.CentralNode.endRepTime = null;
		if (this.IsInterleaved)
		{
			this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
		}
		this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.animator.speed = this.CentralNode._RealParams.ForwardSpeed;
        this._StayInPoseState = StayInPoseState.GoingTo;
		this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
		this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
	}

    override public void RunWeb()
    {
        this.initializeRandomAnimations();
        if (this.IsInterleaved)
		{
			this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_DEFAULT);
		}
		this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;

        this.CentralNode._StayInPoseState = StayInPoseState.GoingTo;
        this.CentralNode.endRepTime = null;
        this.CentralNode._RealParams = new BehaviourParams();
        this.CentralNode.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
		this.CentralNode.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
	}
	override public void RunWeb(BehaviourParams bp)
	{
        if(this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
        {
            animator.speed = this.CentralNode._RealParams.ForwardSpeed;
            this.CentralNode._StayInPoseState = StayInPoseState.GoingTo;
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
            clockBehaviour.stopExecutionTimer();
            clockBehaviour.stopTimeBetweenRepetitionsTimer();
            this.CentralNode.actualRandomAnimationIndex = 0;
            IsRewinding = true;

            BehaviourParams p = (BehaviourParams)bp;
            this.CentralNode.endRepTime = null;
            this.CentralNode._RealParams = p;
            this.initializeRandomAnimations(bp.Variations);
        }
        else
        {
            this.CentralNode._StayInPoseState = StayInPoseState.GoingTo;
            BehaviourParams p = (BehaviourParams)bp;
            this.CentralNode.endRepTime = null;
            this.CentralNode._RealParams = p;
            this.initializeRandomAnimations(bp.Variations);
            this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
            this.CentralNode.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
            this.CentralNode.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
        }
		
	}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            if (this.IsCentralNode)
                animator.speed = 0;
            return;
        }

        if(this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS && !IsCentralNode && !IsWeb)
        {
            this.CentralNode.OnRepetitionReallyStart();
        }
        
        if (!this.IsCentralNode && this.CentralNode.randomAnimations[0] == this.movement)
            IsRewinding = false;

        if (this.IsCentralNode && hasEnteredBefore == false)
        {
            hasEnteredBefore = true;

            clockBehaviour = new ClockBehaviour();
            clockBehaviour.executionTimerFinish += executionTimerFinish;
            clockBehaviour.executionTimerStart += executionTimerStart;
            clockBehaviour.pauseBetweenRepetitionsStart += pauseBetweenRepetitionsStart;
            clockBehaviour.pauseBetweenRepetitionsFinish += pauseBetweenRepetitionsFinish;

        }
        
        if (this._BehaviourState == AnimationBehaviourState.PREPARING_WEB || this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
        {
            OnRepetitionEnd();
            Stop();
        }
        else if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS
                 || this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            //Como en este behaviour se utiliza animation.Play para cada repetición, se entra más de una vez al metodo OnStateEnter, 
            //por lo que si ya se ha entrado alguna vez, la velocidad se asigna como 0 para que se respete el tiempo entre ejecución 
            //antes de comenzar la siguiente repetición.

            //Se asume que si el ejercicio utiliza solo un tipo de velocidad, el forwardspeed y backwardspeed serán iguales.

            animator.speed = this.CentralNode._RealParams.ForwardSpeed;

        }/*
        if (IsCentralNode)
            Debug.Log("entre central ");
        else
            Debug.Log("entre ");*/

    }

    float startHoldTime;
	float startRestTime;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        clockBehaviour.Update();

        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            if (!animator.IsInTransition(0))
                animator.speed = 0;
            return;
        }
		float DELTA = 0.05f;
		DateTime temp = DateTime.Now;
        if (this._BehaviourState != AnimationBehaviourState.STOPPED && !IsCentralNode)
		{
            if (!BeginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) &&
			    this._BehaviourState != AnimationBehaviourState.PREPARING_WEB &&
			    this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS &&
			    this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
			{
				OnRepetitionReallyStart();
				BeginRep = true;
			}

            if (this.CentralNode._StayInPoseState == StayInPoseState.GoingTo && stateInfo.normalizedTime - DELTA >= 1)
			{
                animator.speed = 0;
				startHoldTime = Time.time;
                clockBehaviour.executeRepetitionTime(this.CentralNode._RealParams.SecondsInPose);
                this.CentralNode._StayInPoseState = StayInPoseState.HoldingOn;
			}
			
			//Si ya pasó el tiempo en el ángulo máximo
			else if(this.CentralNode._StayInPoseState == StayInPoseState.HoldingOn && this.CentralNode.holdingPose)
			{
                this.CentralNode.holdingPose = false;
				animator.StartRecording(0);
				animator.speed = -this.CentralNode._RealParams.BackwardSpeed;
				animator.StopRecording();
                this.CentralNode._StayInPoseState = StayInPoseState.Leaving;
            }
			else if (this.CentralNode._StayInPoseState == StayInPoseState.Leaving && 
                stateInfo.normalizedTime - DELTA <= 0)
			{
                BeginRep = false;
				animator.speed = this.CentralNode._RealParams.ForwardSpeed;
                this.CentralNode._StayInPoseState = StayInPoseState.GoingTo;
				startRestTime = Time.time;
                animator.SetInteger("Movement", -1);
				
				if (IsInterleaved && this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
				{
					animator.SetTrigger("ChangeLimb");
				}
				
				if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                    _BehaviourState = AnimationBehaviourState.STOPPED;

                //OnRepetitionEnd();

            }
		}

    }
	
	
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!IsCentralNode)
        {
            BeginRep = false;
            if (this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_DEFAULT)
            {
                OnLerpRoundTripEnd();

                if (!IsRewinding && (!IsInterleaved || (IsInterleaved && limb == Limb.Right)))
                {
                    this.PauseAnimation();
                    OnRepetitionEnd();
                    SetNextVariation();
                }
                if (IsInterleaved)
                {
                    animator.SetTrigger("ChangeLimb");
                }
                if (!IsInterleaved && ((this.IsWeb) || (this.IsInInstruction) && !IsRewinding))
                {
                    animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
                }

                if (this._BehaviourState == AnimationBehaviourState.STOPPED)
                {
                    this.CentralNode.endRepTime = null;
                }
            }
            else if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
            {
                OnRepetitionEnd();
                Stop();
            }
            else if (this._BehaviourState == AnimationBehaviourState.STOPPED) 
            {
                //Salida forzada de la repetición, aumentar indice
                Debug.Log("aumentando contador forzoso " + this.CentralNode.IsInInstruction);
                ++this.CentralNode.actualRandomAnimationIndex;
            }
            if ((this.IsWeb) || (this.IsInInstruction) && !IsRewinding)
            {
//                Debug.Log("2-ISWEB: [" + this.IsWeb + "] isInInsruction [" + this.IsInInstruction + "]" + "is rewinding [" + IsRewinding + "]" );
                this.ResumeAnimation();
            }
        }
    }

    private float SecondsBetweenRepetitions;
    public float secondsBetweenRepetitions
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._realParams.SecondsBetweenRepetitions;
            }
            else
            {
                return CentralNode._realParams.SecondsBetweenRepetitions;
            }

        }
    }

    public override void ResumeAnimation()
    {
        if (this.IsRepetitionEnd)
        {
            clockBehaviour.executeTimeBetweenRepetitions(secondsBetweenRepetitions);
            animator.speed = this.CentralNode._RealParams.ForwardSpeed;
        }
        base.ResumeAnimation();
    }

    protected override void OnRepetitionEnd()
	{
        if (!IsCentralNode)
            this.CentralNode.OnRepetitionEnd();
        else
        {
            base.OnRepetitionEnd();
        }
	}
	/// <summary>
	/// Detiene la interpolación que actualmente se está ejecutando
	/// </summary>
	override public void Stop()
	{
        clockBehaviour.stopExecutionTimer();
        clockBehaviour.stopTimeBetweenRepetitionsTimer();
        IsResumen = false;
		animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
		this.CentralNode._BehaviourState = AnimationBehaviourState.STOPPED;
        this.animator.speed = this.CentralNode._RealParams.ForwardSpeed;
	}
	
	void OnDestroy()
	{
		if (this.IsInterleaved)
			this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;
		
		this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
		
		base.OnDestroy();
	}
}