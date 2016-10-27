using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using System.IO;

namespace Assets.Script.NyshaRig
{
    [Serializable]
    public class RigPose:JsonSerializable
    {

        public const string DEFAULT_ASSET_PATH = "Assets/Animations/Poses/";

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

        public RigPose()
        {
        }

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

        public RigPose GetMirroredPose()
        {
            RigPose normalPose = this;

            RigPose mirroredPose = new RigPose();

            //mirroredPose = normalPose;
            //mirroredPose = normalPose;
            //mirroredPose.CenterOfMass.Position = normalPose.CenterOfMass.Position;
            mirroredPose.RootControl = normalPose.RootControl;
            mirroredPose.CenterOfMass = normalPose.CenterOfMass;
            mirroredPose.LookAtTarget = normalPose.LookAtTarget;
            // mirroredPose.CenterOfMass.Rotation = Quaternion.AngleAxis(180, Vector3.up);

            //mirroredPose.LeftHandIKTarget = normalPose.RightHandIKTarget;
            //mirroredPose.RightHandIKTarget = normalPose.LeftHandIKTarget;

            //mirroredPose.RightFootIKTarget = normalPose.LeftFootIKTarget;
            //mirroredPose.LeftFootIKTarget = normalPose.RightFootIKTarget;
            
            mirroredPose.LeftFootIKTarget = normalPose.RightFootIKTarget.Mirror();
            mirroredPose.LeftFootIKPole = normalPose.RightFootIKPole.Mirror();
            //((Transform)mirroredPose.LeftFootIKTarget).Rotate(Vector3.up, 180);

            mirroredPose.RightFootIKTarget = normalPose.LeftFootIKTarget.Mirror();
            mirroredPose.RightFootIKPole = normalPose.LeftFootIKPole.Mirror();

            mirroredPose.LeftHandIKTarget = normalPose.RightHandIKTarget.Mirror();
            mirroredPose.LeftHandIKPole = normalPose.RightHandIKPole.Mirror();

            mirroredPose.RightHandIKTarget = normalPose.LeftHandIKTarget.Mirror();
            mirroredPose.RightHandIKPole = normalPose.LeftHandIKPole.Mirror();
            // ((Transform)mirroredPose.RightFootIKTarget).Rotate(Vector3.up, 180);

            //mirroredPose.CenterOfMass.Rotation = normalPose.CenterOfMass.Rotation.AngleAxis(180, Vector3.up); Quaternion.
            //mirroredPose.CenterOfMass.Scale=

            return mirroredPose;
        }

        public static RigPose LoadFromFile(string fileName)
        {
            return JsonUtility.FromJson<RigPose>(LoadJsonFile(RigPose.DEFAULT_ASSET_PATH, fileName));
        }

        public override void SaveToFile(string fileName, string directoryPath)
        {
            base.SaveToFile(fileName, directoryPath);
        }

        public void SaveToFile(string fileName)
        {
            this.SaveToFile(fileName, DEFAULT_ASSET_PATH);
        }
    }
}
