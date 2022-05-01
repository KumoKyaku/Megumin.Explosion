using Megumin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtension_B0724B3FE1814EEB809D274438B254A6
{
    /// <summary>
    /// 取得指定骨骼轴向
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="bone"></param>
    /// <returns></returns>
    public static AxialAigned GetAxial(this Animator animator, HumanBodyBones bone)
    {
        HumanBodyBones humanBoneId = bone.FirstChild();
        var foot = animator.GetBoneTransform(humanBoneId);
        var locol = foot.transform.localPosition;
        return GetAxial(locol);
    }

    public static AxialAigned GetAxial(Vector3 locol)
    {
        AxialAigned axial = AxialAigned.None;
        var absx = Mathf.Abs(locol.x);
        var absy = Mathf.Abs(locol.y);
        var absz = Mathf.Abs(locol.z);

        if (absx > absy)
        {
            if (absx > absz)
            {
                if (locol.x > 0)
                {
                    axial = AxialAigned.X;
                }
                else
                {
                    axial = AxialAigned.Xn;
                }
            }
            else
            {
                if (locol.z > 0)
                {
                    axial = AxialAigned.Z;
                }
                else
                {
                    axial = AxialAigned.Zn;
                }
            }
        }
        else
        {
            if (absy > absz)
            {
                if (locol.y > 0)
                {
                    axial = AxialAigned.Y;
                }
                else
                {
                    axial = AxialAigned.Yn;
                }
            }
            else
            {
                if (locol.z > 0)
                {
                    axial = AxialAigned.Z;
                }
                else
                {
                    axial = AxialAigned.Zn;
                }
            }
        }

        return axial;
    }
}
