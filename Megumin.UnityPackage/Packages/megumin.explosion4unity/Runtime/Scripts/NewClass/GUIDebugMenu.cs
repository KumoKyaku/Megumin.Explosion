using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteAlways]
public class GUIDebugMenu : MonoBehaviour
{
    public Rect ButtonPos = new Rect(1650, 50, 160, 45);
    public int Space = 10;
    public Button.ButtonClickedEvent[] Bindings = new Button.ButtonClickedEvent[1];

    void OnGUI()
    {
        if (Bindings != null)
        {
            for (int i = 0; i < Bindings.Length; i++)
            {
                var rect = ButtonPos;
                rect.y += (Space + ButtonPos.height) * i;

                if (GUI.Button(rect, $"Debug {i}"))
                {
                    Bindings[i]?.Invoke();
                }
            }
        }
    }

    public void TestLog()
    {
        Debug.Log("TestLog");
    }
}




