using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.NyshaRig.Excersice
{
    [Serializable]
    public class ExerciseInfo//:MonoBehaviour
    {
        public string Name;
        public string Description;

        public Guid UniqueID;


        public float WaitTimeWarmUp;
        public float WaitTimeOnRest;
        public float WaitTimeOnExtreme;

        public float TransitionTimeRestToExtreme;
        public float TransitionTimeExtremeToRest;

        public bool AlternateMirroring;
        //---
        public float TransitionAlpha;

        [Range(0, 1)]
        public float TransitionThreshold = 1;
        public float SpeedModifier = 1;
        //-----

        //public RigPose[] ExercisePoses;

        public RigSetup Rig;

        public ExerciseInfo()
        {
            //Rig = GetComponentInParent<RigSetup>();
        }

        public string ToJson()
        {
            return ToJson(true);
        }
        public string ToJson(bool prettyPrint)
        {
            return JsonUtility.ToJson(this, prettyPrint);
        }

        public ExerciseInfo FromJson(string JsonString)
        {
            return JsonUtility.FromJson<ExerciseInfo>(JsonString);
        }

    }
}
