using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

namespace Megumin
{
    public class HoverTrigger : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        public TriggerEvent PointerEnter;
        public TriggerEvent PointerExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit?.Invoke(eventData);
        }
    }
}

