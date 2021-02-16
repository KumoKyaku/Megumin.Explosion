using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Link
{
    [MenuItem("Tool/Link/IconList")]
    static void IconList()
    {
        Application.OpenURL("https://github.com/halak/unity-editor-icons/blob/master/README.md");
    }
}



