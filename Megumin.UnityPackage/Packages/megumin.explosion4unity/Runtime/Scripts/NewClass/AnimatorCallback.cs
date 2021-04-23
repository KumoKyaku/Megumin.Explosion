using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatorCallback
{
    void OnAnimatorMove(Animator animator);
    void OnAnimatorIK(Animator animator, int layerIndex);
}

[ExecuteAlways]
[RequireComponent(typeof(Animator))]
public class AnimatorCallback : MonoBehaviour
{
    [ReadOnlyInInspector]
    public Animator Animator;
    public IAnimatorCallback Callback { get; set; }

    public Transform OverrideRoot;
    void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (Callback != null)
        {
            Callback.OnAnimatorIK(Animator, layerIndex);
        }
    }

    void OnAnimatorMove()
    {
        if (Callback != null)
        {
            Callback.OnAnimatorMove(Animator);
        }
        else
        {
            if (OverrideRoot)
            {
                OverrideRoot.Translate(Animator.deltaPosition);
                OverrideRoot.rotation *= Animator.deltaRotation;
            }
            else
            {
                Animator.ApplyBuiltinRootMotion();
            }
        }
    }
}



