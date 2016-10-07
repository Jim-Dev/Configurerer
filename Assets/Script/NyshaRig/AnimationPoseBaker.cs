using UnityEngine;
using System.Collections;
using UnityEditor;

using Assets.Script;

using System.IO;
//[RequireComponent(typeof(RigSetup))]

namespace Assets.Script.NyshaRig
{
    public class AnimationPoseBaker : MonoBehaviour
    {
        [Header("Pose Parameters")]
        [Space(5)]
        public string AnimPath = @"TestPose";
        [Space(5)]
        public bool CreateJsonPose;
        public bool SetPoseFromJson;
        [Space(10)]

        [TextArea(2, 10)]
        public string ToJsonPose;
        [TextArea(2, 10)]
        public string FromJsonPose;
        [Space(10)]
        public RigSetup Rig;
        private RigPose rigPose;

        public RigAnimation RigAnim;
        public RigAnimation[] RigAnims;
        // Use this for initialization
        void Start()
        {
            Rig = GetComponentInChildren<RigSetup>();
        }

        // Update is called once per frame
        void Update()
        {
            if (CreateJsonPose)
            {
                CreateJsonPose = false;
                SetPose();
                rigPose.PoseName = AnimPath;
                ToJsonPose = rigPose.ToJson(true);
                SaveItemInfo();
            }
            if (SetPoseFromJson)
            {
                SetPoseFromJson = false;
                rigPose = RigPose.FromJson(FromJsonPose);
                SetJsonPoseControls();

            }


        }

        public void SaveItemInfo()
        {
            string assetPath = string.Empty;

            assetPath = string.Format(@"{0}{1}.json", RigPose.PosesPath, rigPose.PoseName);
            using (FileStream fs = new FileStream(assetPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(ToJsonPose);
                }
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }


        private void SetPose()
        {

            rigPose = new RigPose();

            rigPose.RootControl = Rig.RootControl;
            rigPose.CenterOfMass = Rig.CenterOfMass;

            rigPose.LookAtTarget = Rig.LookAtTarget;//.transform;
            rigPose.HeadControl = Rig.HeadControl;//.transform;
            rigPose.NeckControl = Rig.NeckControl;//.transform;
            rigPose.ChestControl = Rig.ChestControl;//.transform;
            rigPose.HipsControl = Rig.HipsControl;//.transform;

            rigPose.LeftHandIKTarget = Rig.LeftHandIKTarget;//.transform;
            rigPose.RightHandIKTarget = Rig.RightHandIKTarget;//.transform;

            rigPose.LeftFootIKTarget = Rig.LeftFootIKTarget;//.transform;
            rigPose.RightFootIKTarget = Rig.RightFootIKTarget;//.transform;

            rigPose.LeftHandIKPole = Rig.LeftHandIKPole;//.transform;
            rigPose.RightHandIKPole = Rig.RightHandIKPole;//.transform;

            rigPose.LeftFootIKPole = Rig.LeftFootIKPole;//.transform;
            rigPose.RightFootIKPole = Rig.RightFootIKPole;//.transform;

        }

        private void SetJsonPoseControls()
        {
            Rig.RootControl.transform.position = rigPose.RootControl.Position;
            Rig.RootControl.transform.rotation = rigPose.RootControl.Rotation;
            Rig.RootControl.transform.localScale = rigPose.RootControl.Scale;

            Rig.CenterOfMass.transform.position = rigPose.CenterOfMass.Position;
            Rig.CenterOfMass.transform.rotation = rigPose.CenterOfMass.Rotation;
            Rig.CenterOfMass.transform.localScale = rigPose.CenterOfMass.Scale;


            Rig.LookAtTarget.transform.position = rigPose.LookAtTarget.Position;
            Rig.LookAtTarget.transform.rotation = rigPose.LookAtTarget.Rotation;
            Rig.LookAtTarget.transform.localScale = rigPose.LookAtTarget.Scale;

            Rig.HeadControl.transform.position = rigPose.HeadControl.Position;
            Rig.HeadControl.transform.rotation = rigPose.HeadControl.Rotation;
            Rig.HeadControl.transform.localScale = rigPose.HeadControl.Scale;

            Rig.NeckControl.transform.position = rigPose.NeckControl.Position;
            Rig.NeckControl.transform.rotation = rigPose.NeckControl.Rotation;
            Rig.NeckControl.transform.localScale = rigPose.NeckControl.Scale;

            Rig.ChestControl.transform.position = rigPose.ChestControl.Position;
            Rig.ChestControl.transform.rotation = rigPose.ChestControl.Rotation;
            Rig.ChestControl.transform.localScale = rigPose.ChestControl.Scale;

            Rig.HipsControl.transform.position = rigPose.HipsControl.Position;
            Rig.HipsControl.transform.rotation = rigPose.HipsControl.Rotation;
            Rig.HipsControl.transform.localScale = rigPose.HipsControl.Scale;


            Rig.LeftHandIKTarget.transform.position = rigPose.LeftHandIKTarget.Position;
            Rig.LeftHandIKTarget.transform.rotation = rigPose.LeftHandIKTarget.Rotation;
            Rig.LeftHandIKTarget.transform.localScale = rigPose.LeftHandIKTarget.Scale;

            Rig.RightHandIKTarget.transform.position = rigPose.RightHandIKTarget.Position;
            Rig.RightHandIKTarget.transform.rotation = rigPose.RightHandIKTarget.Rotation;
            Rig.RightHandIKTarget.transform.localScale = rigPose.RightHandIKTarget.Scale;

            Rig.LeftFootIKTarget.transform.position = rigPose.LeftFootIKTarget.Position;
            Rig.LeftFootIKTarget.transform.rotation = rigPose.LeftFootIKTarget.Rotation;
            Rig.LeftFootIKTarget.transform.localScale = rigPose.LeftFootIKTarget.Scale;

            Rig.RightFootIKTarget.transform.position = rigPose.RightFootIKTarget.Position;
            Rig.RightFootIKTarget.transform.rotation = rigPose.RightFootIKTarget.Rotation;
            Rig.RightFootIKTarget.transform.localScale = rigPose.RightFootIKTarget.Scale;

            Rig.LeftHandIKPole.transform.position = rigPose.LeftHandIKPole.Position;
            Rig.LeftHandIKPole.transform.rotation = rigPose.LeftHandIKPole.Rotation;
            Rig.LeftHandIKPole.transform.localScale = rigPose.LeftHandIKPole.Scale;

            Rig.RightHandIKPole.transform.position = rigPose.RightHandIKPole.Position;
            Rig.RightHandIKPole.transform.rotation = rigPose.RightHandIKPole.Rotation;
            Rig.RightHandIKPole.transform.localScale = rigPose.RightHandIKPole.Scale;

            Rig.LeftFootIKPole.transform.position = rigPose.LeftFootIKPole.Position;
            Rig.LeftFootIKPole.transform.rotation = rigPose.LeftFootIKPole.Rotation;
            Rig.LeftFootIKPole.transform.localScale = rigPose.LeftFootIKPole.Scale;

            Rig.RightFootIKPole.transform.position = rigPose.RightFootIKPole.Position;
            Rig.RightFootIKPole.transform.rotation = rigPose.RightFootIKPole.Rotation;
            Rig.RightFootIKPole.transform.localScale = rigPose.RightFootIKPole.Scale;

        }


    }
}