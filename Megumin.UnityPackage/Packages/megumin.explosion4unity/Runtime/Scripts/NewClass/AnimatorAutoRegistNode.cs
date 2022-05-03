using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorAutoRegistNode : MonoBehaviour
    {
        private void Awake()
        {
            Regist();
        }

        [Button]
        public void Regist()
        {
            var a = GetComponent<Animator>();
            if (a && a.isHuman)
            {
                var finder = gameObject.GetComponentInParent<ITransformNodeFinder<HumanBodyBones>>();
                if (finder == null)
                {
                    var strFinder = gameObject.GetComponentInParent<ITransformNodeFinder<string>>();
                    foreach (HumanBodyBones item in Enum.GetValues(typeof(HumanBodyBones)))
                    {
                        if (item != HumanBodyBones.LastBone)
                        {
                            var target = a.GetBoneTransform(item);
                            if (target)
                            {
                                strFinder.Regist(item.ToString(), target);
                            }
                        }
                    }
                }
                else
                {
                    foreach (HumanBodyBones item in Enum.GetValues(typeof(HumanBodyBones)))
                    {
                        if (item != HumanBodyBones.LastBone)
                        {
                            var target = a.GetBoneTransform(item);
                            if (target)
                            {
                                finder.Regist(item, target);
                            }
                        }
                    }
                }
            }
        }
    }

}
