using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
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
        public bool EnableCallback = true;
        public List<Object> CallbackTarget;

        public Transform OverrideRoot;

        [Space]
        public bool LogDelta = false;
        [ReadOnlyInInspector]
        public Vector3 TotalDelta = default;
        [ReadOnlyInInspector]
        public Vector3 TotalAngel = default;
        [ReadOnlyInInspector]
        public Quaternion TotalQ = Quaternion.identity;

        void Awake()
        {
            Animator = GetComponent<Animator>();
            BindCallback(CallbackTarget);
        }

        public void Start()
        {

        }

        public void BindCallback<T>(T callBackBindTarget)
            where T : IEnumerable<Object>
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
            if (Callback?.Count > 0 && EnableCallback)
            {
                foreach (var item in Callback)
                {
                    item.OnAnimatorMoveCallback(Animator);
                }
            }
        }

        void OnAnimatorMove()
        {
            if (LogDelta)
            {
                TotalDelta += Animator.deltaPosition;
                TotalQ *= Animator.deltaRotation;
                TotalAngel = TotalQ.eulerAngles;
                Debug.Log($"Frame:{Time.frameCount} -- deltaPosition:{Animator.deltaPosition} -- deltaRotation:{Animator.deltaRotation} -- angle:{Animator.deltaRotation.eulerAngles}" +
                    $" -- TotalDelta:{TotalDelta} -- TotalAngel:{TotalAngel}");
            }

            if (Callback?.Count > 0 && EnableCallback)
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

        [EditorButton]
        public void ClearLogDelta()
        {
            TotalDelta = default;
            TotalAngel = default;
            TotalQ = Quaternion.identity;
        }
    }


}



