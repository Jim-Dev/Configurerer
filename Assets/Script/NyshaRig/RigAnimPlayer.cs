using Assets.Script.NyshaRig.Excersice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class RigAnimPlayer : MonoBehaviour
    {
        public RigAnimation RigAnimationSelected;
        public ExerciseInfo exerInfo;

        public string RigAnimName;

        public bool LoadAnimation = false;
        public bool PlayAnimation = false;

        public bool IsAnimPlaying = false;


        private float totalDeltaTime = 0;

        private eTransitionState currentTransitionState;

        public int selectedPoseIndex = 0;

        public RigPose CurrentPose;

        RigSetup Rig;

        public bool MirrorAnim = false;

        public void Start()
        {
            Rig = GetComponentInParent<RigSetup>();
            currentTransitionState = eTransitionState.OnWarmUp;
        }

        public void Update()
        {
            if (LoadAnimation)
            {
                LoadAnimation = false;
                RigAnimationSelected = RigAnimation.LoadFromFile(RigAnimName);

                currentTransitionState = eTransitionState.OnWarmUp;
                totalDeltaTime = 0;
                CurrentPose = RigAnimationSelected.GetFirstFrame().Pose;

            }  

            if (PlayAnimation)
            {
                PlayAnimation = false;
                IsAnimPlaying = !IsAnimPlaying;
            }

            if (IsAnimPlaying)
            {
                Rig.SetFromPose(CurrentPose);



                if (exerInfo.TransitionAlpha >= exerInfo.TransitionThreshold && currentTransitionState == eTransitionState.OnExtremeToRest)
                {
                    Debug.Log("ReachRest");
                    exerInfo.TransitionAlpha = 0;
                    totalDeltaTime = 0;
                    currentTransitionState = eTransitionState.OnRest;
                    MirrorAnim = !MirrorAnim;


                }
                else if (exerInfo.TransitionAlpha >= exerInfo.TransitionThreshold && currentTransitionState == eTransitionState.OnRestToExtreme)
                {
                    Debug.Log("ReachExtreme");
                    exerInfo.TransitionAlpha = 0;
                    totalDeltaTime = 0;
                    currentTransitionState = eTransitionState.OnExtreme;

                }

                switch (currentTransitionState)
                {
                    case eTransitionState.None:
                        break;
                    case eTransitionState.OnWarmUp:
                        totalDeltaTime += Time.deltaTime;
                        if (totalDeltaTime >= exerInfo.WaitTimeWarmUp / exerInfo.SpeedModifier)
                        {
                            totalDeltaTime = 0;
                            currentTransitionState = eTransitionState.OnRestToExtreme;
                        }
                        break;
                    case eTransitionState.OnRest:
                        //Debug.Log("OnRest");
                        totalDeltaTime += Time.deltaTime;
                        if (totalDeltaTime >= exerInfo.WaitTimeOnRest / exerInfo.SpeedModifier)
                        {
                            totalDeltaTime = 0;
                            currentTransitionState = eTransitionState.OnRestToExtreme;
                        }
                        break;
                    case eTransitionState.OnRestToExtreme:
                        totalDeltaTime += Time.deltaTime;
                        exerInfo.TransitionAlpha = totalDeltaTime / exerInfo.TransitionTimeRestToExtreme * exerInfo.SpeedModifier;
                        if (MirrorAnim)
                            CurrentPose = RigAnimationSelected.GetFinalPoseAtAnimPercentage(exerInfo.TransitionAlpha).GetMirroredPose();
                        else
                            CurrentPose = RigAnimationSelected.GetFinalPoseAtAnimPercentage(exerInfo.TransitionAlpha);
                        break;
                    case eTransitionState.OnExtreme:
                        //Debug.Log("OnExtreme");
                        totalDeltaTime += Time.deltaTime;
                        if (totalDeltaTime >= exerInfo.WaitTimeOnExtreme / exerInfo.SpeedModifier)
                        {
                            totalDeltaTime = 0;
                            currentTransitionState = eTransitionState.OnExtremeToRest;
                        }
                        break;
                    case eTransitionState.OnExtremeToRest:
                        totalDeltaTime += Time.deltaTime;
                        exerInfo.TransitionAlpha = (totalDeltaTime / exerInfo.TransitionTimeExtremeToRest * exerInfo.SpeedModifier);
                        if (MirrorAnim)
                            CurrentPose = RigAnimationSelected.GetFinalPoseAtAnimPercentage(1 - exerInfo.TransitionAlpha).GetMirroredPose();
                        else
                            CurrentPose = RigAnimationSelected.GetFinalPoseAtAnimPercentage(1 - exerInfo.TransitionAlpha);
                        break;
                    default:
                        break;
                }

            }




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
            //exerInfo.Rig.SetFromPose(RigPose.Lerp(CurrentPose, exerInfo.ExercisePoses[PoseSeries[selectedPoseIndex]], Alpha));
        }


    }


}
