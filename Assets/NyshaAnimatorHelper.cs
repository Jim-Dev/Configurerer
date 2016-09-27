using UnityEngine;
using System.Collections;

public class NyshaAnimatorHelper : MonoBehaviour
{
    private AnimatorOverrideController exerciseOverride;

    public AnimatorOverrideController ExerciseOverride
    {
        get { return this.exerciseOverride; }
        set
        {
            exerciseOverride = value;
            animator.runtimeAnimatorController = value;
        }
    }

    private Animator animator;
    private float totalDeltaTime;

    private eTransitionState currentTransitionState;
    private eTransitionState previousTransitionState;

    int restPoseHash = Animator.StringToHash("Rest");
    int extremePoseHash = Animator.StringToHash("Extreme");

    int transitionTimeHash = Animator.StringToHash("TransitionTime");
    int transitionAlphaHash = Animator.StringToHash("TransitionAlpha");

    int onRestTimeHash = Animator.StringToHash("OnRestTime");
    int onExtremeTimeHash = Animator.StringToHash("OnExtremeTime");

    int mirrorAnimHash = Animator.StringToHash("MirrorAnim");

    private float TransitionAlpha
    {
        get
        {
            return animator.GetFloat(transitionAlphaHash);
        }
        set
        {
            animator.SetFloat(transitionAlphaHash, Mathf.Clamp01(value));
        }
    }
    private float TransitionTime
    {
        get
        {
            return animator.GetFloat(transitionTimeHash);
        }
    }

    private float OnRestTime
    {
        get
        {
            return animator.GetFloat(onRestTimeHash);
        }
    }
    private float OnExtremeTime
    {
        get
        {
            return animator.GetFloat(onExtremeTimeHash);
        }
    }

    private bool MirrorAnim
    {
        get
        {
            return animator.GetBool(mirrorAnimHash);
        }
        set
        {
            animator.SetBool(mirrorAnimHash,value);
        }
    }

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        totalDeltaTime = 0;
        previousTransitionState = eTransitionState.None;
        currentTransitionState = eTransitionState.None;

        currentTransitionState = eTransitionState.OnRestToExtreme;


    }

    // Update is called once per frame
    void Update()
    {
        SetAnimator();
        
        if (TransitionAlpha<=0 && currentTransitionState == eTransitionState.OnExtremeToRest)
        {
            totalDeltaTime = 0;
            previousTransitionState = currentTransitionState;
            currentTransitionState = eTransitionState.OnRest;
        }
        else if (TransitionAlpha >= 1 && currentTransitionState == eTransitionState.OnRestToExtreme)
        {
            totalDeltaTime = 0;
            previousTransitionState = currentTransitionState;
            currentTransitionState = eTransitionState.OnExtreme;
        }

        switch (currentTransitionState)
        {
            case eTransitionState.None:
                break;
            case eTransitionState.OnRest:
                totalDeltaTime += Time.deltaTime;
                if (totalDeltaTime >= OnRestTime)
                {
                    totalDeltaTime = 0;
                    previousTransitionState = currentTransitionState;
                    currentTransitionState = eTransitionState.OnRestToExtreme;
                    MirrorAnim = !MirrorAnim;
                }
                break;
            case eTransitionState.OnRestToExtreme:
                totalDeltaTime += Time.deltaTime;
                TransitionAlpha = totalDeltaTime / TransitionTime;
                break;
            case eTransitionState.OnExtreme:
                totalDeltaTime += Time.deltaTime;
                if (totalDeltaTime>= OnExtremeTime)
                {
                    totalDeltaTime = TransitionTime;
                    previousTransitionState = currentTransitionState;
                    currentTransitionState = eTransitionState.OnExtremeToRest;
                    
                }
                //TransitionAlpha = totalDeltaTime / TransitionTime;
                break;
            case eTransitionState.OnExtremeToRest:
                totalDeltaTime -= Time.deltaTime;
                TransitionAlpha = totalDeltaTime / TransitionTime;
                break;
            default:
                break;
        }

        Debug.Log(TransitionAlpha);
        Debug.Log(currentTransitionState);
    }

    private void SetAnimator()
    {

        if (ExerciseOverride != null)
            animator.runtimeAnimatorController = ExerciseOverride;
    }

    private void OnRepFinished()
    {
        
    }

    private enum eTransitionState
    {
        None,
        OnRest,
        OnRestToExtreme,
        OnExtreme,
        OnExtremeToRest
    }
}