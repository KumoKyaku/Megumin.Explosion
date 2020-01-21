using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 骨骼类扩展
/// </summary>
public static class HumanBodyBonesExtension_97551F106E014682A5820F103C7B301B
{
    /// <summary>
    /// 骨骼的父关系
    /// </summary>
    /// <param name="bone"></param>
    /// <returns>当参数为Hips时返回LastBone</returns>
    public static HumanBodyBones GetParent(this HumanBodyBones bone)
    {
        switch (bone)
        {
            case HumanBodyBones.Hips:
                //return HumanBodyBones.LastBone;
                break;
            case HumanBodyBones.LeftUpperLeg:
                return HumanBodyBones.Hips;

            case HumanBodyBones.RightUpperLeg:
                return HumanBodyBones.Hips;

            case HumanBodyBones.LeftLowerLeg:
                return HumanBodyBones.LeftUpperLeg;

            case HumanBodyBones.RightLowerLeg:
                return HumanBodyBones.RightUpperLeg;

            case HumanBodyBones.LeftFoot:
                return HumanBodyBones.LeftLowerLeg;

            case HumanBodyBones.RightFoot:
                return HumanBodyBones.RightLowerLeg;

            case HumanBodyBones.Spine:
                return HumanBodyBones.Hips;

            case HumanBodyBones.Chest:
                return HumanBodyBones.Spine;

            case HumanBodyBones.Neck:
                return HumanBodyBones.Chest;

            case HumanBodyBones.Head:
                return HumanBodyBones.Neck;

            case HumanBodyBones.LeftShoulder:
                return HumanBodyBones.Chest;

            case HumanBodyBones.RightShoulder:
                return HumanBodyBones.Chest;

            case HumanBodyBones.LeftUpperArm:
                return HumanBodyBones.LeftShoulder;

            case HumanBodyBones.RightUpperArm:
                return HumanBodyBones.RightShoulder;

            case HumanBodyBones.LeftLowerArm:
                return HumanBodyBones.LeftUpperArm;

            case HumanBodyBones.RightLowerArm:
                return HumanBodyBones.RightLowerArm;

            case HumanBodyBones.LeftHand:
                return HumanBodyBones.LeftLowerArm;

            case HumanBodyBones.RightHand:
                return HumanBodyBones.RightLowerArm;

            case HumanBodyBones.LeftToes:
                return HumanBodyBones.LeftLowerLeg;

            case HumanBodyBones.RightToes:
                return HumanBodyBones.RightLowerLeg;


            //以下部分在后续应用中再视具体情况进行修改
            case HumanBodyBones.LeftEye:
                return HumanBodyBones.Head;

            case HumanBodyBones.RightEye:
                return HumanBodyBones.Head;

            case HumanBodyBones.Jaw:
                return HumanBodyBones.Head;

            case HumanBodyBones.LeftThumbProximal:
                return HumanBodyBones.LeftHand;

            case HumanBodyBones.LeftThumbIntermediate:
                return HumanBodyBones.LeftThumbProximal;

            case HumanBodyBones.LeftThumbDistal:
                return HumanBodyBones.LeftThumbIntermediate;

            case HumanBodyBones.LeftIndexProximal:
                return HumanBodyBones.LeftHand;

            case HumanBodyBones.LeftIndexIntermediate:
                return HumanBodyBones.LeftIndexProximal;

            case HumanBodyBones.LeftIndexDistal:
                return HumanBodyBones.LeftIndexIntermediate;

            case HumanBodyBones.LeftMiddleProximal:
                return HumanBodyBones.LeftHand;

            case HumanBodyBones.LeftMiddleIntermediate:
                return HumanBodyBones.LeftMiddleProximal;

            case HumanBodyBones.LeftMiddleDistal:
                return HumanBodyBones.LeftMiddleIntermediate;

            case HumanBodyBones.LeftRingProximal:
                return HumanBodyBones.LeftHand;

            case HumanBodyBones.LeftRingIntermediate:
                return HumanBodyBones.LeftRingProximal;

            case HumanBodyBones.LeftRingDistal:
                return HumanBodyBones.LeftRingIntermediate;

            case HumanBodyBones.LeftLittleProximal:
                return HumanBodyBones.LeftHand;

            case HumanBodyBones.LeftLittleIntermediate:
                return HumanBodyBones.LeftLittleProximal;

            case HumanBodyBones.LeftLittleDistal:
                return HumanBodyBones.LeftLittleIntermediate;

            case HumanBodyBones.RightThumbProximal:
                return HumanBodyBones.RightHand;

            case HumanBodyBones.RightThumbIntermediate:
                return HumanBodyBones.RightThumbProximal;

            case HumanBodyBones.RightThumbDistal:
                return HumanBodyBones.RightThumbIntermediate;

            case HumanBodyBones.RightIndexProximal:
                return HumanBodyBones.RightHand;

            case HumanBodyBones.RightIndexIntermediate:
                return HumanBodyBones.RightIndexProximal;

            case HumanBodyBones.RightIndexDistal:
                return HumanBodyBones.RightIndexIntermediate;

            case HumanBodyBones.RightMiddleProximal:
                return HumanBodyBones.RightHand;

            case HumanBodyBones.RightMiddleIntermediate:
                return HumanBodyBones.RightMiddleProximal;

            case HumanBodyBones.RightMiddleDistal:
                return HumanBodyBones.RightMiddleIntermediate;

            case HumanBodyBones.RightRingProximal:
                return HumanBodyBones.RightHand;

            case HumanBodyBones.RightRingIntermediate:
                return HumanBodyBones.RightRingProximal;

            case HumanBodyBones.RightRingDistal:
                return HumanBodyBones.RightRingIntermediate;

            case HumanBodyBones.RightLittleProximal:
                return HumanBodyBones.RightHand;

            case HumanBodyBones.RightLittleIntermediate:
                return HumanBodyBones.RightLittleProximal;

            case HumanBodyBones.RightLittleDistal:
                return HumanBodyBones.RightLittleIntermediate;

            case HumanBodyBones.LastBone:
                break;
            default:
                break;
        }
        return HumanBodyBones.LastBone;
    }

    /// <summary>
    /// 骨骼的第一子关系
    /// </summary>
    /// <param name="bone"></param>
    /// <returns>没有子时返回LastBone</returns>
    public static HumanBodyBones FirstChild(this HumanBodyBones bone)
    {
        switch (bone)
        {
            case HumanBodyBones.Hips:
                //child为腰
                return HumanBodyBones.Spine;

            case HumanBodyBones.LeftUpperLeg:
                return HumanBodyBones.LeftLowerLeg;

            case HumanBodyBones.RightUpperLeg:
                return HumanBodyBones.RightLowerLeg;

            case HumanBodyBones.LeftLowerLeg:
                return HumanBodyBones.LeftFoot;

            case HumanBodyBones.RightLowerLeg:
                return HumanBodyBones.RightFoot;

            case HumanBodyBones.LeftFoot:
                return HumanBodyBones.LeftToes;

            case HumanBodyBones.RightFoot:
                return HumanBodyBones.RightToes;

            case HumanBodyBones.Spine:
                return HumanBodyBones.Chest;

            case HumanBodyBones.Chest:
                return HumanBodyBones.Neck;

            case HumanBodyBones.Neck:
                return HumanBodyBones.Head;

            case HumanBodyBones.Head:
                //child为下颚
                return HumanBodyBones.Jaw;

            case HumanBodyBones.LeftShoulder:
                return HumanBodyBones.LeftUpperArm;

            case HumanBodyBones.RightShoulder:
                return HumanBodyBones.RightUpperArm;

            case HumanBodyBones.LeftUpperArm:
                return HumanBodyBones.LeftLowerArm;

            case HumanBodyBones.RightUpperArm:
                return HumanBodyBones.RightLowerArm;

            case HumanBodyBones.LeftLowerArm:
                return HumanBodyBones.LeftHand;

            case HumanBodyBones.RightLowerArm:
                return HumanBodyBones.RightHand;

            case HumanBodyBones.LeftHand:
                //child为拇指近端
                return HumanBodyBones.LeftThumbProximal;

            case HumanBodyBones.RightHand:
                //child为拇指近端
                return HumanBodyBones.RightThumbProximal;

            case HumanBodyBones.LeftToes:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.RightToes:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.LeftEye:
                /////无child处理方式
                //////
                break;
            case HumanBodyBones.RightEye:
                /////无child处理方式
                //////
                break;
            case HumanBodyBones.Jaw:
                /////无child处理方式
                //////
                break;
            case HumanBodyBones.LeftThumbProximal:
                return HumanBodyBones.LeftThumbIntermediate;

            case HumanBodyBones.LeftThumbIntermediate:
                return HumanBodyBones.LeftThumbDistal;
            case HumanBodyBones.LeftThumbDistal:
                /////无child处理方式
                //////
                break;
            case HumanBodyBones.LeftIndexProximal:
                return HumanBodyBones.LeftIndexIntermediate;
            case HumanBodyBones.LeftIndexIntermediate:
                return HumanBodyBones.LeftIndexDistal;
            case HumanBodyBones.LeftIndexDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.LeftMiddleProximal:
                return HumanBodyBones.LeftMiddleIntermediate;
            case HumanBodyBones.LeftMiddleIntermediate:
                return HumanBodyBones.LeftMiddleDistal;
            case HumanBodyBones.LeftMiddleDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.LeftRingProximal:
                return HumanBodyBones.LeftRingIntermediate;
            case HumanBodyBones.LeftRingIntermediate:
                return HumanBodyBones.LeftRingDistal;
            case HumanBodyBones.LeftRingDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.LeftLittleProximal:
                return HumanBodyBones.LeftLittleIntermediate;
            case HumanBodyBones.LeftLittleIntermediate:
                return HumanBodyBones.LeftLittleDistal;
            case HumanBodyBones.LeftLittleDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.RightThumbProximal:
                return HumanBodyBones.RightThumbIntermediate;
            case HumanBodyBones.RightThumbIntermediate:
                return HumanBodyBones.RightThumbDistal;
            case HumanBodyBones.RightThumbDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.RightIndexProximal:
                return HumanBodyBones.RightIndexIntermediate;
            case HumanBodyBones.RightIndexIntermediate:
                return HumanBodyBones.RightIndexDistal;
            case HumanBodyBones.RightIndexDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.RightMiddleProximal:
                return HumanBodyBones.RightMiddleIntermediate;
            case HumanBodyBones.RightMiddleIntermediate:
                return HumanBodyBones.RightMiddleDistal;
            case HumanBodyBones.RightMiddleDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.RightRingProximal:
                return HumanBodyBones.RightRingIntermediate;
            case HumanBodyBones.RightRingIntermediate:
                return HumanBodyBones.RightRingDistal;
            case HumanBodyBones.RightRingDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.RightLittleProximal:
                return HumanBodyBones.RightLittleIntermediate;
            case HumanBodyBones.RightLittleIntermediate:
                return HumanBodyBones.RightLittleDistal;
            case HumanBodyBones.RightLittleDistal:
                //////无child处理方式
                //////
                break;
            case HumanBodyBones.LastBone:
                //////无child处理方式
                //////
                break;
            default:
                break;



        }

        return HumanBodyBones.LastBone;
    }

    /// <summary>
    /// 骨骼子关系
    /// </summary>
    /// <param name="bone"></param>
    /// <returns></returns>
    public static List<HumanBodyBones> Children(this HumanBodyBones bone)
    {
        List<HumanBodyBones> children = new List<HumanBodyBones>();

        switch (bone)
        {
            case HumanBodyBones.Hips:
                children.Add(HumanBodyBones.Spine);
                children.Add(HumanBodyBones.LeftUpperLeg);
                children.Add(HumanBodyBones.RightUpperLeg);
                break;

            case HumanBodyBones.Spine:
                children.Add(HumanBodyBones.Chest);
                break;

            case HumanBodyBones.Chest:
                children.Add(HumanBodyBones.Neck);
                children.Add(HumanBodyBones.LeftShoulder);
                children.Add(HumanBodyBones.RightShoulder);
                break;

            case HumanBodyBones.Neck:
                children.Add(HumanBodyBones.Head);
                break;

            case HumanBodyBones.Head:
                children.Add(HumanBodyBones.Jaw);
                children.Add(HumanBodyBones.LeftEye);
                children.Add(HumanBodyBones.RightEye);
                break;

            case HumanBodyBones.Jaw:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.LeftEye:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.RightEye:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.LeftUpperLeg:
                children.Add(HumanBodyBones.LeftLowerLeg);
                break;

            case HumanBodyBones.RightUpperLeg:
                children.Add(HumanBodyBones.RightLowerLeg);
                break;

            case HumanBodyBones.LeftLowerLeg:
                children.Add(HumanBodyBones.LeftFoot);
                break;

            case HumanBodyBones.RightLowerLeg:
                children.Add(HumanBodyBones.RightFoot);
                break;

            case HumanBodyBones.LeftFoot:
                children.Add(HumanBodyBones.LeftToes);
                break;

            case HumanBodyBones.RightFoot:
                children.Add(HumanBodyBones.RightToes);
                break;

            case HumanBodyBones.LeftToes:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.RightToes:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.LeftShoulder:
                children.Add(HumanBodyBones.LeftUpperArm);
                break;

            case HumanBodyBones.RightShoulder:
                children.Add(HumanBodyBones.RightUpperArm);
                break;

            case HumanBodyBones.LeftUpperArm:
                children.Add(HumanBodyBones.LeftLowerArm);
                break;

            case HumanBodyBones.RightUpperArm:
                children.Add(HumanBodyBones.RightLowerArm);
                break;

            case HumanBodyBones.LeftLowerArm:
                children.Add(HumanBodyBones.LeftHand);
                break;

            case HumanBodyBones.RightLowerArm:
                children.Add(HumanBodyBones.RightHand);
                break;

            case HumanBodyBones.LeftHand:
                children.Add(HumanBodyBones.LeftThumbProximal);
                children.Add(HumanBodyBones.LeftIndexProximal);
                children.Add(HumanBodyBones.LeftMiddleProximal);
                children.Add(HumanBodyBones.LeftRingProximal);
                children.Add(HumanBodyBones.LeftLittleProximal);
                break;

            case HumanBodyBones.RightHand:
                children.Add(HumanBodyBones.RightThumbProximal);
                children.Add(HumanBodyBones.RightIndexProximal);
                children.Add(HumanBodyBones.RightMiddleProximal);
                children.Add(HumanBodyBones.RightRingProximal);
                children.Add(HumanBodyBones.RightLittleProximal);
                break;

            case HumanBodyBones.LeftThumbProximal:
                children.Add(HumanBodyBones.LeftThumbIntermediate);
                break;
            case HumanBodyBones.LeftThumbIntermediate:
                children.Add(HumanBodyBones.LeftThumbDistal);
                break;
            case HumanBodyBones.LeftThumbDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.LeftIndexProximal:
                children.Add(HumanBodyBones.LeftIndexIntermediate);
                break;
            case HumanBodyBones.LeftIndexIntermediate:
                children.Add(HumanBodyBones.LeftIndexDistal);
                break;
            case HumanBodyBones.LeftIndexDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.LeftMiddleProximal:
                children.Add(HumanBodyBones.LeftMiddleIntermediate);
                break;
            case HumanBodyBones.LeftMiddleIntermediate:
                children.Add(HumanBodyBones.LeftMiddleDistal);
                break;
            case HumanBodyBones.LeftMiddleDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.LeftRingProximal:
                children.Add(HumanBodyBones.LeftRingIntermediate);
                break;
            case HumanBodyBones.LeftRingIntermediate:
                children.Add(HumanBodyBones.LeftRingDistal);
                break;
            case HumanBodyBones.LeftRingDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.LeftLittleProximal:
                children.Add(HumanBodyBones.LeftLittleIntermediate);
                break;
            case HumanBodyBones.LeftLittleIntermediate:
                children.Add(HumanBodyBones.LeftLittleDistal);
                break;
            case HumanBodyBones.LeftLittleDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.RightThumbProximal:
                children.Add(HumanBodyBones.RightThumbIntermediate);
                break;
            case HumanBodyBones.RightThumbIntermediate:
                children.Add(HumanBodyBones.RightThumbDistal);
                break;
            case HumanBodyBones.RightThumbDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.RightIndexProximal:
                children.Add(HumanBodyBones.RightIndexIntermediate);
                break;
            case HumanBodyBones.RightIndexIntermediate:
                children.Add(HumanBodyBones.RightIndexDistal);
                break;
            case HumanBodyBones.RightIndexDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.RightMiddleProximal:
                children.Add(HumanBodyBones.RightMiddleIntermediate);
                break;
            case HumanBodyBones.RightMiddleIntermediate:
                children.Add(HumanBodyBones.RightMiddleDistal);
                break;
            case HumanBodyBones.RightMiddleDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.RightRingProximal:
                children.Add(HumanBodyBones.RightRingIntermediate);
                break;
            case HumanBodyBones.RightRingIntermediate:
                children.Add(HumanBodyBones.RightRingDistal);
                break;
            case HumanBodyBones.RightRingDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            case HumanBodyBones.RightLittleProximal:
                children.Add(HumanBodyBones.RightLittleIntermediate);
                break;
            case HumanBodyBones.RightLittleIntermediate:
                children.Add(HumanBodyBones.RightLittleDistal);
                break;
            case HumanBodyBones.RightLittleDistal:
                children.Add(HumanBodyBones.LastBone);
                break;

            ////末梢节点的子节点仍为末梢节点，此处若循环引用会死循环，后期视情况改正
            case HumanBodyBones.LastBone:
                children.Add(HumanBodyBones.LastBone);
                break;
        }

        return children;
    }
}
