using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.NyshaRig.Excersice
{
    public class RandomPose : Exercise
    {

        private float totalDeltaTime;

        private eTransitionState currentTransitionState;
        private eTransitionState previousTransitionState;

        public int selectedPoseIndex = 0;


        public RigPose CurrentPose;

        public int[] PoseSeries;
        public void Update()
        {
            if (TransitionAlpha >= TransitionThreshold )
            {
                if (selectedPoseIndex == 9)
                    selectedPoseIndex = -1;
                Debug.Log("ReachExtreme");
                totalDeltaTime = 0;

                selectedPoseIndex++;
                TransitionAlpha = 0;
                CurrentPose = RigPose.FromRig(Rig);
            }
            totalDeltaTime += Time.deltaTime;
            TransitionAlpha = totalDeltaTime / TransitionTimeRestToExtreme;
    

            TransitionPose(TransitionAlpha);
        }
        
        public void Start()
        {
            base.Start();


            WaitTimeWarmUp = 2;
            WaitTimeOnRest = 3;
            WaitTimeOnExtreme = 5;

            TransitionTimeExtremeToRest = 5;
            TransitionTimeRestToExtreme = 5;

            ExercisePoses = new List<RigPose>();
            LoadPoseFromAsset("DancerLeg.R");
            LoadPoseFromAsset("TouchFoot.R");
            LoadPoseFromAsset("TouchFootSide.R");
            LoadPoseFromAsset("TouchNose.R");
            PoseSeries = new int[10];
            PoseSeries[0] = 0;
            PoseSeries[1] = 2;
            PoseSeries[2] = 1;
            PoseSeries[3] = 2;
            PoseSeries[4] = 3;
            PoseSeries[5] = 0;
            PoseSeries[6] = 1;
            PoseSeries[7] = 2;
            PoseSeries[8] = 3;
            PoseSeries[9] = 0;
            selectedPoseIndex = 1;
            CurrentPose = RigPose.FromRig(Rig);

            totalDeltaTime = 0;
            previousTransitionState = eTransitionState.None;
            currentTransitionState = eTransitionState.None;

            currentTransitionState = eTransitionState.OnRestToExtreme;

        }

        private enum eTransitionState
        {
            None,
            OnRest,
            OnRestToExtreme,
            OnExtreme,
            OnExtremeToRest
        }

        public void TransitionPose(float Alpha)
        {
            Rig.SetFromPose(RigPose.Lerp(CurrentPose, ExercisePoses[PoseSeries[selectedPoseIndex]], Alpha));
        }
    }
}
