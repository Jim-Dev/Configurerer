using UnityEngine;
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
    [Header("IK Controls")]
    [Space(5)]
    public Transform LeftFootIKTarget;
    public Transform RightFootIKTarget;

    public Transform LeftHandIKTarget;
    public Transform RightHandIKTarget;

    public Transform LeftFootIKPivot;
    public Transform RightFootIKPivot;

    public Transform LeftHandIKPivot;
    public Transform RightHandIKPivot;

    public Transform LookAtTarget;
    [Space(20)]


    [Header("Inverse Kinematics Weights")]
    [Space(5)]
    [Range(0, 1)]
    public float IKBaseWeight;

    [Range(0, 1)]
    public float LookAtBaseWeight;
    [Range(0, 1)]
    public float LookAtBodyWeight;
    [Range(0, 1)]
    public float LookAtHeadWeight;
    [Range(0, 1)]
    public float LookAtEyesWeight;
    [Range(0, 1)]
    public float LookAtClampWeight;

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

        animator.SetIKRotation(AvatarIKGoal.LeftFoot, LeftFootIKTarget.rotation);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, RightFootIKTarget.rotation);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIKTarget.rotation);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandIKTarget.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKBaseWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKBaseWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKBaseWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKBaseWeight);

        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, IKBaseWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, IKBaseWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKBaseWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, IKBaseWeight);


        animator.SetIKHintPosition(AvatarIKHint.LeftKnee, LeftFootIKPivot.position);
        animator.SetIKHintPosition(AvatarIKHint.RightKnee, RightFootIKPivot.position);
        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftHandIKPivot.position);
        animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightHandIKPivot.position);

        animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, IKBaseWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, IKBaseWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, IKBaseWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, IKBaseWeight);

        animator.SetLookAtPosition(LookAtTarget.position);
        animator.SetLookAtWeight(LookAtBaseWeight,LookAtBodyWeight,LookAtHeadWeight,LookAtEyesWeight,LookAtClampWeight);
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