using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script
{
    public class PoseTransitionBehaviour : StateMachineBehaviour
    {

        public AnimationClip RestPosition;
        public Animation ExtremePosition;

        public float RestTime;
        public float RestToExtremeTransitionTime;
        Animator anim;


        public PoseTransitionBehaviour()
        {
            
            //GetComponent<Animator>();
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("OnStateEnter");
            //stateInfo.
            //anim.Play()
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("OnStateEnter");
        }
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
    }
}
