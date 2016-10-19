using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig.Excersice
{
    public class GenericExercise:Exercise
    {
        public RigAnimation ToExtremeAnim;
        public RigAnimation ToRestAnim;

        RigKeyFrame estocadaRestFrame;
        RigKeyFrame estocadaMidFrame;
        RigKeyFrame estocadaExtremeFrame;

        private float totalDeltaTime=-5;

        private eTransitionState currentTransitionState;
        private eTransitionState previousTransitionState;

        public int selectedPoseIndex = 0;

        public RigPose CurrentPose;

        public int[] PoseSeries;
        public void Update()
        {
            totalDeltaTime += Time.deltaTime;
            //Debug.Log(totalDeltaTime);
            //Debug.Log(string.Format("{0} : {1}",totalDeltaTime, ToExtremeAnim.GetPoseNameAtTime(totalDeltaTime)));

            CurrentPose = ToExtremeAnim.GetFinalPoseAtTime(totalDeltaTime);

            Rig.SetFromPose(CurrentPose);

            /*
            if (TransitionAlpha >= TransitionThreshold)
            {
                if (selectedPoseIndex == 2)
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
            */
        }

        public void Start()
        {
            base.Start();

            RigKeyFrame YogaChildPoseFrame = new RigKeyFrame();
            YogaChildPoseFrame.FrameStartAt = 7;
            YogaChildPoseFrame.Pose = RigPose.LoadPoseFromAsset("Yoga_ChildPose.json");


            estocadaRestFrame = new RigKeyFrame();
            estocadaRestFrame.FrameStartAt = 0;
            //estocadaRestFrame.FrameEndAt = 5;
            //estocadaRestFrame.Duration = 5;
            estocadaRestFrame.Pose = RigPose.LoadPoseFromAsset("EstocadaFrontal_Rest.json");

            estocadaMidFrame = new RigKeyFrame();
            estocadaMidFrame.FrameStartAt = 5;
            //estocadaRestFrame.FrameEndAt = 5;
            //estocadaMidFrame.Duration = 2;
            estocadaMidFrame.Pose = RigPose.LoadPoseFromAsset("EstocadaFrontal_Mid.json");

            estocadaExtremeFrame = new RigKeyFrame();
            estocadaExtremeFrame.FrameStartAt = 6;
            //estocadaExtremeFrame.Duration = 7;
            estocadaExtremeFrame.Pose = RigPose.LoadPoseFromAsset("EstocadaFrontal_90.json");

            ToExtremeAnim = new RigAnimation();
            ToExtremeAnim.AnimationName = "Estocada frontal RestToExtreme";
            ToExtremeAnim.AddKeyFrame(estocadaRestFrame);
            ToExtremeAnim.AddKeyFrame(estocadaMidFrame);
            ToExtremeAnim.AddKeyFrame(estocadaExtremeFrame);
            //ToExtremeAnim.AddKeyFrame(YogaChildPoseFrame);
            //ToExtremeAnim.AnimationFrames.Add(estocadaRestFrame);
            //ToExtremeAnim.AnimationFrames.Add(estocadaMidFrame);
            //ToExtremeAnim.AnimationFrames.Add(estocadaExtremeFrame);

            WaitTimeWarmUp = 2;
            WaitTimeOnRest = 3;
            WaitTimeOnExtreme = 5;

            TransitionTimeExtremeToRest = 5;
            TransitionTimeRestToExtreme = 1;

       
            CurrentPose = RigPose.FromRig(Rig);

            totalDeltaTime = -5;
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
