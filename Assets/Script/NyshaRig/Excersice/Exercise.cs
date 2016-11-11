using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

//using UnityEditor;
using UnityEngine;

namespace Assets.Script.NyshaRig.Excersice
{
    public class Exercise:MonoBehaviour
    {
        public string Name;
        public string Description;

        public Guid UniqueID;

        public List<RigPose> ExercisePoses;

        //-- ANIM PROPERTIES

        public float WaitTimeWarmUp;
        public float WaitTimeOnRest;
        public float WaitTimeOnExtreme;

        public float TransitionTimeRestToExtreme;
        public float TransitionTimeExtremeToRest;

        public bool AlternateMirroring;
        [Range(0, 1)]
        public float TransitionAlpha;

        [Range(0, 1)]
        public float TransitionThreshold=1;
        //------


        protected RigSetup Rig;
      

        public void Start()
        {
            Rig = GetComponentInParent<RigSetup>();
        }

        public Exercise()
        {
         
        }

    }
}
