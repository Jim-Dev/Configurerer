using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
namespace Assets.Script
{
    [Serializable]
    public class RigSetup:MonoBehaviour
    {
        //----- IK
        [Header("IK Controls")]
        [Space(5)]
        public Transform LookAtTarget;
        public Transform HeadControl;
        public Transform NeckControl;
        public Transform ChestControl;
        public Transform HipsControl;

        public Transform LeftHandIKTarget;
        public Transform RightHandIKTarget;

        public Transform LeftFootIKTarget;
        public Transform RightFootIKTarget;

        public Transform LeftHandIKPole;
        public Transform RightHandIKPole;

        public Transform LeftFootIKPole;
        public Transform RightFootIKPole;

        public void SetFromPose(RigPose pose)
        {



            LookAtTarget.transform.position = pose.LookAtTarget.Position;

            LookAtTarget.transform.position = pose.LookAtTarget.Position;
            LookAtTarget.transform.rotation = pose.LookAtTarget.Rotation;
            LookAtTarget.transform.localScale = pose.LookAtTarget.Scale;

            HeadControl.transform.position = pose.HeadControl.Position;
            HeadControl.transform.rotation = pose.HeadControl.Rotation;
            HeadControl.transform.localScale = pose.HeadControl.Scale;

            NeckControl.transform.position = pose.NeckControl.Position;
            NeckControl.transform.rotation = pose.NeckControl.Rotation;
            NeckControl.transform.localScale = pose.NeckControl.Scale;

            ChestControl.transform.position = pose.ChestControl.Position;
            ChestControl.transform.rotation = pose.ChestControl.Rotation;
            ChestControl.transform.localScale = pose.ChestControl.Scale;

            HipsControl.transform.position = pose.HipsControl.Position;
            HipsControl.transform.rotation = pose.HipsControl.Rotation;
            HipsControl.transform.localScale = pose.HipsControl.Scale;


            LeftHandIKTarget.transform.position = pose.LeftHandIKTarget.Position;
            LeftHandIKTarget.transform.rotation = pose.LeftHandIKTarget.Rotation;
            LeftHandIKTarget.transform.localScale = pose.LeftHandIKTarget.Scale;

            RightHandIKTarget.transform.position = pose.RightHandIKTarget.Position;
            RightHandIKTarget.transform.rotation = pose.RightHandIKTarget.Rotation;
            RightHandIKTarget.transform.localScale = pose.RightHandIKTarget.Scale;

            LeftFootIKTarget.transform.position = pose.LeftFootIKTarget.Position;
            LeftFootIKTarget.transform.rotation = pose.LeftFootIKTarget.Rotation;
            LeftFootIKTarget.transform.localScale = pose.LeftFootIKTarget.Scale;

            RightFootIKTarget.transform.position = pose.RightFootIKTarget.Position;
            RightFootIKTarget.transform.rotation = pose.RightFootIKTarget.Rotation;
            RightFootIKTarget.transform.localScale = pose.RightFootIKTarget.Scale;

            LeftHandIKPole.transform.position = pose.LeftHandIKPole.Position;
            LeftHandIKPole.transform.rotation = pose.LeftHandIKPole.Rotation;
            LeftHandIKPole.transform.localScale = pose.LeftHandIKPole.Scale;

            RightHandIKPole.transform.position = pose.RightHandIKPole.Position;
            RightHandIKPole.transform.rotation = pose.RightHandIKPole.Rotation;
            RightHandIKPole.transform.localScale = pose.RightHandIKPole.Scale;

            LeftFootIKPole.transform.position = pose.LeftFootIKPole.Position;
            LeftFootIKPole.transform.rotation = pose.LeftFootIKPole.Rotation;
            LeftFootIKPole.transform.localScale = pose.LeftFootIKPole.Scale;

            RightFootIKPole.transform.position = pose.RightFootIKPole.Position;
            RightFootIKPole.transform.rotation = pose.RightFootIKPole.Rotation;
            RightFootIKPole.transform.localScale = pose.RightFootIKPole.Scale;
        }
    }
}
