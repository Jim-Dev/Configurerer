using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using System.IO;

namespace Assets.Script.NyshaRig
{
    [Serializable]
    public class RigAnimation 
    {
        public string AnimationName;
        private Dictionary<Guid, RigKeyFrame> AnimationFrames; //frameID,RigKeyFrame

        public RigAnimation()
        {
            AnimationFrames = new Dictionary<Guid, RigKeyFrame> ();


            AnimationName = string.Empty;
        }

        public void AddKeyFrame(RigKeyFrame keyFrame)
        {
            AnimationFrames.Add(Guid.NewGuid(), keyFrame);
        }



        public float GetTotalDuration()
        {

            float maxFramePosition = float.MinValue;

            foreach (RigKeyFrame kFrame in AnimationFrames.Values)
            {
                if (kFrame.FrameStartAt > maxFramePosition)
                    maxFramePosition = kFrame.FrameStartAt;
            }

            return maxFramePosition;
        }

        public int GetFrameCount()
        {
            return AnimationFrames.Count;
        }

        public RigKeyFrame GetFirstFrame()
        {
            RigKeyFrame tmpKFrame= new RigKeyFrame();
            tmpKFrame.FrameStartAt = float.MaxValue;

            foreach (RigKeyFrame kFrame in AnimationFrames.Values)
            {
                if (kFrame.FrameStartAt < tmpKFrame.FrameStartAt)
                    tmpKFrame = kFrame;
            }
            return tmpKFrame;
        }
        public RigKeyFrame GetLastFrame()
        {
            RigKeyFrame tmpKFrame = new RigKeyFrame();
            tmpKFrame.FrameStartAt = float.MinValue;

            foreach (RigKeyFrame kFrame in AnimationFrames.Values)
            {
                if (kFrame.FrameStartAt > tmpKFrame.FrameStartAt)
                    tmpKFrame = kFrame;
            }
            return tmpKFrame;
        }

        public List<RigKeyFrame> GetKeyFramesAtTime(float animTime)
        {
            List<RigKeyFrame> output = new List<RigKeyFrame>();


            if (animTime < 0)
            {
                output.Add(GetFirstFrame());
                return output;
            }
            else if (animTime > GetTotalDuration())
            {
                output.Add(GetLastFrame());
                return output;
            }

            foreach (RigKeyFrame kFrame in AnimationFrames.Values)
            {
                if (kFrame.FrameStartAt == animTime)
                {
                    output.Add(kFrame);
                    return output;
                }
            }

            RigKeyFrame negativeFrame = new RigKeyFrame();
            RigKeyFrame positiveFrame = new RigKeyFrame();

            float negativeDelta = float.MaxValue;
            foreach (RigKeyFrame kFrame in AnimationFrames.Values)
            {
                float deltaF = animTime - kFrame.FrameStartAt;
                if (deltaF > 0)
                {
                    if (deltaF < negativeDelta)
                    {
                        negativeDelta = animTime - kFrame.FrameStartAt;
                        negativeFrame = kFrame;
                    }
                }
            }

            float positiveDelta = float.MaxValue;
            foreach (RigKeyFrame kFrame in AnimationFrames.Values)
            {
                float deltaF =   kFrame.FrameStartAt - animTime;
                if (deltaF > 0)
                {
                    if (deltaF < positiveDelta)
                    {
                        positiveDelta = kFrame.FrameStartAt - animTime;
                        positiveFrame = kFrame;
                    }
                }
            }  
            output.Add(negativeFrame);
            output.Add(positiveFrame);

            return output;
        }

        public static RigAnimation LoadPoseFromAsset(string poseName)
        {
            string jsonString = string.Empty;

            if (File.Exists(string.Format("{0}{1}", RigPose.PosesPath, poseName)))
            {
                using (FileStream fs = new FileStream(string.Format("{0}{1}", RigPose.PosesPath, poseName), FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        jsonString = reader.ReadToEnd();
                    }
                }

                return RigAnimation.FromJson(jsonString);
            }
            else
                return null;
        }

        public void WriteAsset(string fileName)
        {
            using (FileStream fs = new FileStream(string.Format( @"Assets/Animations/Poses/{0}.json",fileName), FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(ToJson());
                }
            }
        }

        public string ToJson()
        {
            return ToJson(true);
        }
        public string ToJson(bool prettyPrint)
        {
            return JsonUtility.ToJson(this, prettyPrint);
        }

        public static RigAnimation FromJson(string JsonString)
        {
            return JsonUtility.FromJson<RigAnimation>(JsonString);
        }

        public List<RigPose> GetPoseAtAnimTime(float animTime)
        {
            List<RigPose> output = new List<RigPose>();

            foreach (RigKeyFrame kFrame in GetKeyFramesAtTime(animTime))
            {
                output.Add(kFrame.Pose);
            }

            return output;
        }

        public string GetPoseNameAtTime(float animTime)
        {
            List<RigPose> posesAtTime = GetPoseAtAnimTime(animTime);

            if (posesAtTime.Count == 1)
                return posesAtTime[0].PoseName;
            else if (posesAtTime.Count == 2)
                return posesAtTime[0].PoseName + " + " + posesAtTime[1].PoseName;
            else
                return "No pose found";
        }
        
        public RigPose GetFinalPoseAtTime(float animTime)
        {
            RigPose output;
            List<RigKeyFrame> KFrames = GetKeyFramesAtTime(animTime);

            if (KFrames.Count == 1)
                output = KFrames[0].Pose;
            else if (KFrames.Count == 2)
            {
                float A, B, Total;
                A = animTime - KFrames[0].FrameStartAt;
                B = KFrames[1].FrameStartAt - KFrames[0].FrameStartAt;
                Total = A / B;
                //Debug.Log(string.Format("A={0};B={1};Total={2}", A, B, Total));
                output = RigPose.Lerp(KFrames[0].Pose, KFrames[1].Pose, Total);
            }
            else return null;



            return output;
        }

        public RigPose GetFinalPoseAtAnimPercentage(float animPercentage)
        {
            return GetFinalPoseAtTime(GetTotalDuration() * animPercentage);
        }
        
    }
    [Serializable]
    public class RigKeyFrame
    {
        public float FrameStartAt;
        private float FrameEndAt;

        public float Duration
        {
            get { return FrameStartAt - FrameEndAt; }
        }
        public RigPose Pose;

    }

}
