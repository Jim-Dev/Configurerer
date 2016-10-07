using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.NyshaRig.Excersice
{
    public class CrowYoga : Exercise
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
                if (selectedPoseIndex == 15)
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
            TransitionTimeRestToExtreme = 3;

            ExercisePoses = new List<RigPose>();
            LoadPoseFromAsset("Yoga_ChildPose");
            LoadPoseFromAsset("DownwardDuck01");
            LoadPoseFromAsset("DownwardDuck02");
            LoadPoseFromAsset("DownwardDuck03");
            LoadPoseFromAsset("DownwardDuck04");
            LoadPoseFromAsset("FireFly");
            LoadPoseFromAsset("FireFly02");
            LoadPoseFromAsset("Crow");
            //LoadPoseFromAsset("Burpee06");
            PoseSeries = new int[16];
            PoseSeries[0] = 0;
            PoseSeries[1] = 1;
            PoseSeries[2] = 2;
            PoseSeries[3] = 3;
            PoseSeries[4] = 4;
            PoseSeries[5] = 5;
            PoseSeries[6] = 6;
            PoseSeries[7] = 7;
            PoseSeries[8] = 6;
            PoseSeries[9] = 5;
            PoseSeries[10] = 4;
            PoseSeries[11] = 3;
            PoseSeries[12] = 2;
            PoseSeries[13] = 1;
            PoseSeries[14] = 0;
            PoseSeries[15] = 0;
            selectedPoseIndex = 0;
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
