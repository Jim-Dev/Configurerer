using UnityEngine;
using System.Collections;

using Assets.Script.Excersice;
[RequireComponent(typeof(Animator))]
public class NyshaAnimatorHelper : MonoBehaviour
{
    public ExerciseDescriptor ExerciseDescriptor;

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

    Vector3 hipsOffset;
    Vector3 chestOffset;

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
    public Transform HipsControl;
    public Transform ChestControl;

    public Transform LeftFootIKTarget;
    public Transform RightFootIKTarget;

    public Transform LeftHandIKTarget;
    public Transform RightHandIKTarget;

    public Transform LeftFootIKPole;
    public Transform RightFootIKPole;

    public Transform LeftHandIKPole;
    public Transform RightHandIKPole;

    public Transform LookAtTarget;
    [Space(20)]


    [Header("Inverse Kinematics Weights")]
    [Space(5)]
    [Range(0, 1)]
    public float IKBaseWeight;

    [Space(5)]
    [Range(0, 1)]
    public float IKHandLeftPositionWeight;
    [Range(0, 1)]
    public float IKHandLeftRotationWeight;
    [Range(0, 1)]
    public float IKHandLeftPoleWeight;

    [Space(5)]
    [Range(0, 1)]
    public float IKHandRightPositionWeight;
    [Range(0, 1)]
    public float IKHandRightRotationWeight;
    [Range(0, 1)]
    public float IKHandRightPoleWeight;

    [Space(5)]
    [Range(0, 1)]
    public float IKFootLeftPositionWeight;
    [Range(0, 1)]
    public float IKFootLeftRotationWeight;
    [Range(0, 1)]
    public float IKFootLeftPoleWeight;

    [Space(5)]
    [Range(0, 1)]
    public float IKFootRightPositionWeight;
    [Range(0, 1)]
    public float IKFootRightRotationWeight;
    [Range(0, 1)]
    public float IKFootRightPoleWeight;


    [Space(10)]
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

        hipsOffset = HipsControl.localPosition;
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

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, NormalizeIKWeight(IKFootLeftPositionWeight));
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, NormalizeIKWeight(IKFootRightPositionWeight));
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, NormalizeIKWeight(IKHandLeftPositionWeight));
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, NormalizeIKWeight(IKHandRightPositionWeight));

        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, NormalizeIKWeight(IKFootLeftRotationWeight));
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, NormalizeIKWeight(IKFootRightRotationWeight));
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, NormalizeIKWeight(IKHandLeftRotationWeight));
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, NormalizeIKWeight(IKHandLeftRotationWeight));


        animator.SetIKHintPosition(AvatarIKHint.LeftKnee, LeftFootIKPole.position);
        animator.SetIKHintPosition(AvatarIKHint.RightKnee, RightFootIKPole.position);
        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftHandIKPole.position);
        animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightHandIKPole.position);

        animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, NormalizeIKWeight(IKFootLeftPoleWeight));
        animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, NormalizeIKWeight(IKFootRightPoleWeight));
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, NormalizeIKWeight(IKHandLeftPoleWeight));
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, NormalizeIKWeight(IKHandLeftPoleWeight));

        animator.SetLookAtPosition(LookAtTarget.position);
        animator.SetLookAtWeight(LookAtBaseWeight,LookAtBodyWeight,LookAtHeadWeight,LookAtEyesWeight,LookAtClampWeight);

        gameObject.transform.position = HipsControl.position-hipsOffset;
        animator.SetBoneLocalRotation(HumanBodyBones.Hips, HipsControl.localRotation);

        ChestControl.position = animator.GetBoneTransform(HumanBodyBones.Chest).position;

        animator.SetBoneLocalRotation(HumanBodyBones.Chest, ChestControl.localRotation);

    }

    private float NormalizeIKWeight(float IKWeight)
    {
        return Mathf.Clamp01(IKBaseWeight + IKWeight);
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