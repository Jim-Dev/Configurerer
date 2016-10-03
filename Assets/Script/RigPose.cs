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
        
    }
}
