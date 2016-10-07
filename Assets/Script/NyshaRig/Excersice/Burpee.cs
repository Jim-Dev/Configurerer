﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.NyshaRig.Excersice
{
    public class Burpee : Exercise
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
                if (selectedPoseIndex == 10)
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
            TransitionTimeRestToExtreme = 1;

            ExercisePoses = new List<RigPose>();
            LoadPoseFromAsset("Burpee01");
            LoadPoseFromAsset("Burpee07");
            LoadPoseFromAsset("Burpee08");
            LoadPoseFromAsset("Burpee09");
            LoadPoseFromAsset("Burpee05");
            LoadPoseFromAsset("Burpee06");
            PoseSeries = new int[11];
            PoseSeries[0] = 0;
            PoseSeries[1] = 1;
            PoseSeries[2] = 2;
            PoseSeries[3] = 3;
            PoseSeries[4] = 2;
            PoseSeries[5] = 1;
            PoseSeries[6] = 0;
            PoseSeries[7] = 4;
            PoseSeries[8] = 5;
            PoseSeries[9] = 4;
            PoseSeries[10] = 0;
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
