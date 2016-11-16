using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class RigAnimPlayer : MonoBehaviour
    {
        public bool LoopAnim;
        public bool PingPongAnim;

        public int MaxLoops;
        public int CurrentLoopCount;

        public float MaxTime;
        public float TotalAnimTime;

        public event EventHandler OnAnimationEnd;
        public event EventHandler OnAnimationStart;
        public event EventHandler OnWaitTimeComplete;

        public string CurrentAnimName;
        RigAnimation CurrentAnimation;

        public float TransitionAlpha;
        public float TransitionThreshold;

        private RigSetup Rig;
        private RigPose CurrentPose;
        private RigPose RestPose;
        public float totalDeltaTime = 0;
        public eTransitionState currentTransitionState;


        public float WaitTimeWarmUp;
        public float WaitTimeOnRest;
        public float WaitTimeOnExtreme;

        public float TransitionTimeRestToExtreme;
        public float TransitionTimeExtremeToRest;

        public float SpeedModifier;
        public bool MirrorAnim;
        public bool IsPlaying = false;


        public void Start()
        {
            Rig = GetComponentInParent<RigSetup>();
            currentTransitionState = eTransitionState.OnWarmUp;

            CurrentLoopCount = 0;
            MaxTime = 0;
        }

        public void Update()
        {

            if (MaxTime > 0 && TotalAnimTime >= MaxTime)
            {
                StopAnimation();
            }
            if (MaxLoops > 0 && CurrentLoopCount >= MaxLoops)
            {
                StopAnimation();
            }

            if (IsPlaying)
            {

                Rig.SetFromPose(CurrentPose);
                TotalAnimTime += Time.deltaTime;
                
                if (TransitionAlpha >= TransitionThreshold && currentTransitionState == eTransitionState.OnExtremeToRest)
                {
                    Debug.Log("ReachRest");
                    TransitionAlpha = 0;
                    totalDeltaTime = 0;
                    currentTransitionState = eTransitionState.OnRest;

                    if (PingPongAnim)
                        CurrentLoopCount++;
                }
                else if (TransitionAlpha >= TransitionThreshold && currentTransitionState == eTransitionState.OnRestToExtreme)
                {
                    Debug.Log("ReachExtreme");
                    TransitionAlpha = 0;
                    totalDeltaTime = 0;
                    currentTransitionState = eTransitionState.OnExtreme;
                }

                switch (currentTransitionState)
                {
                    case eTransitionState.None:
                        break;
                    case eTransitionState.OnWarmUp:
                        totalDeltaTime += Time.deltaTime;
                        if (totalDeltaTime >= WaitTimeWarmUp / SpeedModifier)
                        {
                            totalDeltaTime = 0;
                            currentTransitionState = eTransitionState.OnRestToExtreme;
                        }
                        break;
                    case eTransitionState.OnRest:
                        //Debug.Log("OnRest");
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
                            CurrentPose = CurrentAnimation.GetFinalPoseAtAnimPercentage(TransitionAlpha).GetMirroredPose();
                        else
                            CurrentPose = CurrentAnimation.GetFinalPoseAtAnimPercentage(TransitionAlpha);
                        break;
                    case eTransitionState.OnExtreme:
                        //Debug.Log("OnExtreme");
                        totalDeltaTime += Time.deltaTime;
                        if (totalDeltaTime >= WaitTimeOnExtreme / SpeedModifier)
                        {
                            totalDeltaTime = 0;

                            if (!PingPongAnim)
                                CurrentLoopCount++;

                            if (LoopAnim)
                            {
                                if (PingPongAnim)
                                    currentTransitionState = eTransitionState.OnExtremeToRest;
                                else
                                    currentTransitionState = eTransitionState.OnRest;
                            }
                            else
                            {
                                StopAnimation();
                            }
                        }
                        break;
                    case eTransitionState.OnExtremeToRest:
                        totalDeltaTime += Time.deltaTime;
                        TransitionAlpha = (totalDeltaTime / TransitionTimeExtremeToRest * SpeedModifier);
                        if (MirrorAnim)
                            CurrentPose = CurrentAnimation.GetFinalPoseAtAnimPercentage(1 - TransitionAlpha).GetMirroredPose();
                        else
                            CurrentPose = CurrentAnimation.GetFinalPoseAtAnimPercentage(1 - TransitionAlpha);
                        break;
                    default:
                        break;
                }


            }
        }

        private void LoadAnimation(string animationName)
        {
            CurrentAnimation=RigAnimation.LoadFromFile(animationName);
            CurrentAnimName = CurrentAnimation.AnimationName;
        }

        public void PlayAnimation(RigAnimation animation)
        {
            CurrentAnimation = animation;
            CurrentAnimName = CurrentAnimation.AnimationName;
            RestPose = RigPose.LoadFromFile(string.Format("rest_{0}", animation.AnimationName));
            CurrentPose = RestPose;
            IsPlaying = true;

            if (OnAnimationStart != null)
                OnAnimationStart.Invoke(this, EventArgs.Empty);
        }

        public void PlayAnimation(RigAnimation animation, float duration)
        {
            CurrentAnimation = animation;
            CurrentAnimName = CurrentAnimation.AnimationName;
            RestPose = RigPose.LoadFromFile(string.Format("rest_{0}", animation.AnimationName));
            CurrentPose = RestPose;

            MaxTime = duration;
            IsPlaying = true;

            if (OnAnimationStart != null)
                OnAnimationStart.Invoke(this, EventArgs.Empty);
        }

        public void PlayAnimation(RigAnimation animation, int loops)
        {
            CurrentAnimation = animation;
            CurrentAnimName = CurrentAnimation.AnimationName;
            RestPose = RigPose.LoadFromFile(string.Format("rest_{0}", animation.AnimationName));
            CurrentPose = RestPose;

            MaxLoops = loops;
            IsPlaying = true;

            if (OnAnimationStart != null)
                OnAnimationStart.Invoke(this, EventArgs.Empty);
        }

        public void StopAnimation()
        {
            IsPlaying = false;
            TotalAnimTime = 0;
            totalDeltaTime = 0;
            TransitionAlpha = 0;
            CurrentLoopCount = 0;
           
            currentTransitionState = eTransitionState.OnWarmUp;
            CurrentPose = RestPose;
            Rig.SetFromPose(RestPose);

            if (OnAnimationEnd != null)
                OnAnimationEnd.Invoke(this, EventArgs.Empty);

        }

        public void PauseAnimation()
        {

        }

        public void ResumeAnimation()
        {

        }

    }
}
