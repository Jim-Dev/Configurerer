using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class RigAnimator : MonoBehaviour
    {

        private Animator animator;

        private Vector3 CenterOfMassOffset;

        private RigSetup RigConfig;
        private IKSetup IkConfig;

        // Use this for initialization
        void Start()
        {

            CenterOfMassOffset = RigConfig.CenterOfMass.localPosition;
            IkConfig.OnSnapIKToFK += IkConfig_OnSnapIKToFK;
        }

        void Awake()
        {
            animator = GetComponentInParent<Animator>();
            RigConfig = GetComponentInParent<RigSetup>();
            IkConfig = GetComponentInParent<IKSetup>();
        }
        private void IkConfig_OnSnapIKToFK(object sender, EventArgs e)
        {
            Vector3 tmpLookAtPos = RigConfig.LookAtTarget.position;

            RigConfig.ChestControl.position = animator.GetBoneTransform(HumanBodyBones.Chest).position;
            RigConfig.ChestControl.rotation = animator.GetBoneTransform(HumanBodyBones.Chest).rotation;

            RigConfig.HeadControl.position = animator.GetBoneTransform(HumanBodyBones.Head).position;
            RigConfig.HeadControl.rotation = animator.GetBoneTransform(HumanBodyBones.Head).rotation;

            RigConfig.NeckControl.position = animator.GetBoneTransform(HumanBodyBones.Neck).position;
            RigConfig.NeckControl.rotation = animator.GetBoneTransform(HumanBodyBones.Neck).rotation;

            RigConfig.LookAtTarget.position = tmpLookAtPos;
        }


        // Update is called once per frame
        void Update()
        {
            RigConfig.RootControl.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.CenterOfMass.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;

            RigConfig.LookAtTarget.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.HeadControl.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.NeckControl.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.ChestControl.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.HipsControl.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;


            RigConfig.LeftFootIKTarget.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.RightFootIKTarget.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.LeftHandIKTarget.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.RightHandIKTarget.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;

            RigConfig.LeftFootIKPole.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.RightFootIKPole.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.LeftHandIKPole.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;
            RigConfig.RightHandIKPole.GetComponent<Renderer>().enabled = IkConfig.RenderOnGame;


            if (IkConfig.RenderOnGame)
            {
                DrawDebugLines();
                DrawReferenceLines();
            }

        }

        void OnAnimatorIK()
        {
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, RigConfig.LeftFootIKTarget.position);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, RigConfig.RightFootIKTarget.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, RigConfig.LeftHandIKTarget.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, RigConfig.RightHandIKTarget.position);

            animator.SetIKRotation(AvatarIKGoal.LeftFoot, RigConfig.LeftFootIKTarget.rotation);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, RigConfig.RightFootIKTarget.rotation);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, RigConfig.LeftHandIKTarget.rotation);
            animator.SetIKRotation(AvatarIKGoal.RightHand, RigConfig.RightHandIKTarget.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IkConfig.NormalizeIKWeight(IkConfig.IKFootLeftPositionWeight));
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IkConfig.NormalizeIKWeight(IkConfig.IKFootRightPositionWeight));
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IkConfig.NormalizeIKWeight(IkConfig.IKHandLeftPositionWeight));
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IkConfig.NormalizeIKWeight(IkConfig.IKHandRightPositionWeight));

            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, IkConfig.NormalizeIKWeight(IkConfig.IKFootLeftRotationWeight));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, IkConfig.NormalizeIKWeight(IkConfig.IKFootRightRotationWeight));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IkConfig.NormalizeIKWeight(IkConfig.IKHandLeftRotationWeight));
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, IkConfig.NormalizeIKWeight(IkConfig.IKHandRightRotationWeight));


            animator.SetIKHintPosition(AvatarIKHint.LeftKnee, RigConfig.LeftFootIKPole.position);
            animator.SetIKHintPosition(AvatarIKHint.RightKnee, RigConfig.RightFootIKPole.position);
            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, RigConfig.LeftHandIKPole.position);
            animator.SetIKHintPosition(AvatarIKHint.RightElbow, RigConfig.RightHandIKPole.position);

            animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, IkConfig.NormalizeIKWeight(IkConfig.IKFootLeftPoleWeight));
            animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, IkConfig.NormalizeIKWeight(IkConfig.IKFootRightPoleWeight));
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, IkConfig.NormalizeIKWeight(IkConfig.IKHandLeftPoleWeight));
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, IkConfig.NormalizeIKWeight(IkConfig.IKHandRightPoleWeight));

            animator.SetLookAtPosition(RigConfig.LookAtTarget.position);
            animator.SetLookAtWeight(IkConfig.LookAtBaseWeight, IkConfig.LookAtBodyWeight, IkConfig.LookAtHeadWeight, IkConfig.LookAtEyesWeight, IkConfig.LookAtClampWeight);

            RigConfig.SKRoot.localPosition = RigConfig.CenterOfMass.localPosition - CenterOfMassOffset;
            RigConfig.SKRoot.rotation = RigConfig.CenterOfMass.rotation;// - CenterOfMassOffset;



            //RigConfig.ChestControl.localPosition = animator.GetBoneTransform(HumanBodyBones.Chest).localPosition;
            //animator.SetBoneLocalRotation(HumanBodyBones.Chest, RigConfig.ChestControl.localRotation);

            //RigConfig.HeadControl.localPosition = animator.GetBoneTransform(HumanBodyBones.Head).localPosition;
            //animator.SetBoneLocalRotation(HumanBodyBones.Head, RigConfig.HeadControl.localRotation);

            //RigConfig.NeckControl.localPosition = animator.GetBoneTransform(HumanBodyBones.Neck).localPosition;
            //animator.SetBoneLocalRotation(HumanBodyBones.Neck, RigConfig.NeckControl.localRotation);

        }

        private void DrawDebugLines()
        {
            if (IkConfig != null && IkConfig.DrawDebugReferenceLines)
            {
                Debug.DrawLine(RigConfig.LeftHandIKTarget.position, animator.GetBoneTransform(HumanBodyBones.LeftHand).position, IkConfig.IKLineReferenceColor);
                Debug.DrawLine(RigConfig.RightHandIKTarget.position, animator.GetBoneTransform(HumanBodyBones.RightHand).position, IkConfig.IKLineReferenceColor);
                Debug.DrawLine(RigConfig.LeftFootIKTarget.position, animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, IkConfig.IKLineReferenceColor);
                Debug.DrawLine(RigConfig.RightFootIKTarget.position, animator.GetBoneTransform(HumanBodyBones.RightFoot).position, IkConfig.IKLineReferenceColor);


                Debug.DrawLine(RigConfig.LeftHandIKPole.position, animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).position, IkConfig.IKLineReferenceColor);
                Debug.DrawLine(RigConfig.RightHandIKPole.position, animator.GetBoneTransform(HumanBodyBones.RightLowerArm).position, IkConfig.IKLineReferenceColor);
                Debug.DrawLine(RigConfig.LeftFootIKPole.position, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position, IkConfig.IKLineReferenceColor);
                Debug.DrawLine(RigConfig.RightFootIKPole.position, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position, IkConfig.IKLineReferenceColor);
            }
        }

        private void DrawReferenceLines()
        {


            if (IkConfig != null && IkConfig.DrawGLReferenceLines)
            {
                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RigConfig.LeftHandIKTarget.position.x, RigConfig.LeftHandIKTarget.position.y, RigConfig.LeftHandIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.x,
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.y,
                    animator.GetBoneTransform(HumanBodyBones.LeftHand).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RigConfig.RightHandIKTarget.position.x, RigConfig.RightHandIKTarget.position.y, RigConfig.RightHandIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.x,
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.y,
                    animator.GetBoneTransform(HumanBodyBones.RightHand).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RigConfig.LeftFootIKTarget.position.x, RigConfig.LeftFootIKTarget.position.y, RigConfig.LeftFootIKTarget.position.z);
                GL.Vertex3(
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.x,
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.y,
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.z
                    );
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(new Color(0, 1, 1));
                GL.Vertex3(RigConfig.RightFootIKTarget.position.x, RigConfig.RightFootIKTarget.position.y, RigConfig.RightFootIKTarget.position.z);
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
