using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class RigHelper : MonoBehaviour
    {

        private Animator animator;
        private Vector3 hipsOffset;
        private Vector3 chestOffset;

        public bool RenderOnGame = false;

        [Header("IK Reference Lines")]
        [Space(5)]
        public Color IKLineReferenceColor;
        public bool DrawDebugReferenceLines = true;
        public bool DrawGLReferenceLines;
        [Space(10)]

        //----- IK
        [Header("IK Controls")]
        [Space(5)]
        public RigSetup RigSetup;
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
            hipsOffset = RigSetup.HipsControl.localPosition;

        }

        // Update is called once per frame
        void Update()
        {
            RigSetup.LookAtTarget.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.HeadControl.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.NeckControl.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.ChestControl.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.HipsControl.GetComponent<Renderer>().enabled = RenderOnGame;


            RigSetup.LeftFootIKTarget.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.RightFootIKTarget.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.LeftHandIKTarget.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.RightHandIKTarget.GetComponent<Renderer>().enabled = RenderOnGame;

            RigSetup.LeftFootIKPole.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.RightFootIKPole.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.LeftHandIKPole.GetComponent<Renderer>().enabled = RenderOnGame;
            RigSetup.RightHandIKPole.GetComponent<Renderer>().enabled = RenderOnGame;
            

            if (RenderOnGame)
            {
                DrawDebugLines();
                DrawReferenceLines();
            }
        }


        void OnAnimatorIK()
        {
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, RigSetup.LeftFootIKTarget.position);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, RigSetup.RightFootIKTarget.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, RigSetup.LeftHandIKTarget.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, RigSetup.RightHandIKTarget.position);

            animator.SetIKRotation(AvatarIKGoal.LeftFoot, RigSetup.LeftFootIKTarget.rotation);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, RigSetup.RightFootIKTarget.rotation);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, RigSetup.LeftHandIKTarget.rotation);
            animator.SetIKRotation(AvatarIKGoal.RightHand, RigSetup.RightHandIKTarget.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, NormalizeIKWeight(IKFootLeftPositionWeight));
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, NormalizeIKWeight(IKFootRightPositionWeight));
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, NormalizeIKWeight(IKHandLeftPositionWeight));
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, NormalizeIKWeight(IKHandRightPositionWeight));

            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, NormalizeIKWeight(IKFootLeftRotationWeight));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, NormalizeIKWeight(IKFootRightRotationWeight));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, NormalizeIKWeight(IKHandLeftRotationWeight));
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, NormalizeIKWeight(IKHandRightRotationWeight));


            animator.SetIKHintPosition(AvatarIKHint.LeftKnee, RigSetup.LeftFootIKPole.position);
            animator.SetIKHintPosition(AvatarIKHint.RightKnee, RigSetup.RightFootIKPole.position);
            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, RigSetup.LeftHandIKPole.position);
            animator.SetIKHintPosition(AvatarIKHint.RightElbow, RigSetup.RightHandIKPole.position);

            animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, NormalizeIKWeight(IKFootLeftPoleWeight));
            animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, NormalizeIKWeight(IKFootRightPoleWeight));
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, NormalizeIKWeight(IKHandLeftPoleWeight));
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, NormalizeIKWeight(IKHandRightPoleWeight));

            animator.SetLookAtPosition(RigSetup.LookAtTarget.position);
            animator.SetLookAtWeight(LookAtBaseWeight, LookAtBodyWeight, LookAtHeadWeight, LookAtEyesWeight, LookAtClampWeight);

            gameObject.transform.position = RigSetup.HipsControl.position - hipsOffset;
            animator.SetBoneLocalRotation(HumanBodyBones.Hips, RigSetup.HipsControl.localRotation);

            RigSetup.ChestControl.position = animator.GetBoneTransform(HumanBodyBones.Chest).position;
            animator.SetBoneLocalRotation(HumanBodyBones.Chest, RigSetup.ChestControl.localRotation);

            RigSetup.HeadControl.position = animator.GetBoneTransform(HumanBodyBones.Head).position;
            animator.SetBoneLocalRotation(HumanBodyBones.Head, RigSetup.HeadControl.localRotation);

            RigSetup.NeckControl.position = animator.GetBoneTransform(HumanBodyBones.Neck).position;
            animator.SetBoneLocalRotation(HumanBodyBones.Neck, RigSetup.NeckControl.localRotation);

        }

        private float NormalizeIKWeight(float IKWeight)
        {
            return Mathf.Clamp01(IKBaseWeight * IKWeight);
        }


        private void DrawDebugLines()
        {
            if (DrawDebugReferenceLines)
            {
                Debug.DrawLine(RigSetup.LeftHandIKTarget.position, animator.GetBoneTransform(HumanBodyBones.LeftHand).position, IKLineReferenceColor);
                Debug.DrawLine(RigSetup.RightHandIKTarget.position, animator.GetBoneTransform(HumanBodyBones.RightHand).position, IKLineReferenceColor);
                Debug.DrawLine(RigSetup.LeftFootIKTarget.position, animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, IKLineReferenceColor);
                Debug.DrawLine(RigSetup.RightFootIKTarget.position, animator.GetBoneTransform(HumanBodyBones.RightFoot).position, IKLineReferenceColor);


                Debug.DrawLine(RigSetup.LeftHandIKPole.position, animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).position, IKLineReferenceColor);
                Debug.DrawLine(RigSetup.RightHandIKPole.position, animator.GetBoneTransform(HumanBodyBones.RightLowerArm).position, IKLineReferenceColor);
                Debug.DrawLine(RigSetup.LeftFootIKPole.position, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position, IKLineReferenceColor);
                Debug.DrawLine(RigSetup.RightFootIKPole.position, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position, IKLineReferenceColor);
            }
        }

        private void DrawReferenceLines()
        {


            if (DrawGLReferenceLines)
            {
                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RigSetup.LeftHandIKTarget.position.x, RigSetup.LeftHandIKTarget.position.y, RigSetup.LeftHandIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.x,
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.y,
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RigSetup.RightHandIKTarget.position.x, RigSetup.RightHandIKTarget.position.y, RigSetup.RightHandIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.x,
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.y,
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RigSetup.LeftFootIKTarget.position.x, RigSetup.LeftFootIKTarget.position.y, RigSetup.LeftFootIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.x,
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.y,
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RigSetup.RightFootIKTarget.position.x, RigSetup.RightFootIKTarget.position.y, RigSetup.RightFootIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.RightFoot).position.x,
                    animator.GetBoneTransform(HumanBodyBones.RightFoot).position.y,
                    animator.GetBoneTransform(HumanBodyBones.RightFoot).position.z
                    );
                GL.End();
            }

        }
        void OnPostRender()
        {
            DrawReferenceLines();
        }

        // To show the lines in the editor
        void OnDrawGizmos()
        {
            DrawReferenceLines();
        }


        public string ToJson()
        {
            return ToJson(true);
        }
        public string ToJson(bool prettyPrint)
        {
            return JsonUtility.ToJson(this, prettyPrint);
        }

        /*
        public ExerciseDescriptor FromJson(string JsonString)
        {
            return JsonUtility.FromJson<ExerciseDescriptor>(JsonString);
        }
        */
    }
}
