using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using System.IO;

namespace Assets.Script.NyshaRig
{
    public class RigAnimCreator:MonoBehaviour
    {

        public RigAnimation RigAnim;
        public bool NewRigAnim = false;
        public List<string> Anims;
        public void Start()
        {
            DirectoryInfo dInfo = new DirectoryInfo("Assets/Animations/Poses/");
            foreach (FileInfo fInfo in dInfo.GetFiles("*.json", SearchOption.AllDirectories))
            {
                Anims.Add(fInfo.Name);
            } 
        }

        public void Update()
        {

            if(NewRigAnim)
            {
                NewRigAnim = false;
            }
        }



    }
}
