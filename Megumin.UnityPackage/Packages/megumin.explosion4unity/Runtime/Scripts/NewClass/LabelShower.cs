using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    [ExecuteAlways]
    public class LabelShower : MonoBehaviour
    {
        public bool ShowOnRuntime = true;

        [field: SerializeField]
        public string Prefix { get; set; } = "Åö×²ºÐ";

        [field: SerializeField]
        public string OverrideLabel { get; set; } = null;

        void Start()
        {
            
        }

        void OnGUI()
        {
            if (ShowOnRuntime)
            {
                //todo
                if (Camera.current)
                {
                    var pos = Camera.current.WorldToScreenPoint(transform.position);
                    var rect = new Rect(pos.x, pos.y, 500, 40);
                    string text = CalText();
                    GUI.Label(rect, text);
                }
            }
        }

        private string CalText()
        {
            string text = OverrideLabel;
            if (string.IsNullOrEmpty(text))
            {
                text = $"{Prefix}£º[{name}|{tag}]";
            }

            this.Macro(ref text);
            return text;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (enabled)
            {
                string text = CalText();

                UnityEditor.Handles.Label(transform.position,
                    text);
            }
        }
#endif

    }
}





