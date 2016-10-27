using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class RigAnimPlayer:MonoBehaviour
    {
        public RigAnimation RigAnimationSelected;

        public string RigAnimName;

        public bool LoadAnimation = false;
        public bool PlayAnimation = false;

        public void Start()
        {

        }

        public void Update()
        {
            if (LoadAnimation)
            {
                LoadAnimation = false;



            }

            if (PlayAnimation)
            {
                PlayAnimation = false;
            }
        }
    }
}
