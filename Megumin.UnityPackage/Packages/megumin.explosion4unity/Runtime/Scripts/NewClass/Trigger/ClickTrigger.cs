using System.Collections;
#if MEGUMIN_UGUI
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

namespace Megumin 
{
    public class ClickTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        public TriggerEvent PointerClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClick?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }
    }

}
#endif

