﻿using UnityEngine;
using System.Collections;
using System;
using Assets;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class LerpBehaviour : AnimationBehaviour {
    #region Variables
    /// <summary>
    /// Velocidad durante la fase concéntrica
    /// </summary>
    private float forwardSpeed = 1f;

    /// <summary>
    /// Velocidad durante la fase excentrica
    /// </summary>
    private float backwardSpeed = 1f;

    /// <summary>
    /// Momento en segundos en que la animación está en el ángulo máximo
    /// </summary>
    /// //TODO: Debe ser privado
    private float timeTakenDuringLerp = 1f;

    /// <summary>
    /// Indica si actualmente de está interpolando
    /// </summary>
    private bool isLerping = false;

    /// <summary>
    /// Indica desde que punto se hará la interpolación
    /// </summary>
    private float startPosition;

    /// <summary>
    /// Indica hasta qué punto se hará la interpolación
    /// </summary>
    private float endPosition;

    /// <summary>
    /// Marca de tiempo en que se inició la interpolación
    /// </summary>
    private float timeStartedLerping;

    /// <summary>
    /// Se utiliza para enviar datos a ExerciseDataGenerator en intervalos de tiempo determinados.
    /// El tiempo que ha pasado desde que se hizo la última captura de datos.
    /// </summary>
    private float timeSinceCapture = 0;

    /// <summary>
    /// Representa el estado actual de la animación, este se utiliza para identificar el sentido de la animación y aplicar las curvas de aceleración o desaceleración
    /// </summary>
    private LerpState _currentLerpState;

    /// <summary>
    /// Representa el último estado de la animación, este se utiliza para identificar el sentido de la animación y aplicar las curvas de aceleración o desaceleración
    /// </summary>
    private LerpState _lastLerpState;

    private bool holdingPose = false;

    private event EventHandler LerpRoundTripEnd;
    protected void OnLerpRoundTripEnd()
    {
        EventHandler eh = LerpRoundTripEnd;
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
    }
    #endregion
    protected override bool HasCentralNode { get { return false; } }

    protected override AnimationBehaviourState _BehaviourState
    {
        get { return _behaviourState; }
        set { 
            _behaviourState = value;
            switch (value) 
            { 
                case AnimationBehaviourState.RUNNING_DEFAULT:
                case AnimationBehaviourState.RUNNING_WITH_PARAMS:
                    animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
                    break;
                case AnimationBehaviourState.PREPARING_WITH_PARAMS:
                    StartLerp(); 
                    break;
            }
        }
    }


    //public Animator animator;
    private List<AnimationInfo> _timeAndAngles;

    /// <summary>
    /// Tiempo que demora acelerar o desacelerar el movimiento concéntrico
    /// </summary>
    private float timeTakenDuringForwardLerp = 1f;

    /// <summary>
    /// Tiempo que demora acelerar o desacelerar el movimiento excéntrico
    /// </summary>
    private float timeTakenDuringBackwardLerp = 1f;
    public BehaviourParams GetParams()
    {
        return this._currentParams;
    }
    /*
    public void SetParams(LerpParams lp, LerpBehaviourState lbs)
    {
        this._realLerpParams = lp;
        this._lerpBehaviourState = lbs;
    }
    */
    //
    //==== Controlar tiempos de repeticiones
    //

    public ClockBehaviour _ClockBehaviour;
    public ClockBehaviour clockBehaviour
    {
        get
        {
            return this._ClockBehaviour;
        }
        set
        {
            this._ClockBehaviour = value;
        }
    }

    public float secondsInPose
    {
        get
        {
            return this._realParams.SecondsInPose;
        }
    }

    //
    //==== FIN Controlar tiempos de repeticiones
    //


    //===== FIN RELOJ

    private void executionTimerStart()
    {
        Debug.Log("comienza ejecucion| HORA: " + DateTime.Now.ToString());
        //Debug.Log(" INICIO manteniendo pose pose ");
        this.holdingPose = true;
    }

    private void executionTimerFinish()
    {
        Debug.Log("termina ejecucion| HORA: " + DateTime.Now.ToString());
        //Debug.Log("FIN manteniendo pose pose ");
        this.holdingPose = false;
        clockBehaviour.stopExecutionTimer();
        animator.speed = -backwardSpeed;
    }


    /**
    Cuando se acaba el tiempo de pausa entre repeticiones
    **/
    private void pauseBetweenRepetitionsFinish()
    {
        //clockBehaviour.stopTimeBetweenRepetitionsTimer();
        //Debug.Log("termina pausa entre repeticiones| HORA: " + DateTime.Now.ToString());
        //SetNextVariation();
    }

    private void pauseBetweenRepetitionsStart()
    {
        //Debug.Log("comienza pausa entre repeticiones| HORA: " + DateTime.Now.ToString());
    }
    override public void Prepare(BehaviourParams bp)
    {
        BehaviourParams lp = (BehaviourParams)bp;
        this._RealParams = lp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_DEFAULT;
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;
    }

    private void _Opposite_RepetitionEnd(object sender, EventArgs e)
    {
        OnRepetitionEnd();
    }
    override protected void PrepareWebInternal()
    {
        /*
        if (this.IsInterleaved)
        {
            this.isWeb = true;
            this._Opposite.isWeb = true;
        }*/
        this._BehaviourState = AnimationBehaviourState.PREPARING_WEB;
    }
    override public void Run()
    {
        endRepTime = null;
        //animator.speed = 1;
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }
        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
        
        //animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
        //this.StartLerp();
    }

    
    override public void RunWeb()
    {
        endRepTime = null;
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_DEFAULT);
        }
        this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }
    override public void RunWeb(BehaviourParams bp)
    {
        BehaviourParams lerpParams = (BehaviourParams)bp;
        endRepTime = null;
        
        
        isLerping = false;
        ReadyToLerp = false;
	

        this._RealParams = lerpParams;
        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }

    /// <summary>
    /// Almacena una lista de frames con su correspondiente ángulo, considerando el ejercicio especificado, por ejemplo, para rodilla puede importar el ángulo que se genera entre los segmentos involucrados con el plano sagital
    /// </summary>
    /// <param name="exercise">Ejercicio al cual se le quieren extraer los ángulo de interés</param>
    /// <param name="action">Callback especificado para guardar los frames que se van generando</param>
    /// <returns></returns>
    private void SaveTimesAngle(AnimatorStateInfo animatorState) //ref List<AnimationInfo> aInfo)
    {
        JointTypePlanoResult tempJointTypePlanoResult = MovementJointMatch.movementJointMatch[new MovementLimbKey(movement, limb)];
        ArticulacionClass joint = AnimatorScript.instance.utils.getArticulacion(tempJointTypePlanoResult.jointType);
        AnimationInfo tempAnimationInfo = new AnimationInfo();
        float time = animatorState.normalizedTime * animatorState.length;
        switch (tempJointTypePlanoResult.plain)
        {   
            case Plano.planos.planoFrontal:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleFrontal);
                break;
            case Plano.planos.planoHorizontal:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleHorizontal);
                break;
            case Plano.planos.planoSagital:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleSagital);
                break;
            case Plano.planos.planoHorizontalAcostado:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleHorizontalAcostado);
                break;
        }
        _timeAndAngles.Add(tempAnimationInfo);
    }


	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private float defaultAnimationLength;
    private bool hasEnteredBefore = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if (hasEnteredBefore == false)
        {
            hasEnteredBefore = true;

            clockBehaviour = new ClockBehaviour();
            clockBehaviour.executionTimerFinish += executionTimerFinish;
            clockBehaviour.executionTimerStart += executionTimerStart;
            clockBehaviour.pauseBetweenRepetitionsStart += pauseBetweenRepetitionsStart;
            clockBehaviour.pauseBetweenRepetitionsFinish += pauseBetweenRepetitionsFinish;

        }
        if (this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            return;
        }
        defaultAnimationLength = stateInfo.length;
       
        //Está la animación en caché
        if(PreparedExercises.tryGetPreparedExercise(new Exercise(movement, limb), out this._timeAndAngles, stateInfo.length))
        {
            //Repetición de preparación
            if (this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT && this._BehaviourState != AnimationBehaviourState.PREPARING_WEB)
            {
                this.StartLerp();
            }

            else if (this._BehaviourState == AnimationBehaviourState.PREPARING_DEFAULT)
            {
                //OnRepetitionEnd();
                _BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
                timeSinceCapture = 0;
                this.StartLerp();
            }

            else if (this._BehaviourState == AnimationBehaviourState.PREPARING_WEB)
            {
                OnRepetitionEnd();
                Stop();
            }
        }
        //No está la animación en caché
        else
        {
            if (this._BehaviourState == AnimationBehaviourState.PREPARING_DEFAULT || this._BehaviourState == AnimationBehaviourState.PREPARING_WEB)
            {
                this._timeAndAngles = new List<AnimationInfo>();
                this.StartLerp();
            }
            else
            {
                throw new Exception("Animation not prepared");
            }
        }
	}
    void LerpBehaviour_LerpRoundTripEnd(object sender, EventArgs e)
    {
        endRepTime = DateTime.Now;
        if (IsInterleaved)
        {

            (this._Opposite as LerpBehaviour).endRepTime = endRepTime;
        }
    }
    const float INTERVAL = 0.1f;
    private bool ReadyToLerp = false;
    private bool lastReadyToLerp = false;
	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    int debug = 0;
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        clockBehaviour.Update();
        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            if (!animator.IsInTransition(0))
                animator.speed = 0;
            return;
        }
                
        #region Interpolate
        //Si no estamos en estado Stopped 
        //Y estamos preparados para hacer Lerp
        //Y el tiempo que ha transcurrido de la ultima rep es mayor al tiempo de pause entre repeticiones. O no ha habido última rep.
       
        if (_BehaviourState != AnimationBehaviourState.STOPPED && ReadyToLerp
            && (endRepTime == null || new TimeSpan(0, 0, (int)_currentParams.SecondsBetweenRepetitions) <= DateTime.Now - endRepTime))
        {
            if (!BeginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) && 
                this._BehaviourState != AnimationBehaviourState.PREPARING_WEB && 
                this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS && 
                this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
            {
                OnRepetitionReallyStart();
                BeginRep = true;
            }
            //if(_LerpBehaviourState == LerpBehaviourState.STOPPED)
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = 0f;
            /*
            switch (_currentLerpState)
            {
                case LerpState.Forward:
                    
                    percentageComplete = timeSinceStarted / timeTakenDuringForwardLerp;
                    Debug.Log("1 " + percentageComplete + "= " + timeSinceStarted + " / " + timeTakenDuringForwardLerp + " " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime );
                    break;

                case LerpState.Stopped:
                    if (_lastLerpState == LerpState.Forward)
                    {
                        percentageComplete = timeSinceStarted / timeTakenDuringForwardLerp;
                        Debug.Log("2 " + percentageComplete + "= " + timeSinceStarted + " / " + timeTakenDuringForwardLerp + " " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    }

                    //De ser true, indica que termino una repeticion
                    else if (_lastLerpState == LerpState.Backward)
                    {
                        percentageComplete = timeSinceStarted / timeTakenDuringBackwardLerp;
                        Debug.Log("3 " + percentageComplete + "= " + timeSinceStarted + " / " + timeTakenDuringBackwardLerp + " " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    }
                    break;

                case LerpState.Backward:
                    percentageComplete = timeSinceStarted / timeTakenDuringBackwardLerp;
                    Debug.Log("4 " + percentageComplete + "= " + timeSinceStarted + " / " + timeTakenDuringBackwardLerp + " " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    break;
            }*/

            percentageComplete = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            //float normalizedSpeed;
            //if (percentageComplete < 0.5)
            //    normalizedSpeed = timeSinceStarted/timeTakenDuringForwardLerp * 4;
            //else
            //    normalizedSpeed = 4f- timeSinceStarted / timeTakenDuringForwardLerp * 4;
            //Aplico el suavizado "Smotherstep"
            //percentageComplete = percentageComplete * percentageComplete * (3f - 2f * percentageComplete);
            //float percentageSmotherstep = timeSinceStarted * timeSinceStarted * timeSinceStarted * (timeSinceStarted * (6f * timeSinceStarted - 15f) + 10f);

            float sp = endPosition + startPosition;


            if (!holdingPose)
            {
                animator.StartRecording(0);
                animator.speed = sp;// Mathf.Lerp(startPosition, endPosition, normalizedSpeed);
                                    //Debug.Log("% "+ percentageComplete + "normalized speed " + normalizedSpeed + "  speed " + animator.speed);
                animator.StopRecording();
            }

            float DELTA = 0.03f;
            if ((animator.speed > 0  && percentageComplete >= 1.0f - DELTA) ||
                (animator.speed < 0 && percentageComplete <= 0f + DELTA))
            {
                Debug.Log("int end " + percentageComplete);
                InterpolationEnd();
            }
        }
        else if(this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
        {
            animator.speed = 0;
        }


        if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
        {
            timeSinceCapture += Time.deltaTime;
            if (timeSinceCapture > INTERVAL)
            {
                timeSinceCapture = timeSinceCapture - INTERVAL;
                /*
                if (exerciseDataGenerator == null)
                    exerciseDataGenerator = GameObject.FindObjectOfType<Detector.ExerciseDataGenerator>();
                if (this.exerciseDataGenerator != null)
                    this.exerciseDataGenerator.CaptureData();*/
            }
        }

        
        #endregion
        if (_BehaviourState == AnimationBehaviourState.PREPARING_DEFAULT || _BehaviourState == AnimationBehaviourState.PREPARING_WEB)
            this.SaveTimesAngle(stateInfo);
        lastReadyToLerp = ReadyToLerp;


        JointTypePlanoResult tempJointTypePlanoResult = MovementJointMatch.movementJointMatch[new MovementLimbKey(movement, limb)];
        ArticulacionClass joint = AnimatorScript.instance.utils.getArticulacion(tempJointTypePlanoResult.jointType);
        AnimationInfo tempAnimationInfo = new AnimationInfo();
        float time = stateInfo.normalizedTime; //* stateInfo.length;
        switch (tempJointTypePlanoResult.plain)
        {
            case Plano.planos.planoFrontal:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleFrontal);
                break;
            case Plano.planos.planoHorizontal:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleHorizontal);
                break;
            case Plano.planos.planoSagital:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleSagital);
                break;
            case Plano.planos.planoHorizontalAcostado:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleHorizontalAcostado);
                break;
        }
        //GameObject.FindGameObjectWithTag("angulotexto").GetComponent<Text>().text = "Angulo " + joint.articulacion.ToString() + " : " + tempAnimationInfo.angle.ToString();

    }


	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.speed = 1.0f;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}


    /// <summary>
    /// Comienza la interpolación entre 0 y el tiempo donde está el ángulo requerido (LerpTime), aplicando las velocidades concéntras y excéntricas indicadas
    /// </summary>
    /// <param name="_lerpTime">Tiempo donde se encuentra el ángulo al que se quiere llegar</param>
    /// <param name="_forwardSpeed">Velocidad concéntrica</param>
    /// <param name="_backwardSpeed">Velocidad excéntrica</param>
    public void StartLerp()
    {
        _currentLerpState = LerpState.Forward;

        if (_BehaviourState == AnimationBehaviourState.PREPARING_DEFAULT || _BehaviourState == AnimationBehaviourState.PREPARING_WEB || _BehaviourState == AnimationBehaviourState.RUNNING_DEFAULT)
        {
            _currentParams = new BehaviourParams(defaultAnimationLength, 1, 1, 0, 3);
            timeTakenDuringLerp = (float)Math.Floor(defaultAnimationLength * 10) / 10;
            Debug.Log("1timeTakenDuringLerp " + timeTakenDuringLerp);
        }
        else
        {
            _currentParams = _RealParams;
            timeTakenDuringLerp = (float)Math.Floor(GetAnimationInfo(_currentParams.Angle, _timeAndAngles).time * 10) / 10;
            Debug.Log("2timeTakenDuringLerp " + timeTakenDuringLerp);
        }
        //Normalizo el tiempo que tardo en parar
        timeTakenDuringForwardLerp = timeTakenDuringLerp / _currentParams.ForwardSpeed;
        timeTakenDuringBackwardLerp = timeTakenDuringLerp / _currentParams.BackwardSpeed;
        forwardSpeed = _currentParams.ForwardSpeed;
        backwardSpeed = _currentParams.BackwardSpeed;
        BeginLerp(0, forwardSpeed, true);
    }

    /// <summary>
    /// Inicia una interpolación y ajusta los valores iniciales
    /// </summary>
    private void BeginLerp(float startPos, float endPos, bool pauseBeforeLerping = false)
    {
        timeStartedLerping = Time.time;
        // En caso de que se ejecute un Lerp que requiera pausa entre lerps, se agrega el tiempo de pausa a el tiempo de inicio del Lerp.
        if (pauseBeforeLerping && endRepTime != null)
            timeStartedLerping += _currentParams.SecondsBetweenRepetitions;
        startPosition = startPos;
        endPosition = endPos;
        ReadyToLerp = true;
    }
    

  /*  public override void PauseAnimation()
    {
        DebugLifeware.Log("Puasing animation", DebugLifeware.Developer.Alfredo_Gallardo);
        originalABS = this._behaviourState;



        _BehaviourState = AnimationBehaviourState.STOPPED;

        if (this.IsInterleaved)
            if ((_Opposite as LerpBehaviour)._BehaviourState != AnimationBehaviourState.STOPPED)
                _Opposite.PauseAnimation();

        if (animator.speed < 0)
        {
            animator.StartRecording(0);
            animator.speed *= -1;
            animator.StopRecording();
        }

        //animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
        animator.speed = 1;
       
    }*/
    /// <summary>
    /// Cuando termina una interpolacion se comprueba el estado de la animacion para continuar con el ciclo de aceleración y desaceleracion
    /// </summary>
    private void InterpolationEnd()
    {
        switch (_currentLerpState)
        {
            case LerpState.Forward:
                _currentLerpState = LerpState.Stopped;
                _lastLerpState = LerpState.Forward;
                animator.speed = 0;
                //BeginLerp(0, -backwardSpeed);
                this.holdingPose = true;
                Debug.Log("Comenzando pausa entre rep " + this._currentParams.SecondsInPose);
                clockBehaviour.executeRepetitionTime(this._currentParams.SecondsInPose);
                break;

            case LerpState.Stopped:
                //si holdingPose es TRUE el instructor esta en mantener Pose
                if (holdingPose == false) {
                    Debug.Log("Holding Pose " + holdingPose + " " + _currentLerpState + " " +  _lastLerpState + " " + backwardSpeed);
                    if (_lastLerpState == LerpState.Forward)
                    {
                        _currentLerpState = LerpState.Backward;
                        _lastLerpState = LerpState.Stopped;
                        BeginLerp(0, -backwardSpeed);
                    }


                    //De ser true, indica que termino una repeticion
                    else if (_lastLerpState == LerpState.Backward)
                    {
                        _currentLerpState = LerpState.Forward;
                        _lastLerpState = LerpState.Forward;
                        BeginRep = false;
                        if (this._BehaviourState == AnimationBehaviourState.PREPARING_DEFAULT || this._BehaviourState == AnimationBehaviourState.PREPARING_WEB)
                        {
                            try
                            {
                                DebugLifeware.Log("se intentara savear", DebugLifeware.Developer.Marco_Rojas);
                                PreparedExercises.InsertPreparedExercise(new Exercise(movement, limb), _timeAndAngles);
                            }
                            catch
                            {
                                DebugLifeware.Log("ya existia el ejercicio", DebugLifeware.Developer.Marco_Rojas);
                                ; // do nothing
                            }
                            if (this._BehaviourState == AnimationBehaviourState.PREPARING_DEFAULT)
                            {
                                this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
                            }
                            else
                            {
                                this._BehaviourState = AnimationBehaviourState.STOPPED;
                                //TODO: NO SABEMOS SI DEJAR ESTA LINEA
                                Stop();
                                OnRepetitionEnd();
                            }
                        }
                        else if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                        {
                            //_BehaviourState = AnimationBehaviourState.STOPPED;
                            Stop();
                            OnRepetitionEnd();
                            //TODO: Recolectar datos y entregarlos a jorge
                        }
                        //Hace repeticiones hasta el infinito
                        else if (this._BehaviourState == AnimationBehaviourState.RUNNING_DEFAULT || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                        {

                            OnLerpRoundTripEnd();
                            if (!IsInterleaved || IsInterleaved && limb == Limb.Right)
                            {
                                if ((!this.IsWeb) && (!this.IsInInstruction))
                                    this.PauseAnimation();
                                OnRepetitionEnd();
                                Debug.Log("REPETITION END____");
                            }

                            if (IsInterleaved)
                            {
                                if (this.limb == Limb.Left)
                                    this._Opposite.endRepTime = null;
                                animator.SetTrigger("ChangeLimb");
                            }
                            if (this._BehaviourState == AnimationBehaviourState.STOPPED)
                            {
                                endRepTime = null;
                                ReadyToLerp = false;

                            }
                            else if (!IsInterleaved)
                            {

                                StartLerp();
                                //BeginLerp(0, forwardSpeed, true);
                                //animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
                            }
                        }
                    }
                }
                break;

            case LerpState.Backward:
                _currentLerpState = LerpState.Stopped;
                _lastLerpState = LerpState.Backward;
                BeginLerp(-backwardSpeed, 0);
                break;
        }
    }

    /// <summary>
    /// Detiene la interpolación que actualmente se está ejecutando
    /// </summary>
    override public void Stop()
    {
        _BehaviourState = AnimationBehaviourState.STOPPED;

        if(this.IsInterleaved)
        if ((_Opposite as LerpBehaviour)._BehaviourState != AnimationBehaviourState.STOPPED)
            _Opposite.Stop();

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        if (animator.speed < 0)
        {
            //animator.SetFloat("MovSpeed", animator.GetFloat("MovSpeed") * -1);
            animator.StartRecording(0);
            animator.speed *= -1;
            animator.StopRecording();

        }

        animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
        animator.speed = 1;

    }
    /// <summary>
    /// Obtiene el intervalo de tiempo en segundos y el ángulo más cercano al entregado por parámetro
    /// </summary>
    /// <param name="angle">Ángulo que se quiere buscar en la lista de frames</param>
    /// <param name="aiList">Lista de AnimationInfo que representan el tiempo en segundos y el ángulo en cada frame</param>
    /// <returns>Retorna una AnimationInfo que contiene el tiempo y el ángulo encontrado más cercano al solicitado</returns>
    AnimationInfo GetAnimationInfo(float angle, List<AnimationInfo> aiList)
    {
        //TODO: Quizás se pueda mejorar
        List<AnimationInfo> aiListTemp = aiList.GetRange(1, (int)aiList.Count / 2);
        aiListTemp.Reverse();
        AnimationInfo aiTemp = new AnimationInfo(float.MaxValue, float.MaxValue);
        bool wasNear = false;
        foreach (AnimationInfo ai in aiListTemp)
        {
            float dif = Math.Abs(ai.angle - angle);
            if ((dif <= 3) && !wasNear)
            {
                wasNear = true;
            }
            else if ((dif > 3) && wasNear)
            {
                return aiTemp;
            }

            if (dif < Math.Abs(aiTemp.angle - angle))
                aiTemp = ai;
            else
                continue;
        }
        
        return aiTemp;
    }
    void OnDestroy()
    {
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;

        base.OnDestroy();
    }

}