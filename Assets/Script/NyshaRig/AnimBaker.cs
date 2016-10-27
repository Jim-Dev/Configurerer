using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class AnimBaker : MonoBehaviour
    {
        [Header("Pose Parameters")]
        [Space(5)]
        public string AnimName = "NewAnim";
        [Space(5)]
        public bool BakeAnimation;
        public bool LoadAnimation;
        [Space(10)]

        [TextArea(2, 10)]
        public string ToJsonAnim;
        [TextArea(2, 10)]
        public string FromJsonAnim;

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

        void Start()
        {

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
                UnityEditor.AssetDatabase.Refresh();
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
