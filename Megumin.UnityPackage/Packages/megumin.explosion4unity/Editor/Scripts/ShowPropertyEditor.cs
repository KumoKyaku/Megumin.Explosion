using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 用于面板显示属性
/// </summary>
/// <typeparam name="T">Type that this editor can edit.</typeparam>
public abstract class ShowPropertyEditor<T>:Editor
    where T:MonoBehaviour
{
    protected static bool IsGetProp = false;

    
    private static List<PropertyInfo> propertyCollection = new List<PropertyInfo>();

    bool isShowProperties = true;

    /// <summary>
    /// 需要显示在面板上的属性
    /// </summary>
    protected static List<PropertyInfo> PropertyCollection
    {
        get
        {
            return propertyCollection;
        }
    }

    /// <summary>
    /// 是否显示属性/ShowProperties?
    /// </summary>
    protected bool IsShowProperties { get { return isShowProperties; }set { isShowProperties = value; } } 

    public override void OnInspectorGUI()
    {
        lock (PropertyCollection)
        {
            ///初始化属性列表
            if (!IsGetProp)
            {
                var collection = typeof(T).GetProperties();

                foreach (var item in collection)
                {
                    if (!MonoBehaviourExtension_DA182CC20A33453FA684CD22CE5B97DC.
                        InnerPropertiesNames.Contains(item.Name))
                    {
                        PropertyCollection.Add(item);
                    }
                }

                IsGetProp = true;
            }
        }

        IsShowProperties = EditorGUILayout.Toggle("显示属性/ShowProperties?", IsShowProperties);

        if (IsShowProperties)
        {
            ///显示属性
            foreach (var item in PropertyCollection)
            {
                if (item.CanRead)
                {
                    var res = item.GetValue(target, null);
                    if (res == null)
                    {
                        EditorGUILayout.LabelField("{" + item.Name + "}: {null}");
                    }
                    else
                    {
                        EditorGUILayout.LabelField("{" + item.Name + "}: {" + res.ToString() + "}");
                    }
                }
            }
        }

        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}