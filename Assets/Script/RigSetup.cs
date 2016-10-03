using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
namespace Assets.Script
{
    [Serializable]
    public class RigSetup
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
