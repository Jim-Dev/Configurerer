using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.NyshaRig
{
    public interface IExercisePlayer
    {
        void InitializeExercise(Exercise exercise, BehaviourParams bParam);
        void InitializeWebExercise(string jsonString);

        void SetRestPose();

        void StartExercise(bool isInInstruction);
        void StartWebExercise(string jsonString);
        void StartExerciseNoParams();

        void ResumePauseExercise();
        void StopExercise();

        event EventHandler<RepetitionStartEventArgs> OnRepetitionStart;
        event EventHandler OnRepetitionEnd;
        event EventHandler OnSubrepetitionEnd;
        event EventHandler OnRepetitionReallyStart;
        event EventHandler<PrepareEventArgs> OnInitializeExerciseStart;
        event EventHandler<PrepareEventArgs> OnInitializeExerciseEnd;
    }
}
