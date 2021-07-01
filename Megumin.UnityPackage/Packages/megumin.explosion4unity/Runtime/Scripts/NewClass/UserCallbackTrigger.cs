using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Megumin
{
    public class UserCallbackTrigger : MonoBehaviour
    {
        public UnityEvent OnAwake;
        public UnityEvent OnOnEnable_;
        public UnityEvent OnStart;
        public UnityEvent OnDisable_;
        public UnityEvent OnDestroy_;

        private void Awake()
        {
            OnAwake?.Invoke();
        }

        private void OnEnable()
        {
            OnOnEnable_?.Invoke();
        }
        
        private void Start()
        {
            OnStart?.Invoke();
        }

        private void OnDisable()
        {
            OnDisable_?.Invoke();
        }

        private void OnDestroy()
        {
            OnDestroy_?.Invoke();
        }
    }
}



