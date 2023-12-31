using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

namespace Megumin
{
    /// <summary>
    /// 自动填入MetaGUID.
    /// </summary>
    public class MetaGUIDAttribute : PropertyAttribute
    {

    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(MetaGUIDAttribute))]
#endif
    internal sealed class MetaGUIDAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var propertyPosition = position;
                propertyPosition.width -= 84;

                var buttonPosition = position;
                buttonPosition.width = 80;
                buttonPosition.x += position.width - 80;

                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUI.PropertyField(propertyPosition, property, label);
                }

                var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
                if (!string.IsNullOrEmpty(path))
                {
                    var guid = AssetDatabase.GUIDFromAssetPath(path);
                    property.stringValue = guid.ToString();
                }

                if (GUI.Button(buttonPosition, "Copy"))
                {
                    Debug.Log($"MetaGUID: [{property.stringValue.Html(HexColor.EgyptianBlue)}]  is copyed.");
                    GUIUtility.systemCopyBuffer = property.stringValue;
                }
            }
            else
            {
                this.NotMatch(position, property, label);
            }
        }
    }
}

#endif



