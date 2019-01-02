using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Megumin;
using System.Reflection;
using System.Linq;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TextBinding))]
public class TextBindingEditor: Editor
{
    private readonly DropDownControl<Component> m_ComparerDropDown = new DropDownControl<Component>();
    private readonly DropDownControl<string> propInfo = new DropDownControl<string>();

    private GameObject go;
    private string[] options;

    public TextBindingEditor()
    {
        m_ComparerDropDown.convertForButtonLabel = (type)=> 
        {
            if (type == null)
            {
                return "null";
            }
            else
            {
                return type.ToString();
            }
        };
        m_ComparerDropDown.convertForGUIContent = (type) =>
        {
            if (type == null)
            {
                return "null";
            }
            else
            {
                return type.ToString();
            }
        };
        m_ComparerDropDown.ignoreConvertForGUIContent = types => false;
        m_ComparerDropDown.tooltip = "Comparer that will be used to compare values and determine the result of assertion.";
        propInfo.convertForButtonLabel = (info) =>
        {
            if (info == null)
            {
                return "null";
            }
            else
            {
                return info;
            }
        };

        propInfo.convertForGUIContent = (info) =>
        {
            if (info == null)
            {
                return "null";
            }
            else
            {
                return info;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        var script = (TextBinding)target;

        if (go == null && script.BindingScript)
        {
            go = script.BindingScript.gameObject;
        }

        go = (GameObject)EditorGUILayout.ObjectField("Target", go, typeof(GameObject), true);
        if (go == null)
        {
            script.BindingScript = null;
        }

        if (go)
        {
            if (script.BindingScript == null || script.BindingScript.gameObject != go)
            {
                script.BindingScript = go.GetComponents<Component>()[0];
            }

            EditorGUILayout.BeginHorizontal();
            m_ComparerDropDown.Draw(script.BindingScript, go.GetComponents<Component>(),
                                        type =>
                                        {
                                            if (script.BindingScript != type)
                                            {
                                                script.BindingScript = type;
                                                GeneralOptions(script);

                                                if (options != null && options.Length > 0)
                                                {
                                                    if (!options.Contains(script.FeildName))
                                                    {
                                                        script.FeildName = options[0];
                                                    }
                                                }
                                                else
                                                {
                                                    script.FeildName = "";
                                                }
                                            }
                                        });
            if (options == null || options.Length == 0)
            {
                GeneralOptions(script);
            }

            propInfo.Draw(script.FeildName, options,
                info => script.FeildName = info);
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorUtility.SetDirty(script);
        }
        
    }

    private void GeneralOptions(TextBinding script)
    {
        options = (from item in script.BindingScript.GetType().GetProperties()
                   select item.Name).ToArray();
    }
}
