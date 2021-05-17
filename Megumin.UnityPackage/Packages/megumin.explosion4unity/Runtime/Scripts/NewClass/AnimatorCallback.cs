using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatorCallback
{
    void OnAnimatorMoveCallback(Animator animator);
    void OnAnimatorIKCallback(Animator animator, int layerIndex);
}

[ExecuteAlways]
[RequireComponent(typeof(Animator))]
public class AnimatorCallback : MonoBehaviour
{
    [ReadOnlyInInspector]
    public Animator Animator;
    public HashSet<IAnimatorCallback> Callback { get; } = new HashSet<IAnimatorCallback>();

    public Object[] CallbackTarget;

    public Transform OverrideRoot;
    void Awake()
    {
        Animator = GetComponent<Animator>();
        BindCallback(CallbackTarget);
    }

    public void BindCallback(Object[] callBackBindTarget)
    {
        if (callBackBindTarget != null)
        {
            foreach (var item in callBackBindTarget)
            {
                if (item is IAnimatorCallback callback)
                {
                    Callback.Add(callback);
                }
            }
        }
    }

    private void OnValidate()
    {
        BindCallback(CallbackTarget);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (Callback?.Count > 0)
        {
            foreach (var item in Callback)
            {
                item.OnAnimatorMoveCallback(Animator);
            }
        }
    }

    void OnAnimatorMove()
    {
        if (Callback?.Count > 0)
        {
            foreach (var item in Callback)
            {
                item.OnAnimatorMoveCallback(Animator);
            }
        }
        else
        {
            if (OverrideRoot)
            {
                OverrideRoot.Translate(Animator.deltaPosition, Space.World);
                OverrideRoot.rotation *= Animator.deltaRotation;
            }
            else
            {
                Animator.ApplyBuiltinRootMotion();
            }
        }
    }
}



