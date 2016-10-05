using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Excersice
{
    public class LinearPoseInterpolation : Exercise
    {

        private float totalDeltaTime;

        private eTransitionState currentTransitionState;
        private eTransitionState previousTransitionState;

        public int selectedPoseIndex = 0;

        public RigPose CurrentPose;
        
        public void Update()
        {
            if (TransitionAlpha >= TransitionThreshold && currentTransitionState == eTransitionState.OnExtremeToRest)
            {
                Debug.Log("ReachRest");
                totalDeltaTime = 0;
                previousTransitionState = currentTransitionState;
                currentTransitionState = eTransitionState.OnRest;


            }
            else if (TransitionAlpha >= TransitionThreshold && currentTransitionState == eTransitionState.OnRestToExtreme)
            {
                Debug.Log("ReachExtreme");
                totalDeltaTime = 0;
                previousTransitionState = currentTransitionState;
                currentTransitionState = eTransitionState.OnExtreme;

            }

            switch (currentTransitionState)
            {
                case eTransitionState.None:
                    break;
                case eTransitionState.OnRest:
                    //Debug.Log("OnRest");
                    totalDeltaTime += Time.deltaTime;
                    if (totalDeltaTime >= WaitTimeOnRest)
                    {
                        totalDeltaTime = 0;
                        previousTransitionState = currentTransitionState;
                        currentTransitionState = eTransitionState.OnRestToExtreme;

                        selectedPoseIndex = 1;
                        CurrentPose = RigPose.FromRig(Rig);
                        TransitionAlpha = 0;
                        //MirrorAnim = !MirrorAnim;
                    }
                    break;
                case eTransitionState.OnRestToExtreme:
                    totalDeltaTime += Time.deltaTime;
                    TransitionAlpha = totalDeltaTime / TransitionTimeRestToExtreme;
                    break;
                case eTransitionState.OnExtreme:
                    //Debug.Log("OnExtreme");
                    totalDeltaTime += Time.deltaTime;
                    if (totalDeltaTime >= WaitTimeOnExtreme)
                    {
                        totalDeltaTime = 0;
                        previousTransitionState = currentTransitionState;
                        currentTransitionState = eTransitionState.OnExtremeToRest;

                        selectedPoseIndex = 0;
                        CurrentPose = RigPose.FromRig(Rig);
                        TransitionAlpha = 0;

                    }
                    break;
                case eTransitionState.OnExtremeToRest:
                    totalDeltaTime += Time.deltaTime;
                    TransitionAlpha = totalDeltaTime / TransitionTimeExtremeToRest;
                    break;
                default:
                    break;
            }

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
            LoadPoseFromAsset("TPose");
            LoadPoseFromAsset("APose");

            selectedPoseIndex = 1;
            //Rig = GetComponent<RigSetup>();
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
            Rig.SetFromPose(RigPose.Lerp(CurrentPose, ExercisePoses[selectedPoseIndex], Alpha));
        }
    }
}
