using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.InputSystem;

namespace Megumin
{
    public class SubmitTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        public InputActionReference Action;
        public TriggerEvent PointerClick;

        private void OnEnable()
        {
            if (Action)
            {
                Action.action.performed += Action_performed;
            }

        }

        private void OnDisable()
        {
            if (Action)
            {
                Action.action.performed -= Action_performed;
            }
        }

        private void Action_performed(InputAction.CallbackContext obj)
        {
            PointerClick?.Invoke(null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClick?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }
    }
}

