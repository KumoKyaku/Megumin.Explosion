using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


namespace Megumin
{
    /// <summary>
    /// 提交事件触发,点击,按键等
    /// </summary>
    public class SubmitTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {

#if ENABLE_INPUT_SYSTEM
        public InputActionReference Action;
#endif
        public TriggerEvent PointerClick;

        private void OnEnable()
        {
#if ENABLE_INPUT_SYSTEM
            if (Action)
            {
                Action.action.performed += Action_performed;
            }
#endif
        }

        private void OnDisable()
        {

#if ENABLE_INPUT_SYSTEM
            if (Action)
            {
                Action.action.performed -= Action_performed;
            }
#endif
        }

#if ENABLE_INPUT_SYSTEM
        private void Action_performed(InputAction.CallbackContext obj)
        {
            PointerClick?.Invoke(null);
        }
#endif
        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClick?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        /// <summary>
        /// 等效触发,用于按键回调
        /// </summary>
        public void EquivalentTrigger()
        {
            PointerClick?.Invoke(null);
        }
    }
}

