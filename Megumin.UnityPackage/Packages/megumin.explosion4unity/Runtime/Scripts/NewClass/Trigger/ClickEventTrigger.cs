using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace Megumin
{
    /// <summary>
    /// <see cref="EventTrigger"/> 实现了太多接口,拦截了不必要事件
    /// </summary>
    public class ClickEventTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool LeftClick = true;
        public bool RightClick = true;
        public bool DoubleClick = false;
        [Indent]
        [Range(0, 1)]
        public float delta = 0.5f;

        [Space]
        public TriggerEvent PointerClick;
        public TriggerEvent OnDoubleClick;
        public TriggerEvent OnLeftClick;
        public TriggerEvent OnRightClick;
        float last = -1;

        public bool DebugLog = false;
        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (DebugLog)
            {
                Debug.Log(eventData.ToString());
            }

            PointerClick?.Invoke(eventData);
            if (LeftClick)
            {
                //右键
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    OnLeftClick?.Invoke(eventData);
                }
            }

            if (RightClick)
            {
                //右键
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    OnRightClick?.Invoke(eventData);
                }
            }

            if (DoubleClick)
            {
                //双击
                var now = eventData.clickTime;
                if (now - last < delta)
                {
                    last = -1;
                    //双击
                    //Debug.Log("shuangji");
                    OnDoubleClick?.Invoke(eventData);
                }
                else
                {
                    last = now;
                }
            }
        }

        public void TestLog(BaseEventData eventData)
        {
            Debug.Log(eventData.selectedObject);
        }
    }
}


