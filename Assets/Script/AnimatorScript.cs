using UnityEngine;
using System.Collections;
using System.Timers;
using System;
using System.ComponentModel;
using Assets;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.UI;

// Require these components when using this script
[RequireComponent(typeof(Animator))]

public class AnimatorScript : MonoBehaviour
{
    const bool RESET_CACHE = true;
    #region Variables privadas
    /// <summary>
    /// Referencia al Animator
    /// </summary>
    [HideInInspector]
    public Animator anim  {
        get
        {
            return AnimatorScript.instance.GetComponent<Animator>();
        }
    }
    public HumanoidUtils utils
    {
        get
        {

            return AnimatorScript.instance.GetComponent<HumanoidUtils>();
        }
    }
    private Exercise _currentExercise = new Exercise();
    private const float DELAY = 2f;
    private const float DELAY_TO_FAST_PREPARE = 0.3F;
    public static AnimatorScript instance{
        get{
            return FindObjectOfType<AnimatorScript>();
        }
    }
    

    public event EventHandler<RepetitionStartEventArgs> OnRepetitionStart;
    public event EventHandler OnRepetitionEnd;
    public event EventHandler OnSubrepetitionEnd;
    public event EventHandler OnRepetitionReallyStart;
    public event EventHandler<PrepareEventArgs> OnPrepareExerciseStart;
    public event EventHandler<PrepareEventArgs> OnPrepareExerciseEnd;


    Caller prepareCaller;
    [HideInInspector]
    public BehaviourParams behaviourParams;

    public Exercise CurrentExercise
    {
        get
        {
            return _currentExercise;
        }
        set
        {
            //value = (Exercise)value;
            _currentExercise = value;
            RewindExercise();
        }
    }




    #endregion


    void Start()
    {
        CurrentExercise.PropertyChanged += currentExercise_PropertyChanged;
    }

    public void SetIdle(int pose)
    {
        anim.SetInteger("Idle", pose);
    }

    public void RewindExercise()
    {
        anim.SetInteger(AnimatorParams.Limb, (int)_currentExercise.Limb);
        anim.SetInteger(AnimatorParams.Movement, (int)_currentExercise.Movement);
    }
    public void testPrepare()
    {
        Movement mov = Movement.EstocadaFrontalCorta;
        Limb l = Limb.Left;
        float forwardSpeed = (float)Convert.ToDouble(FixValue(GameObject.Find("VEL IDA/Text").GetComponent<Text>().text, "1"));
        float backwardSpeed = (float)Convert.ToDouble(FixValue(GameObject.Find("VEL VUELTA/Text").GetComponent<Text>().text, "1"));
        int secondsBE = Convert.ToInt32(FixValue(GameObject.Find("PAUSA EXE/Text").GetComponent<Text>().text, "0"));
        int secondsBR = Convert.ToInt32(FixValue(GameObject.Find("PAUSA REPS/Text").GetComponent<Text>().text, "0"));
        float rom = AngleFixer.FixAngle((float)Convert.ToDouble(FixValue(GameObject.Find("ROM/Text").GetComponent<Text>().text, "70")), mov);
        Debug.Log("rom " + rom);

        List<Movement> variations = new List<Movement>() {};

        PrepareExercise(new Exercise(mov, l), new BehaviourParams(rom, forwardSpeed, backwardSpeed, secondsBE, secondsBR, variations));
    }
    private string FixValue(string valueToFix, string fix)
    {
        return valueToFix != String.Empty ? valueToFix : fix;
    }
    public void testRun()
    {
        float forwardSpeed = (float)Convert.ToDouble(FixValue(GameObject.Find("VEL IDA/Text").GetComponent<Text>().text, "1"));
        float backwardSpeed = (float)Convert.ToDouble(FixValue(GameObject.Find("VEL VUELTA/Text").GetComponent<Text>().text, "1"));
        int secondsBE = Convert.ToInt32(FixValue(GameObject.Find("PAUSA EXE/Text").GetComponent<Text>().text, "0"));
        int secondsBR = Convert.ToInt32(FixValue(GameObject.Find("PAUSA REPS/Text").GetComponent<Text>().text, "0"));
        float rom = AngleFixer.FixAngle((float)Convert.ToDouble(FixValue(GameObject.Find("ROM/Text").GetComponent<Text>().text, "70")), CurrentExercise.Movement);

        BehaviourParams param = new BehaviourParams(rom, forwardSpeed, backwardSpeed, secondsBE, secondsBR);

  
        RunExercise(true);
  

    }
    public void testResume()
    {
        ResumeExercise();
    }
  

    void Awake()
    {
        //TODO: No olvidar borrar, elimina el caché de los ejercicios ya preparados
        if (RESET_CACHE)
            PlayerPrefs.SetString("ExerciseCache", null);
    }
    // Update is called once per frame


    void currentExercise_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Exercise exercise = (Exercise)sender;
        switch (e.PropertyName)
        {
            case AnimatorParams.Movement:
                anim.SetInteger(AnimatorParams.Movement, (int)exercise.Movement);
                break;

            case AnimatorParams.Limb:
                anim.SetInteger(AnimatorParams.Limb, (int)exercise.Limb);
                break;
        }
    }

    public AnimationBehaviour behaviour;

   
    /// <summary>
    /// Nota: Si hay errores de Quaternion to Matrix comprobar que los parametros enviados son validos para el ejercicio indicado
    /// </summary>
    /// <param name="e"></param>
    /// <param name="param"></param>
    public void PrepareExercise(Exercise e, BehaviourParams param)
    {

        param.Angle = AngleFixer.FixAngle(param.Angle, e.Movement);
        param.BackwardSpeed *= SpeedFixer.FixSpeed(e.Movement);
        param.ForwardSpeed *= SpeedFixer.FixSpeed(e.Movement);

        if (param.Variations == null || param.Variations.Count == 0)
            behaviour = AnimationBehaviour.GetBehaviour(e.Movement, e.Limb);
        else
            behaviour = AnimationBehaviour.GetCentralBehaviour(e.Movement, e.Limb);

        if (behaviour == null)
        {
            Debug.LogError("No se encontró la máquina de estado. (Ejercicio = " + e.Movement + " "
                + (int)e.Movement + ") (Limb = " + e.Limb + ")" +
                "). Posiblemente se deba a una mala combinación de esos parámetros o el MonitoAnimatorController se bugeó");
            return;
        }

        behaviour.Prepare(param);
        behaviour.RepetitionEnd += behaviour_PrepareEnd;

        if (param.Variations == null || param.Variations.Count == 0)
            CurrentExercise = e;
        else
            CurrentExercise = new Exercise(behaviour.randomAnimations[0], e.Limb);
    }

    private IEnumerator InitialPoseDelayed()
    {

        //yield return new WaitForSeconds(DELAY);
        RewindExercise();
        yield return new WaitForSeconds(0);//0.1f);
        behaviour.InitialPose();
    }


    public void InitialPose()
    {
        StartCoroutine(InitialPoseDelayed());
    }

    public void ResetVariations()
    {
        if (this.behaviour != null && this.behaviour.CentralNode != null)
            this.behaviour.CentralNode.actualRandomAnimationIndex = 0;
    }
    public class PrepareExerciseWebParams
    {
        public Exercise Exercise { get; set; }
        public Caller Caller { get; set; }
        public PrepareExerciseWebParams(Exercise e, Caller c)
        {
            this.Exercise = e;
            this.Caller = c;
        }
    }
    float timeSinceStartPrepareWeb;
    public void PrepareExerciseWeb(string s)
    {

        RaiseEvent(OnPrepareExerciseStart, PrepareStatus.Preparing);
        PrepareExerciseWebParams pwp = JsonConvert.DeserializeObject<PrepareExerciseWebParams>(s);

        //PrepareExerciseWebParams pwp = new PrepareExerciseWebParams(new Exercise(Movement.EstocadaFrontalCorta,Limb.Left), Caller.Preview);//TEST NO JSON
        //PrepareExerciseWebParams pwp = JsonUtility.FromJson<PrepareExerciseWebParams>(s);// as PrepareExerciseWebParams;
        //JsonUtility.FromJson<PlayerInfo>(jsonString);
        Exercise e = (pwp.Exercise) as Exercise;
        this.prepareCaller = (Caller)(pwp.Caller);

        behaviour = AnimationBehaviour.GetBehaviour(e.Movement, e.Limb);
        if (behaviour == null)
        {
            DebugLifeware.LogAllDevelopers("Importante: Behaviour no encontrado");
            RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.NotFound);
            return;
        }
        behaviour.RepetitionEnd += behaviour_PrepareEndWeb;
        timeSinceStartPrepareWeb = Time.time;  
        behaviour.PrepareWeb();
        CurrentExercise = e;    
    }

    void behaviour_PrepareEndWeb(object sender, EventArgs e)
    {
       behaviour = sender as AnimationBehaviour;
        behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
        if (timeSinceStartPrepareWeb > Time.time - DELAY_TO_FAST_PREPARE)
            StartCoroutine(RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.Prepared, DELAY_TO_FAST_PREPARE));
        else 
            RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.Prepared);
    }

    public void RunExercise(bool isInInstruction)
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.Run(isInInstruction);
        RewindExercise();
        behaviour.RepetitionEnd -= behaviour_RepetitionEnd;
        behaviour.RepetitionEnd += behaviour_RepetitionEnd;

        behaviour.RepetitionStart -= behaviour_RepetitionStart;
        behaviour.RepetitionStart += behaviour_RepetitionStart;
        behaviour.SubrepetitionEnd -= behaviour_SubrepetitionEnd;
        behaviour.SubrepetitionEnd += behaviour_SubrepetitionEnd;
        behaviour.RepetitionReallyStart -= behaviour_RepetitionReallyStart;
        behaviour.RepetitionReallyStart += behaviour_RepetitionReallyStart;
    }

    private void behaviour_RepetitionStart(object sender, RepetitionStartEventArgs e)
    {
        RaiseEvent(OnRepetitionStart, e);
    }


    void behaviour_RepetitionReallyStart(object sender, EventArgs e)
    {
        RaiseEvent(OnRepetitionReallyStart);
    }

    public void ResumeExercise()
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.ResumeAnimation();
    }
    void behaviour_RepetitionEnd(object sender, EventArgs e)
    {
        //Terminó una repetición
        RaiseEvent(OnRepetitionEnd);
    }

    void behaviour_SubrepetitionEnd(object sender, EventArgs e)
    {
        //Terminó una subrepetición
        RaiseEvent(OnSubrepetitionEnd);
    }

    void behaviour_PrepareEnd(object sender, EventArgs e)
    {
        //        GoToIddle();
        behaviour = sender as AnimationBehaviour;
        behaviour.RepetitionEnd -= behaviour_PrepareEnd;
        RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.Prepared);
    }

    public void testWeb()
    {
        RunExerciseWebWithoutParams();
    }
    public void testWeb2()
    {
        StopExercise();
    }
    public void RunExerciseWeb(string s)
    {
        BehaviourParams p = JsonConvert.DeserializeObject<BehaviourParams>(s);
        //BehaviourParams p = new BehaviourParams();
        //BehaviourParams p = JsonUtility.FromJson(s, typeof(BehaviourParams)) as BehaviourParams;
        //BehaviourParams p = JsonUtility.FromJson<BehaviourParams>(s);
        p.Angle = 90;
        p.ForwardSpeed = 2f;
        p.BackwardSpeed = 1.5f;
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);

        behaviour.Stop();
        p.Angle = AngleFixer.FixAngle(p.Angle, CurrentExercise.Movement);
        p.BackwardSpeed *= SpeedFixer.FixSpeed(CurrentExercise.Movement);
        p.ForwardSpeed *= SpeedFixer.FixSpeed(CurrentExercise.Movement);
        StartCoroutine(RunWebInSeconds(0.4f, p));

    }
    private IEnumerator RunWebInSeconds(float time, BehaviourParams p)
    {
        yield return new WaitForSeconds(time);
        behaviour.RunWeb(p);

        if (p.Variations == null)
            RewindExercise();
        else
            CurrentExercise = new Exercise(behaviour.randomAnimations[0], CurrentExercise.Limb);
    }
    private IEnumerator RunWebInSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        behaviour.RunWeb();
        RewindExercise();
    }

    public void RunExerciseWebWithoutParams()
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.Stop();
        StartCoroutine(RunWebInSeconds(0.4f));
    }
    
    public void StopExercise()
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);

        if (behaviour == null)
            return;

        behaviour.Stop();

        behaviour.RepetitionEnd -= behaviour_RepetitionEnd;
        behaviour.RepetitionEnd -= behaviour_SubrepetitionEnd;
        behaviour.RepetitionEnd -= behaviour_PrepareEnd;
        behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
        behaviour.RepetitionReallyStart -= behaviour_RepetitionReallyStart;
    }


    /// <summary>
    /// Se ejecuta cuando termina una repetición
    /// </summary>
    private void AnimatorScript_OnRepetitionEnd(object sender, System.EventArgs e)
    {
        if (/*(CurrentExercise.Laterality == Laterality.Single) &&*/ (CurrentExercise.Limb == Limb.Interleaved))
        {
           // ChangeLimb();
        }
    }

    void RaiseEvent(EventHandler eh)
    {
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
    }
    
    void RaiseEvent(EventHandler<PrepareEventArgs> eh, PrepareStatus status)
    {
        if (eh != null)
        {
            eh(this, new PrepareEventArgs(status, this.prepareCaller));
        }
    }
    void RaiseEvent(EventHandler<RepetitionStartEventArgs> eh, RepetitionStartEventArgs e)
    {
        if (eh != null)
        {
            eh(this, e);
        }
    }
    IEnumerator RaiseEvent(EventHandler<PrepareEventArgs> eh, PrepareStatus status, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (eh != null)
        {
            eh(this, new PrepareEventArgs(status, this.prepareCaller));
        }
    }
    void OnDestroy()
    {

        //StopExercise();
        if (instance != null)
            instance.StopAllCoroutines();
        if (AnimatorScript.instance != null)
        {
            AnimatorScript.instance.OnRepetitionEnd -= AnimatorScript_OnRepetitionEnd;
        }
        StopAllCoroutines();
        CurrentExercise.PropertyChanged -= currentExercise_PropertyChanged;
        if (behaviour != null)
        {
            behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
            behaviour.RepetitionEnd -= behaviour_PrepareEnd;
            behaviour.RepetitionEnd -= behaviour_RepetitionEnd;
            behaviour.RepetitionEnd -= behaviour_SubrepetitionEnd;
            behaviour.RepetitionReallyStart -= behaviour_RepetitionReallyStart;
        }
        Destroy(instance);
        Destroy(this);
    }
}
