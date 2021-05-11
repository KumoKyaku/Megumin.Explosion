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
    public IAnimatorCallback Callback { get; set; }

    public Object CallbackTarget;

    public Transform OverrideRoot;
    void Awake()
    {
        Animator = GetComponent<Animator>();
        BindCallback(CallbackTarget);
    }

    public void BindCallback(Object callBackBindTarget)
    {
        if (CallbackTarget != callBackBindTarget)
        {
            CallbackTarget = callBackBindTarget;
        }

        if (CallbackTarget)
        {
            if (CallbackTarget is IAnimatorCallback callback)
            {
                Callback = callback;
            }
        }
    }

    private void OnValidate()
    {
        BindCallback(CallbackTarget);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (Callback != null)
        {
            Callback.OnAnimatorIKCallback(Animator, layerIndex);
        }
    }

    void OnAnimatorMove()
    {
        if (Callback != null)
        {
            Callback.OnAnimatorMoveCallback(Animator);
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



