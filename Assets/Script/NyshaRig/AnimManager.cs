using Assets.Script.NyshaRig.Excersice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class AnimManager : MonoBehaviour
    {



        [Header("Animation Parameters")]
        [Space(5)]
        public string AnimName = "NewAnim";
        [Space(5)]
        public bool BakeAnimation = false;
        public bool LoadAnimation = false;
        public bool PlayAnimation = false;
        public bool AlternateMirroring = true;
        [Space(5)]
        public bool IsAnimPlaying = false;
        public bool MirrorAnim = false;

        [Space(10)]
        public string ActivePoseName;
        [Space(10)]

        public ExerciseInfo exerInfo;

        [Space(10)]
        public List<FrameInfo> Frames;

        public RigAnimation RigAnim;
        // Use this for initialization

        [Header("New Frame")]
        [Space(5)]
        public string PoseName;
        [Space(5)]
        public float PoseStartAt;
        [Space(5)]
        public bool AddFrameToAnim;

        private RigSetup Rig;
        private RigPose CurrentPose;
        private float totalDeltaTime = 0;
        private eTransitionState currentTransitionState;

        void Start()
        {
            Rig = GetComponentInParent<RigSetup>();
            currentTransitionState = eTransitionState.OnWarmUp;
        }

        void Update()
        {
            if (BakeAnimation)
            {
                BakeAnimation = false;
                RigAnim = new RigAnimation();
                RigAnim.AnimationName = AnimName;
                foreach (FrameInfo fInfo in Frames)
                {
                    RigKeyFrame kFrame = new RigKeyFrame();
                    kFrame.Pose = RigPose.LoadFromFile(fInfo.PoseName);
                    kFrame.FrameStartAt = fInfo.FrameStartAt;
                    RigAnim.AddKeyFrame(kFrame);
                }
                RigAnim.SaveToFile(RigAnim.AnimationName);
                //RigAnim.SaveToDisk(RigAnim.AnimationName);
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
            if (AddFrameToAnim)
            {
                AddFrameToAnim = false;

                RigPose rigPose = RigPose.LoadFromFile(PoseName);
                if (rigPose != null)
                {
                    FrameInfo fInfo = new FrameInfo();
                    fInfo.PoseName = rigPose.PoseName;
                    fInfo.FrameStartAt = PoseStartAt;
                    Frames.Add(fInfo);
                    PoseStartAt++;
                }
            }

            if (LoadAnimation)
            {
                LoadAnimation = false;
                RigAnim = RigAnimation.LoadFromFile(AnimName);
                Frames.Clear();
                foreach (RigKeyFrame frame in RigAnim.AnimationFrames)
                {
                    FrameInfo fInfo = new FrameInfo();
                    fInfo.PoseName = frame.Pose.PoseName;
                    fInfo.FrameStartAt = frame.FrameStartAt;

                    Frames.Add(fInfo);
                }
                currentTransitionState = eTransitionState.OnWarmUp;
                totalDeltaTime = 0;
                CurrentPose = RigAnim.GetFirstFrame().Pose;

            }

            if (PlayAnimation)
            {
                PlayAnimation = false;
                IsAnimPlaying = !IsAnimPlaying;
            }


            if (IsAnimPlaying)
            {
                Rig.SetFromPose(CurrentPose);
                ActivePoseName = CurrentPose.PoseName;


                if (exerInfo.TransitionAlpha >= exerInfo.TransitionThreshold && currentTransitionState == eTransitionState.OnExtremeToRest)
                {
                    Debug.Log("ReachRest");
                    exerInfo.TransitionAlpha = 0;
                    totalDeltaTime = 0;
                    currentTransitionState = eTransitionState.OnRest;
                    if (AlternateMirroring)
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
                            CurrentPose = RigAnim.GetFinalPoseAtAnimPercentage(exerInfo.TransitionAlpha).GetMirroredPose();
                        else
                            CurrentPose = RigAnim.GetFinalPoseAtAnimPercentage(exerInfo.TransitionAlpha);
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
                            CurrentPose = RigAnim.GetFinalPoseAtAnimPercentage(1 - exerInfo.TransitionAlpha).GetMirroredPose();
                        else
                            CurrentPose = RigAnim.GetFinalPoseAtAnimPercentage(1 - exerInfo.TransitionAlpha);
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
    }

    [Serializable]
    public struct FrameInfo
    {
        public string PoseName;
        public float FrameStartAt;

        public override string ToString()
        {
            return string.Format("{0} : At {1}", PoseName, FrameStartAt);
        }
    }
}
