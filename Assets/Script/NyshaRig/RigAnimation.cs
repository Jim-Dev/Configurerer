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
        public List<RigKeyFrame> AnimationFrames;

        public float GetTotalDuration()
        {
            float output = 0;

            foreach (RigKeyFrame kFrame in AnimationFrames)
            {
                output += kFrame.Duration;
            }

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
    }
    [Serializable]
    public class RigKeyFrame
    {
        public float Duration;
        public RigPose Pose;
    }

}
