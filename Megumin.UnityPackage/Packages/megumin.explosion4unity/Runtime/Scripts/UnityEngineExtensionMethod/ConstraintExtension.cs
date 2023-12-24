using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public static class ConstraintExtension_2BC2DF21765A4B2DA4F5FEAF623440E7
{
    public static void SetParent(this ParentConstraint constraint, Transform parent)
    {
        if (constraint)
        {
            constraint.SetSource(0, new ConstraintSource()
            {
                sourceTransform = parent,
                weight = 1
            });
        }
    }

    /// <summary>
    /// 标记显示一个对象
    /// </summary>
    /// <param name="constraint"></param>
    /// <param name="target"></param>
    public static void Mark(this ParentConstraint constraint, Transform target)
    {
        constraint.SetParent(target);
        constraint.gameObject.SetActive(target);
    }

    /// <summary>
    /// 标记显示一个对象
    /// </summary>
    /// <param name="constraint"></param>
    /// <param name="target"></param>
    public static void Mark(this ParentConstraint constraint, GameObject target)
    {
        constraint.SetParent(target?.transform);
        constraint.gameObject.SetActive(target?.transform);
    }

    /// <summary>
    /// 标记显示一个对象
    /// </summary>
    /// <param name="constraint"></param>
    /// <param name="target"></param>
    public static void Mark(this ParentConstraint constraint, Component target)
    {
        constraint.SetParent(target?.transform);
        constraint.gameObject.SetActive(target?.transform);
    }
}




