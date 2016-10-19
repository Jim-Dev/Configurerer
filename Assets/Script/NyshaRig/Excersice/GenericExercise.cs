using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig.Excersice
{
    public class GenericExercise : Exercise
    {
        public RigAnimation ToExtremeAnim;
        public RigAnimation ToRestAnim;

        RigKeyFrame estocadaRestFrame;
        RigKeyFrame estocadaMidFrame;
        RigKeyFrame estocadaExtremeFrame;

        private float totalDeltaTime = 0;

        private eTransitionState currentTransitionState;
        private eTransitionState previousTransitionState;

        public int selectedPoseIndex = 0;

        public RigPose CurrentPose;

        public int[] PoseSeries;

        [Range(0.1f, 2)]
        public float SpeedModifier = 1;

        public bool MirrorAnim = false;

        public void Update()
        {


            Rig.SetFromPose(CurrentPose);


            if (TransitionAlpha >= TransitionThreshold && currentTransitionState == eTransitionState.OnExtremeToRest)
            {
                Debug.Log("ReachRest");
                TransitionAlpha = 0;
                totalDeltaTime = 0;
                previousTransitionState = currentTransitionState;
                currentTransitionState = eTransitionState.OnRest;
                MirrorAnim = !MirrorAnim;


            }
            else if (TransitionAlpha >= TransitionThreshold && currentTransitionState == eTransitionState.OnRestToExtreme)
            {
                Debug.Log("ReachExtreme");
                TransitionAlpha = 0;
                totalDeltaTime = 0;
                previousTransitionState = currentTransitionState;
                currentTransitionState = eTransitionState.OnExtreme;

            }



            switch (currentTransitionState)
            {
                case eTransitionState.None:
                    break;
                case eTransitionState.OnWarmUp:
                    totalDeltaTime += Time.deltaTime;
                    if (totalDeltaTime >= WaitTimeWarmUp/SpeedModifier)
                    {
                        totalDeltaTime = 0;
                        currentTransitionState = eTransitionState.OnRestToExtreme;
                    }
                    break;
                case eTransitionState.OnRest:
                    Debug.Log("OnRest");
                    totalDeltaTime += Time.deltaTime;
                    if (totalDeltaTime >= WaitTimeOnRest / SpeedModifier)
                    {
                        totalDeltaTime = 0;
                        currentTransitionState = eTransitionState.OnRestToExtreme;
                    }
                    break;
                case eTransitionState.OnRestToExtreme:
                    totalDeltaTime += Time.deltaTime;
                    TransitionAlpha = totalDeltaTime / TransitionTimeRestToExtreme * SpeedModifier;
                    if (MirrorAnim)
                        CurrentPose = ToExtremeAnim.GetFinalPoseAtAnimPercentage(TransitionAlpha).GetMirroredPose();
                    else
                        CurrentPose = ToExtremeAnim.GetFinalPoseAtAnimPercentage(TransitionAlpha);
                    break;
                case eTransitionState.OnExtreme:
                    Debug.Log("OnExtreme");
                    totalDeltaTime += Time.deltaTime;
                    if (totalDeltaTime >= WaitTimeOnExtreme / SpeedModifier)
                    {
                        totalDeltaTime = 0;
                        currentTransitionState = eTransitionState.OnExtremeToRest;
                    }
                    break;
                case eTransitionState.OnExtremeToRest:
                    totalDeltaTime += Time.deltaTime;
                    TransitionAlpha = (totalDeltaTime / TransitionTimeExtremeToRest * SpeedModifier);
                    if (MirrorAnim)
                        CurrentPose = ToExtremeAnim.GetFinalPoseAtAnimPercentage(1 - TransitionAlpha).GetMirroredPose();
                    else
                        CurrentPose = ToExtremeAnim.GetFinalPoseAtAnimPercentage(1 - TransitionAlpha);
                    break;
                default:
                    break;
            }


        }

        public void Start()
        {
            base.Start();

            RigKeyFrame YogaChildPoseFrame = new RigKeyFrame();
            YogaChildPoseFrame.FrameStartAt = 7;
            YogaChildPoseFrame.Pose = RigPose.LoadPoseFromAsset("Yoga_ChildPose.json");


            estocadaRestFrame = new RigKeyFrame();
            estocadaRestFrame.FrameStartAt = 0;
            estocadaRestFrame.Pose = RigPose.LoadPoseFromAsset("EstocadaFrontal_Rest.json");

            estocadaMidFrame = new RigKeyFrame();
            estocadaMidFrame.FrameStartAt = 5;
            estocadaMidFrame.Pose = RigPose.LoadPoseFromAsset("EstocadaFrontal_Mid.json");

            estocadaExtremeFrame = new RigKeyFrame();
            estocadaExtremeFrame.FrameStartAt = 6;
            estocadaExtremeFrame.Pose = RigPose.LoadPoseFromAsset("EstocadaFrontal_90.json");

            ToExtremeAnim = new RigAnimation();
            ToExtremeAnim.AnimationName = "Estocada frontal RestToExtreme";
            ToExtremeAnim.AddKeyFrame(estocadaRestFrame);
            ToExtremeAnim.AddKeyFrame(estocadaMidFrame);
            ToExtremeAnim.AddKeyFrame(estocadaExtremeFrame);


            WaitTimeWarmUp = 2;
            WaitTimeOnRest = 3;
            WaitTimeOnExtreme = 5;

            TransitionTimeExtremeToRest = 5;
            TransitionTimeRestToExtreme = 1;


            CurrentPose = RigPose.FromRig(Rig);

            totalDeltaTime = 0;
            previousTransitionState = eTransitionState.None;
            currentTransitionState = eTransitionState.None;

            currentTransitionState = eTransitionState.OnWarmUp;

        }

        private enum eTransitionState
        {
            None,
            OnWarmUp,
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