using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Megumin
{
    public class Copy2Clipboard : MonoBehaviour
    {
        public TextMeshProUGUI Value;
        public bool ShowCopyed = true;
        public int ShowTime = 1000;
        public void Copy()
        {
            if (!Value)
            {
                return;
            }

            if (ShowCopyed)
            {
                ShowCopyedText();
            }

            GUIUtility.systemCopyBuffer = Value.text;
        }

        private async void ShowCopyedText()
        {
            const string COPYED = "Copy to clipboard !!!";

            if (Value.text == COPYED)
            {
                return;
            }

            var old = Value.text;
            Value.text = COPYED;

            await Task.Delay(ShowTime);
            Value.text = old;
        }

        private void Reset()
        {
            if (!Value)
            {
                Value = GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }
}


