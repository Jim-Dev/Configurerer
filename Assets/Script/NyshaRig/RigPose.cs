using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
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




        

        public SerializableTransform RootControl;
        public SerializableTransform CenterOfMass;

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

            rigPose.RootControl = rig.RootControl;
            rigPose.CenterOfMass = rig.CenterOfMass;

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
            rigPose.RightFootIKPole = rig.RightFootIKPole.transform;

            return rigPose;
        }

        public static RigPose Lerp(RigPose A,RigPose B,float Alpha)
        {
            RigPose rigPose = new RigPose();

            rigPose.RootControl = SerializableTransform.Lerp(A.RootControl, B.RootControl, Alpha);
            rigPose.CenterOfMass = SerializableTransform.Lerp(A.CenterOfMass, B.CenterOfMass, Alpha);

            rigPose.LookAtTarget = SerializableTransform.Lerp(A.LookAtTarget, B.LookAtTarget, Alpha);
            rigPose.HeadControl = SerializableTransform.Lerp(A.HeadControl, B.HeadControl, Alpha);
            rigPose.NeckControl = SerializableTransform.Lerp(A.NeckControl, B.NeckControl, Alpha);
            rigPose.ChestControl = SerializableTransform.Lerp(A.ChestControl, B.ChestControl, Alpha);
            rigPose.HipsControl = SerializableTransform.Lerp(A.HipsControl, B.HipsControl, Alpha);

            rigPose.LeftHandIKTarget = SerializableTransform.Lerp(A.LeftHandIKTarget, B.LeftHandIKTarget, Alpha);
            rigPose.RightHandIKTarget = SerializableTransform.Lerp(A.RightHandIKTarget, B.RightHandIKTarget, Alpha);
            rigPose.LeftHandIKPole = SerializableTransform.Lerp(A.LeftHandIKPole, B.LeftHandIKPole, Alpha);
            rigPose.RightHandIKPole = SerializableTransform.Lerp(A.RightHandIKPole, B.RightHandIKPole, Alpha);

            rigPose.LeftFootIKTarget = SerializableTransform.Lerp(A.LeftFootIKTarget, B.LeftFootIKTarget, Alpha);
            rigPose.RightFootIKTarget = SerializableTransform.Lerp(A.RightFootIKTarget, B.RightFootIKTarget, Alpha);
            rigPose.LeftFootIKPole = SerializableTransform.Lerp(A.LeftFootIKPole, B.LeftFootIKPole, Alpha);
            rigPose.RightFootIKPole = SerializableTransform.Lerp(A.RightFootIKPole, B.RightFootIKPole, Alpha);

            return rigPose;
        }

    }
}
