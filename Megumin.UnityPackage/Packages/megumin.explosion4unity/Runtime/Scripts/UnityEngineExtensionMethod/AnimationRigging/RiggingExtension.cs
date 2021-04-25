#if MEGUMIN_RIGGING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace UnityEngine.Animations.Rigging
{
    public static class RiggingExtension_6CC6A4A4
    {
        public static void Enable(this MultiParentConstraint constraint, int index = 0)
        {
            if (constraint)
            {
                var temp = constraint.data.sourceObjects;
                for (int i = 0; i < temp.Count; i++)
                {
                    temp.SetWeight(i, i == index ? 1 : 0);
                }

                constraint.data.sourceObjects = temp;
            }
        }

        //public static void LerpTo(this MultiParentConstraint constraint, int index = 0)
        //{
            
        //}
    }
}


#endif

