//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

using Assets.Script.NyshaRig;
using UnityEngine;
using Assets.Script;


[CustomEditor(typeof(RigSetup))]
[CanEditMultipleObjects]
public class RigSetupEditor : Editor
{
    /*
    RigSetup rigSetup;
   bool groupEnabled=true;

    public override void OnInspectorGUI()
    {
        rigSetup = (RigSetup)target;



        GUILayout.Label("Base Settings", EditorStyles.boldLabel);


        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
      EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal();

        rigSetup.RootControl = EditorGUILayout.ObjectField("", rigSetup.RootControl, typeof(Transform), false) as Transform;
       
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndToggleGroup();
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);


    }
    */
}

