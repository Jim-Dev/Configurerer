using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    [Serializable]
    public class RigAnimation 
    {
        public List<RigKeyFrame> AnimationFrames;

    }
    [Serializable]
    public class RigKeyFrame
    {
        public float Duration;
        public RigPose Pose;
    }
}
