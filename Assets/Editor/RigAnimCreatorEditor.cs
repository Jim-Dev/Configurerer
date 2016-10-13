//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

using Assets.Script.NyshaRig;
using UnityEngine;
using Assets.Script;


[CustomEditor(typeof(RigAnimCreator))]
[CanEditMultipleObjects]
public class RigAnimCreatorEditor : Editor
{
    
    RigAnimCreator rigAnimCreator;
   //bool groupEnabled=true;
    int SelectedPoseIndex=0;
    string[] itemsArray;
    float frameDuration = 0;
    public override void OnInspectorGUI()
    {
        rigAnimCreator = (RigAnimCreator)target;
        itemsArray = rigAnimCreator.Anims.ToArray();


        GUILayout.Label("Animation ", EditorStyles.boldLabel);
        GUILayout.Button("NewAnimation");

        //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        // EditorGUILayout.BeginHorizontal();
        rigAnimCreator.RigAnim.AnimationName=EditorGUILayout.TextField("Name ", rigAnimCreator.RigAnim.AnimationName);
        EditorGUILayout.LabelField("Total frames :: ",
               rigAnimCreator.RigAnim.AnimationFrames.Count.ToString());
        EditorGUILayout.LabelField("Total duration :: ",
               rigAnimCreator.RigAnim.GetTotalDuration().ToString());

        EditorGUILayout.BeginVertical();

        foreach (RigKeyFrame rigKFrame in rigAnimCreator.RigAnim.AnimationFrames)
        {
            GUILayout.Label(string.Format("Name : {0} Duration : {1}",rigKFrame.Pose.PoseName,rigKFrame.Duration), EditorStyles.boldLabel);
        }

        EditorGUILayout.EndVertical();

        frameDuration = EditorGUILayout.FloatField("FrameDuration:", frameDuration);
        if (GUILayout.Button("AddNewFrame"))
        {
          
            AddKeyFrame(frameDuration,RigPose.LoadPoseFromAsset(itemsArray[SelectedPoseIndex]));
                }
        SelectedPoseIndex = EditorGUILayout.Popup("Add pose to anim", SelectedPoseIndex, itemsArray);

        // EditorGUILayout.EndHorizontal();

        //EditorGUILayout.EndToggleGroup();
        if (GUILayout.Button("Save Animation"))
            SaveAnimation();


    }

    private void AddKeyFrame(float duration, RigPose pose)
    {
        RigKeyFrame rigKFrame = new RigKeyFrame();
        rigKFrame.Duration = duration;
        rigKFrame.Pose = pose;
        rigAnimCreator.RigAnim.AnimationFrames.Add(rigKFrame);
       // rigAnimCreator.RigAnim
    }

    private void SaveAnimation()
    {
        rigAnimCreator.RigAnim.WriteAsset(rigAnimCreator.RigAnim.AnimationName);
    }
    
}

