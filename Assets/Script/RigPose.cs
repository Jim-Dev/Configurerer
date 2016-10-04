using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script
{
    [Serializable]
    public class RigPose
    {
        public static string PosesPath
        {
            get
            {
#if UNITY_EDITOR
                return "Assets/Animations/Poses/";
#endif

#if UNITY_STANDALONE
                return "Assets/Animations/Poses/";
#endif
            }
        }

        public string PoseName;

        public SerializableTransform LookAtTarget;
        public SerializableTransform HeadControl;
        public SerializableTransform NeckControl;
        public SerializableTransform ChestControl;
        public SerializableTransform HipsControl;

        public SerializableTransform LeftHandIKTarget;
        public SerializableTransform RightHandIKTarget;

        public SerializableTransform LeftFootIKTarget;
        public SerializableTransform RightFootIKTarget;

        public SerializableTransform LeftHandIKPole;
        public SerializableTransform RightHandIKPole;

        public SerializableTransform LeftFootIKPole;
        public SerializableTransform RightFootIKPole;

        public string ToJson()
        {
            return ToJson(true);
        }
        public string ToJson(bool prettyPrint)
        {
            return JsonUtility.ToJson(this, prettyPrint);
        }

        public static RigPose FromJson(string JsonString)
        {
            return JsonUtility.FromJson<RigPose>(JsonString);
        }

        public static RigPose FromRig(RigSetup rig)
        {
            RigPose rigPose = new RigPose();

            rigPose.LookAtTarget = rig.LookAtTarget.transform;
            rigPose.HeadControl = rig.HeadControl.transform;
            rigPose.NeckControl = rig.NeckControl.transform;
            rigPose.ChestControl = rig.ChestControl.transform;
            rigPose.HipsControl = rig.HipsControl.transform;

            rigPose.LeftHandIKTarget = rig.LeftHandIKTarget.transform;
            rigPose.RightHandIKTarget = rig.RightHandIKTarget.transform;

            rigPose.LeftHandIKPole = rig.LeftHandIKPole.transform;
            rigPose.RightHandIKPole = rig.RightHandIKPole.transform;

            rigPose.LeftFootIKTarget = rig.LeftFootIKTarget.transform;
            rigPose.RightFootIKTarget = rig.RightFootIKTarget.transform;

            rigPose.LeftFootIKPole = rig.LeftFootIKPole.transform;
            rigPose.LeftFootIKPole = rig.LeftFootIKPole.transform;

            return rigPose;
        }

        public static RigPose Lerp(RigPose A,RigPose B,float Alpha)
        {
            RigPose rigPose = new RigPose();

            rigPose.LookAtTarget.Position = Vector3.Lerp(A.LookAtTarget.Position, B.LookAtTarget.Position, Alpha);
            rigPose.LookAtTarget.Rotation = Quaternion.Lerp(A.LookAtTarget.Rotation, B.LookAtTarget.Rotation, Alpha);
            rigPose.LookAtTarget.Scale = Vector3.Lerp(A.LookAtTarget.Scale, B.LookAtTarget.Scale, Alpha);

            rigPose.HeadControl.Position = Vector3.Lerp(A.HeadControl.Position, B.HeadControl.Position, Alpha);
            rigPose.HeadControl.Rotation = Quaternion.Lerp(A.HeadControl.Rotation, B.HeadControl.Rotation, Alpha);
            rigPose.HeadControl.Scale = Vector3.Lerp(A.HeadControl.Scale, B.HeadControl.Scale, Alpha);

            rigPose.NeckControl.Position = Vector3.Lerp(A.NeckControl.Position, B.NeckControl.Position, Alpha);
            rigPose.NeckControl.Rotation = Quaternion.Lerp(A.NeckControl.Rotation, B.NeckControl.Rotation, Alpha);
            rigPose.NeckControl.Scale = Vector3.Lerp(A.NeckControl.Scale, B.NeckControl.Scale, Alpha);

            rigPose.ChestControl.Position = Vector3.Lerp(A.ChestControl.Position, B.ChestControl.Position, Alpha);
            rigPose.ChestControl.Rotation = Quaternion.Lerp(A.ChestControl.Rotation, B.ChestControl.Rotation, Alpha);
            rigPose.ChestControl.Scale = Vector3.Lerp(A.ChestControl.Scale, B.ChestControl.Scale, Alpha);

            rigPose.HipsControl.Position = Vector3.Lerp(A.HipsControl.Position, B.HipsControl.Position, Alpha);
            rigPose.HipsControl.Rotation = Quaternion.Lerp(A.HipsControl.Rotation, B.HipsControl.Rotation, Alpha);
            rigPose.HipsControl.Scale = Vector3.Lerp(A.HipsControl.Scale, B.HipsControl.Scale, Alpha);

            rigPose.LeftHandIKTarget.Position = Vector3.Lerp(A.LeftHandIKTarget.Position, B.LeftHandIKTarget.Position, Alpha);
            rigPose.LeftHandIKTarget.Rotation = Quaternion.Lerp(A.LeftHandIKTarget.Rotation, B.LeftHandIKTarget.Rotation, Alpha);
            rigPose.LeftHandIKTarget.Scale = Vector3.Lerp(A.LeftHandIKTarget.Scale, B.LeftHandIKTarget.Scale, Alpha);

            rigPose.RightHandIKTarget.Position = Vector3.Lerp(A.RightHandIKTarget.Position, B.RightHandIKTarget.Position, Alpha);
            rigPose.RightHandIKTarget.Rotation = Quaternion.Lerp(A.RightHandIKTarget.Rotation, B.RightHandIKTarget.Rotation, Alpha);
            rigPose.RightHandIKTarget.Scale = Vector3.Lerp(A.RightHandIKTarget.Scale, B.RightHandIKTarget.Scale, Alpha);

            rigPose.LeftHandIKPole.Position = Vector3.Lerp(A.LeftHandIKPole.Position, B.LeftHandIKPole.Position, Alpha);
            rigPose.LeftHandIKPole.Rotation = Quaternion.Lerp(A.LeftHandIKPole.Rotation, B.LeftHandIKPole.Rotation, Alpha);
            rigPose.LeftHandIKPole.Scale = Vector3.Lerp(A.LeftHandIKPole.Scale, B.LeftHandIKPole.Scale, Alpha);

            rigPose.RightHandIKPole.Position = Vector3.Lerp(A.RightHandIKPole.Position, B.RightHandIKPole.Position, Alpha);
            rigPose.RightHandIKPole.Rotation = Quaternion.Lerp(A.RightHandIKPole.Rotation, B.RightHandIKPole.Rotation, Alpha);
            rigPose.RightHandIKPole.Scale = Vector3.Lerp(A.RightHandIKPole.Scale, B.RightHandIKPole.Scale, Alpha);


            rigPose.LeftFootIKTarget.Position = Vector3.Lerp(A.LeftFootIKTarget.Position, B.LeftFootIKTarget.Position, Alpha);
            rigPose.LeftFootIKTarget.Rotation = Quaternion.Lerp(A.LeftFootIKTarget.Rotation, B.LeftFootIKTarget.Rotation, Alpha);
            rigPose.LeftFootIKTarget.Scale = Vector3.Lerp(A.LeftFootIKTarget.Scale, B.LeftFootIKTarget.Scale, Alpha);

            rigPose.RightFootIKTarget.Position = Vector3.Lerp(A.RightFootIKTarget.Position, B.RightFootIKTarget.Position, Alpha);
            rigPose.RightFootIKTarget.Rotation = Quaternion.Lerp(A.RightFootIKTarget.Rotation, B.RightFootIKTarget.Rotation, Alpha);
            rigPose.RightFootIKTarget.Scale = Vector3.Lerp(A.RightFootIKTarget.Scale, B.RightFootIKTarget.Scale, Alpha);

            rigPose.LeftFootIKPole.Position = Vector3.Lerp(A.LeftFootIKPole.Position, B.LeftFootIKPole.Position, Alpha);
            rigPose.LeftFootIKPole.Rotation = Quaternion.Lerp(A.LeftFootIKPole.Rotation, B.LeftFootIKPole.Rotation, Alpha);
            rigPose.LeftFootIKPole.Scale = Vector3.Lerp(A.LeftFootIKPole.Scale, B.LeftFootIKPole.Scale, Alpha);

            rigPose.LeftFootIKPole.Position = Vector3.Lerp(A.LeftFootIKPole.Position, B.LeftFootIKPole.Position, Alpha);
            rigPose.LeftFootIKPole.Rotation = Quaternion.Lerp(A.LeftFootIKPole.Rotation, B.LeftFootIKPole.Rotation, Alpha);
            rigPose.LeftFootIKPole.Scale = Vector3.Lerp(A.LeftFootIKPole.Scale, B.LeftFootIKPole.Scale, Alpha);


            return rigPose;
        }

    }
}
