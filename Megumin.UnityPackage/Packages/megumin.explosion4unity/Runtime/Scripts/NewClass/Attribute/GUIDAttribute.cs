using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

namespace Megumin
{
    /// <summary>
    /// 将string字段添加GUID功能.
    /// </summary>
    /// <remarks>使用string GUID做key时,Ctrl + D 复制会出现相同ID,不好用</remarks>
    public class GUIDAttribute : PropertyAttribute
    {

    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(GUIDAttribute))]
#endif
    internal sealed class GUIDDrawer : PropertyDrawer
    {
        static GUIStyle left = new GUIStyle("minibuttonleft");
        static GUIStyle right = new GUIStyle("minibuttonright");
        string needNewPath = null;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                if (needNewPath == property.propertyPath && !string.IsNullOrEmpty(needNewPath))
                {
                    needNewPath = null;
                    property.stringValue = GUID.Generate().ToString();
                }

                var propertyPosition = position;
                propertyPosition.width -= 86;

                var buttonPosition = position;
                buttonPosition.width = 80;
                buttonPosition.x += position.width - 80;

                var leftPosotion = buttonPosition;
                leftPosotion.width = 40;
                var rightPosition = buttonPosition;
                rightPosition.width = 40;
                rightPosition.x += 40;

                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.PropertyField(propertyPosition, property, label);


                    string current = property.stringValue;

                    if (string.IsNullOrEmpty(current))
                    {
                        current = GUID.Generate().ToString();
                        property.stringValue = current;
                    }
                }

                if (GUI.Button(leftPosotion, "GUID", left))
                {
                    if (EditorUtility.DisplayDialog("GUID", "确定生成新的GUID吗", "OK", "Cancel"))
                    {
                        needNewPath = property.propertyPath;
                        //property.stringValue = GUID.Generate().ToString();不生效,改用needNewPath.
                    }
                    GUIUtility.ExitGUI();
                }

                if (GUI.Button(rightPosition, "Copy", right))
                {
                    GUIUtility.systemCopyBuffer = property.stringValue;
                }
            }
            else
            {
                label.tooltip += $"{nameof(GUIDAttribute)}失效！\n{label.text} 字段类型必须是string";
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}

#endif









