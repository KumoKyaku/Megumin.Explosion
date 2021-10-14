using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Megumin
{
    public class UserEventTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public float delta = 0.5f;
        public UnityEvent OnDoubleClick;
        float last = -1;
        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log($"count {eventData.clickCount}");

            var now = eventData.clickTime;
            if (now - last < delta)
            {
                last = -1;
                //双击
                //Debug.Log("shuangji");
                OnDoubleClick?.Invoke();
            }
            else
            {
                last = now;
            }
        }
    }
}


