using Assets.Script.NyshaRig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script
{
    public class RigExercisePlayerLite : MonoBehaviour, IExercisePlayer
    {

        public event EventHandler<PrepareEventArgs> OnInitializeExerciseEnd;
        public event EventHandler<PrepareEventArgs> OnInitializeExerciseStart;
        public event EventHandler OnRepetitionEnd;
        public event EventHandler<RepetitionStartEventArgs> OnRepetitionStart;

        RigAnimPlayer AnimPlayer;

        public bool TEST_InitializeAnim = false;

        public bool EDITOR_InitializePingPongExercise = false;
        public bool EDITOR_InitializeMultiExercise = false;
        public bool EDITOR_InitializeTimedExercise = false;

        public int MaxLoops;
        public float MaxTime;

        public eExerciseType CurrentExerciseType = eExerciseType.None;

        private RigAnimation SingleAnimExercise;
        private RigAnimation TimedAnimExercise;
        private RigAnimation[] MultiAnimExercise;
        private int MultiAnim_CurrentExerciseIndex;

        public void Start()
        {
            AnimPlayer = GetComponentInParent<RigAnimPlayer>();
            AnimPlayer.OnAnimationStart += AnimPlayer_OnAnimationStart;
            AnimPlayer.OnAnimationEnd += AnimPlayer_OnAnimationEnd;

            SingleAnimExercise = RigAnimation.LoadFromFile("EstocadaFrontal");
            TimedAnimExercise = RigAnimation.LoadFromFile("LeftHandFront");

            MultiAnimExercise = new RigAnimation[4];
            MultiAnimExercise[0]= RigAnimation.LoadFromFile("LeftHandUpAnim");
            MultiAnimExercise[1] = RigAnimation.LoadFromFile("RightHandUpAnim");
            MultiAnimExercise[2] = RigAnimation.LoadFromFile("LeftFootUpAnim");
            MultiAnimExercise[3] = RigAnimation.LoadFromFile("RightFootUpAnim");
            MultiAnim_CurrentExerciseIndex = 0;

            OnRepetitionStart += RigExercisePlayerLite_OnRepetitionStart;
            OnRepetitionEnd += RigExercisePlayerLite_OnRepetitionEnd;
        }

        private void RigExercisePlayerLite_OnRepetitionEnd(object sender, EventArgs e)
        {
            Debug.Log("DEBUG - REPETITION END");
        }

        private void RigExercisePlayerLite_OnRepetitionStart(object sender, RepetitionStartEventArgs e)
        {
            Debug.Log("DEBUG - REPETITION START");
        }

        private void AnimPlayer_OnAnimationEnd(object sender, EventArgs e)
        {
            Debug.Log("Anim End event");

            if (CurrentExerciseType == eExerciseType.SingleAnim)
            {
                if (OnRepetitionEnd != null)
                    OnRepetitionEnd.Invoke(this, EventArgs.Empty);
            }

            if (CurrentExerciseType == eExerciseType.Timed)
            {
                if (OnRepetitionEnd != null)
                    OnRepetitionEnd.Invoke(this, EventArgs.Empty);
            }

            if (CurrentExerciseType == eExerciseType.MultiAnim)
            {
                MultiAnim_CurrentExerciseIndex++;
                if (MultiAnim_CurrentExerciseIndex<MultiAnimExercise.Length)
                {
                    AnimPlayer.PlayAnimation(MultiAnimExercise[MultiAnim_CurrentExerciseIndex]);
                }
                else
                {
                    if (OnRepetitionEnd != null)
                        OnRepetitionEnd.Invoke(this, EventArgs.Empty);
                }

            }
        }

        private void AnimPlayer_OnAnimationStart(object sender, EventArgs e)
        {
            Debug.Log("Anim Start event");
        }

        public void Update()
        {
            if (TEST_InitializeAnim)
            {
                TEST_InitializeAnim = false;

                AnimPlayer.PlayAnimation(RigAnimation.LoadFromFile("EstocadaFrontal"));
            }

            if (EDITOR_InitializePingPongExercise)
            {
                CurrentExerciseType = eExerciseType.SingleAnim;
                EDITOR_InitializePingPongExercise = false;
                AnimPlayer.PingPongAnim = true;
                AnimPlayer.MaxTime = 0;
                AnimPlayer.PlayAnimation(SingleAnimExercise, 10);
            }
            if (EDITOR_InitializeTimedExercise)
            {
                CurrentExerciseType = eExerciseType.Timed;
                EDITOR_InitializeTimedExercise = false;
                AnimPlayer.PingPongAnim = false;
                AnimPlayer.LoopAnim = true;
                AnimPlayer.MaxLoops = 0;
                AnimPlayer.PlayAnimation(TimedAnimExercise, MaxTime);
            }

            if (EDITOR_InitializeMultiExercise)
            {
                CurrentExerciseType = eExerciseType.MultiAnim;
                EDITOR_InitializeMultiExercise = false;
                AnimPlayer.PingPongAnim = true;
                AnimPlayer.LoopAnim = true;
                AnimPlayer.PlayAnimation(MultiAnimExercise[0], 1);

            }

        }


        public void InitializeExercise(Exercise exercise, BehaviourParams bParam)
        {
            throw new NotImplementedException();
        }

        public void InitializeWebExercise(string jsonString)
        {
            throw new NotImplementedException();
        }

        public void ResumePauseExercise()
        {
            throw new NotImplementedException();
        }

        public void SetRestPose()
        {
            throw new NotImplementedException();
        }

        public void StartExercise(bool isInInstruction)
        {
            throw new NotImplementedException();
        }

        public void StartExerciseNoParams()
        {
            throw new NotImplementedException();
        }

        public void StartWebExercise(string jsonString)
        {
            throw new NotImplementedException();
        }

        public void StopExercise()
        {
            throw new NotImplementedException();
        }

        public enum eExerciseType
        {
            None,
            SingleAnim,
            MultiAnim,
            Timed
        }
    }
}
