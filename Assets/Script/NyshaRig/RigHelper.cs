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
            hipsOffset = HipsControl.localPosition;

        }

        // Update is called once per frame
        void Update()
        {
            LookAtTarget.GetComponent<Renderer>().enabled = RenderOnGame;
            ChestControl.GetComponent<Renderer>().enabled = RenderOnGame;
            HipsControl.GetComponent<Renderer>().enabled = RenderOnGame;
           

            LeftFootIKTarget.GetComponent<Renderer>().enabled = RenderOnGame;
            RightFootIKTarget.GetComponent<Renderer>().enabled = RenderOnGame;
            LeftHandIKTarget.GetComponent<Renderer>().enabled = RenderOnGame;
            RightHandIKTarget.GetComponent<Renderer>().enabled = RenderOnGame;

            LeftFootIKPole.GetComponent<Renderer>().enabled = RenderOnGame;
            RightFootIKPole.GetComponent<Renderer>().enabled = RenderOnGame;
            LeftHandIKPole.GetComponent<Renderer>().enabled = RenderOnGame;
            RightHandIKPole.GetComponent<Renderer>().enabled = RenderOnGame;
            

            if (RenderOnGame)
            {
                DrawDebugLines();
                DrawReferenceLines();
            }
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
            animator.SetLookAtWeight(LookAtBaseWeight, LookAtBodyWeight, LookAtHeadWeight, LookAtEyesWeight, LookAtClampWeight);

            gameObject.transform.position = HipsControl.position - hipsOffset;
            animator.SetBoneLocalRotation(HumanBodyBones.Hips, HipsControl.localRotation);

            ChestControl.position = animator.GetBoneTransform(HumanBodyBones.Chest).position;
            animator.SetBoneLocalRotation(HumanBodyBones.Chest, ChestControl.localRotation);


        }

        private float NormalizeIKWeight(float IKWeight)
        {
            return Mathf.Clamp01(IKBaseWeight * IKWeight);
        }


        private void DrawDebugLines()
        {
            if (DrawDebugReferenceLines)
            {
                Debug.DrawLine(LeftHandIKTarget.position, animator.GetBoneTransform(HumanBodyBones.LeftHand).position, IKLineReferenceColor);
                Debug.DrawLine(RightHandIKTarget.position, animator.GetBoneTransform(HumanBodyBones.RightHand).position, IKLineReferenceColor);
                Debug.DrawLine(LeftFootIKTarget.position, animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, IKLineReferenceColor);
                Debug.DrawLine(LeftFootIKTarget.position, animator.GetBoneTransform(HumanBodyBones.RightFoot).position, IKLineReferenceColor);


                Debug.DrawLine(LeftHandIKPole.position, animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).position, IKLineReferenceColor);
                Debug.DrawLine(RightHandIKPole.position, animator.GetBoneTransform(HumanBodyBones.RightLowerArm).position, IKLineReferenceColor);
                Debug.DrawLine(LeftFootIKPole.position, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position, IKLineReferenceColor);
                Debug.DrawLine(RightFootIKPole.position, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position, IKLineReferenceColor);
            }
        }

        private void DrawReferenceLines()
        {


            if (DrawGLReferenceLines)
            {
                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(LeftHandIKTarget.position.x, LeftHandIKTarget.position.y, LeftHandIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.x,
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.y,
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RightHandIKTarget.position.x, RightHandIKTarget.position.y, RightHandIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.x,
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.y,
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(LeftFootIKTarget.position.x, LeftFootIKTarget.position.y, LeftFootIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.x,
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.y,
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RightFootIKTarget.position.x, RightFootIKTarget.position.y, RightFootIKTarget.position.z);
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

    }
}
