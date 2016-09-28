﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class NyshaAnimatorHelper : MonoBehaviour
{
    public AnimatorOverrideController ExerciseOverride;
    public AnimationCurve AnimCurve;
    /*
    public AnimatorOverrideController ExerciseOverride
    {
        get { return this.exerciseOverride; }
        set
        {
            exerciseOverride = value;
            animator.runtimeAnimatorController = value;
        }
    }*/

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

    [SerializeField, SetProperty("TransitionAlpha")]
    private float transitionAlpha = 0;
    public float TransitionAlpha
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
    [SerializeField, SetProperty("TransitionTime")]
    private float transitionTime=5;
    private float TransitionTime
    {
        get
        {
            return animator.GetFloat(transitionTimeHash);
        }
        set
        {
            this.transitionTime = value;
        }
    }

    public float Transition = 0;

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

    //----- IK

    public Transform LeftFootIKTarget;
    public Transform RightFootIKTarget;

    public Transform LeftHandIKTarget;
    public Transform RightHandIKTarget;

    public Transform LeftFootIKPivot;
    public Transform RightFootIKPivot;

    public Transform LeftHandIKPivot;
    public Transform RightHandIKPivot;

    public float IKWeight;


    //------ End IK





    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        totalDeltaTime = 0;
        previousTransitionState = eTransitionState.None;
        currentTransitionState = eTransitionState.None;

        currentTransitionState = eTransitionState.OnRestToExtreme;

        AnimCurve = new AnimationCurve();
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

    void OnAnimatorIK()
    {
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftFootIKTarget.position);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, RightFootIKTarget.position);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTarget.position);
        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTarget.position);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);



        animator.SetIKHintPosition(AvatarIKHint.LeftKnee, LeftFootIKPivot.position);
        animator.SetIKHintPosition(AvatarIKHint.RightKnee, RightFootIKPivot.position);
        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftHandIKPivot.position);
        animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightHandIKPivot.position);

        animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, IKWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, IKWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, IKWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, IKWeight);
    }
    private void SetPublicMembers()
    {
        Transition = TransitionAlpha;
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