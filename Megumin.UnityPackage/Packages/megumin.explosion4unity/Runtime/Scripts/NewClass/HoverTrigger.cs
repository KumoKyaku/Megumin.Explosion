using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

namespace Megumin
{
    /// <summary>
    /// 悬浮触发器,AreaMode过滤重复事件
    /// </summary>
    public class HoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// 只关心区域,不在乎子对象,避免重复触发
        /// </summary>
        public bool AreaMode = true;
        public bool ShowDebugLog = false;

        [Space]
        [ReadOnlyInInspector]
        public bool IsPointerInside;
        [ReadOnlyInInspector]
        public bool IsPositionInside;
        [ReadOnlyInInspector]
        public GameObject PointerEnterObj;

        [Space]
        public TriggerEvent PointerEnter;
        public TriggerEvent PointerExit;

        private void Start()
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData == null || eventData.pointerEnter == null || eventData.pointerEnter.GetComponentInParent<HoverTrigger>() != this)
            {
                return;
            }

            var oldState = IsPositionInside;
            IsPositionInside = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, eventData.position);
            IsPointerInside = true;
            PointerEnterObj = eventData.pointerEnter;

            DebugLog(eventData);

            var callEvent = AreaMode == false || oldState != IsPositionInside;
            callEvent &= enabled;
            if (callEvent)
            {
                if (ShowDebugLog)
                {
                    Debug.Log("callEventOnPointerEnter");
                }
                PointerEnter?.Invoke(eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData == null || eventData.pointerEnter == null || eventData.pointerEnter.GetComponentInParent<HoverTrigger>() != this)
            {
                return;
            }

            var oldState = IsPositionInside;
            IsPositionInside = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, eventData.position);
            IsPointerInside = false;
            PointerEnterObj = eventData.pointerEnter;

            DebugLog(eventData);

            var callEvent = AreaMode == false || oldState != IsPositionInside;
            callEvent &= enabled;
            if (callEvent)
            {
                if (ShowDebugLog)
                {
                    Debug.Log("callEvent_OnPointerExit");
                }
                PointerExit?.Invoke(eventData);
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void DebugLog(PointerEventData eventData, [System.Runtime.CompilerServices.CallerMemberName] string funcName = default)
        {
            if (ShowDebugLog)
            {
                Debug.Log($"{funcName} | {eventData.position} | isPointerInside:{IsPointerInside} | isPositionInside:{IsPositionInside}");
                Debug.LogError(eventData.ToStringReflection());
            }
        }
    }
}

