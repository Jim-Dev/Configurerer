using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class IKSetup:MonoBehaviour
    {
        [Header("Rig Control")]
        [Space(5)]
        public bool RenderOnGame = true;
        public bool SnapIKToFK = false;
        [Space(10)]
        [Header("Inverse Kinematics Weights")]
        [Space(5)]
        [Range(0, 1)]
        public float IKBaseWeight;

        [Space(5)]
        [Range(0, 1)]
        public float IKHandLeftPositionWeight;
        [Range(0, 1)]
        public float IKHandLeftRotationWeight;
        [Range(0, 1)]
        public float IKHandLeftPoleWeight;

        [Space(5)]
        [Range(0, 1)]
        public float IKHandRightPositionWeight;
        [Range(0, 1)]
        public float IKHandRightRotationWeight;
        [Range(0, 1)]
        public float IKHandRightPoleWeight;

        [Space(5)]
        [Range(0, 1)]
        public float IKFootLeftPositionWeight;
        [Range(0, 1)]
        public float IKFootLeftRotationWeight;
        [Range(0, 1)]
        public float IKFootLeftPoleWeight;

        [Space(5)]
        [Range(0, 1)]
        public float IKFootRightPositionWeight;
        [Range(0, 1)]
        public float IKFootRightRotationWeight;
        [Range(0, 1)]
        public float IKFootRightPoleWeight;


        [Space(10)]
        [Range(0, 1)]
        public float LookAtBaseWeight;
        [Range(0, 1)]
        public float LookAtBodyWeight;
        [Range(0, 1)]
        public float LookAtHeadWeight;
        [Range(0, 1)]
        public float LookAtEyesWeight;
        [Range(0, 1)]
        public float LookAtClampWeight;


        [Header("IK Reference Lines")]
        [Space(5)]
        public Color IKLineReferenceColor;
        public bool DrawDebugReferenceLines = true;
        public bool DrawGLReferenceLines;


        public float NormalizeIKWeight(float IKWeight)
        {
            return Mathf.Clamp01(IKBaseWeight * IKWeight);
        }

        public event EventHandler OnSnapIKToFK;
     
        public void Update()
        {
            if (SnapIKToFK)
            {
                SnapIKToFK = false;
                if (OnSnapIKToFK!=null)
                {
                    OnSnapIKToFK.Invoke(this, EventArgs.Empty);//Stupid Mono workaround
                }
                //OnSnapIKToFK?.Invoke(this, EventArgs.Empty);
            }
        }
           
    }
}
