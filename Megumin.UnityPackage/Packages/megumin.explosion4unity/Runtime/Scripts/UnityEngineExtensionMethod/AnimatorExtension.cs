﻿using Megumin;
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
        if (humanBoneId != HumanBodyBones.LastBone)
        {
            //通常使用子对象坐标判断轴向
            var foot = animator.GetBoneTransform(humanBoneId);
            var locol = foot.transform.localPosition;
            return GetAxial(locol);
        }
        else
        {
            //不存在子对象时，通过自身坐标，计算父骨骼轴向，当作自身轴向。
            //通常认为末节点骨骼轴向于父骨骼轴向相同。
            if (bone != HumanBodyBones.LastBone)
            {
                var foot = animator.GetBoneTransform(bone);
                var locol = foot.transform.localPosition;
                return GetAxial(locol);
            }
        }
        return AxialAigned.None;
    }

    /// <summary>
    /// 用与在OnAnimatorMove中调用，计算速度等。
    /// https://docs.unity3d.com/ScriptReference/Time-deltaTime.html
    /// </summary>
    /// <returns></returns>
    public static float GetOnAnimatorMoveTime(this Animator animator)
    {
        if (animator)
        {
            switch (animator.updateMode)
            {
                case AnimatorUpdateMode.Normal:
                    return Time.deltaTime;
#if UNITY_2023_1_OR_NEWER
                case AnimatorUpdateMode.Fixed:
                    return Time.deltaTime;
#else
                case AnimatorUpdateMode.AnimatePhysics: 
                    return Time.deltaTime;
#endif
                case AnimatorUpdateMode.UnscaledTime:
                    return Time.unscaledDeltaTime;
                default:
                    break;
            }
        }

        //这里不要返回0，防止/0错误。
        return float.Epsilon;
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
