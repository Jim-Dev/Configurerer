using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Excersice
{
    [Serializable]
    public class ExerciseDescriptor
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

        public string ToJson()
        {
            return ToJson(true);
        }
        public string ToJson(bool prettyPrint)
        {
            return JsonUtility.ToJson(this, prettyPrint);
        }

        public ExerciseDescriptor FromJson(string JsonString)
        {
            return JsonUtility.FromJson<ExerciseDescriptor>(JsonString);
        }


    }
}
