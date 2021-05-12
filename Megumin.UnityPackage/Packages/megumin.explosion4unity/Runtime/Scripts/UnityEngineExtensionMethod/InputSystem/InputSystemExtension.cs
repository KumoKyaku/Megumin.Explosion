#if ENABLE_INPUT_SYSTEM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.InputSystem
{
    public static class InputSystemExtension_F4571CE6
    {
        public static string ToLogStr(this in InputAction.CallbackContext context)
        {
            return $"frameID:{Time.frameCount}, Context:{{started:{context.started}, performed:{context.performed}, canceled:{context.canceled}, Value:{context.ReadValueAsObject()}, action:{context.action}}}";
        }

        public static void Log(this in InputAction.CallbackContext context)
        {
            Debug.Log(context.ToLogStr());
        }

        public static void LogError(this in InputAction.CallbackContext context)
        {
            Debug.LogError(context.ToLogStr());
        }

        public static bool IsMouse(this in InputAction.CallbackContext context)
        {
            return context.control.device.name == "Mouse";
        }

        public static bool IsKeyboard(this in InputAction.CallbackContext context)
        {
            return context.control.device.name == "Keyboard";
        }
    }
}


#endif





