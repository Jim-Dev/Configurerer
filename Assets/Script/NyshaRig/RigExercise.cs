using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    [Serializable]
    public class RigExercise:JsonSerializable
    {

        public const string DEFAULT_ASSET_PATH = "Assets/Animations/Exercises/";

        public string ExerciseName;
        [SerializeField]
        List<AnimInfo> Animations;

        public RigExercise()
        {

        }

        public static RigExercise LoadFromFile(string fileName)
        {
            return JsonUtility.FromJson<RigExercise>(LoadJsonFile(RigExercise.DEFAULT_ASSET_PATH, fileName));
        }

        public override void SaveToFile(string fileName, string directoryPath)
        {
            base.SaveToFile(fileName, directoryPath);
        }

        public void SaveToFile(string fileName)
        {
            this.SaveToFile(fileName, DEFAULT_ASSET_PATH);
        }
    }

    [Serializable]
    public class AnimInfo
    {
        [SerializeField]
        RigAnimation Animation;
        [SerializeField]
        List<AnimTarget> OnEndTargets;

        eAnimStateType AnimStateType;
    }

    [Serializable]
    public class AnimTarget
    {
        [SerializeField]
        RigAnimation TargetAniamtion;
        [SerializeField]
        float TargetWeight;
    }

    public enum eAnimStateType
    {
        none,
        Rest,
        Extreme,
        Transition
    }
}
