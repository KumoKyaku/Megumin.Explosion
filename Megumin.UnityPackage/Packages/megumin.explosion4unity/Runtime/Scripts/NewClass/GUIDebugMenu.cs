using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Megumin
{
    [ExecuteAlways]
    public class GUIDebugMenu : MonoBehaviour
    {
        public Rect ButtonPos = new Rect(0.9f, 0.1f, 100, 30);
        public int Space = 8;
        [SerializeField]
        private Binds[] Buttons = new Binds[1];

        void OnGUI()
        {
            var pos = ButtonPos;
            pos.x = Mathf.Min(Mathf.Abs(pos.x), 1);
            pos.y = Mathf.Min(Mathf.Abs(pos.y), 1);

            pos.x *= (Screen.width - ButtonPos.width);
            pos.y *= (Screen.height - ButtonPos.height);

            if (Buttons != null)
            {
                for (int i = 0; i < Buttons.Length; i++)
                {
                    var rect = pos;
                    rect.y += (Space + pos.height) * i;

                    if (GUI.Button(rect, Buttons[i].Name))
                    {
                        Buttons[i]?.Bind?.Invoke();
                    }
                }
            }
        }

        [System.Serializable]
        class Binds
        {
            public string Name = "DebugButton";
            public Button.ButtonClickedEvent Bind;
        }

        public void TestLog()
        {
            Debug.Log("TestLog");
        }
    }
}






