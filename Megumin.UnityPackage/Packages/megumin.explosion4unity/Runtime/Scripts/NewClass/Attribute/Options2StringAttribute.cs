using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Megumin;

namespace Megumin
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Options2StringAttribute : PropertyAttribute
    {
        public Type Type { get; set; }
        public string OverrideName { get; set; }

        public bool Sort { get; set; } = true;

        public string DefaultValue { get; set; }

        static readonly Dictionary<Type, (string[] Show, string[] Value)> Cache
           = new Dictionary<Type, (string[] Show, string[] Value)>();
        public Options2StringAttribute(Type type, string defaultValue = null, bool sort = true, string overrideName = "")
        {
            Type = type;
            Sort = sort;
            OverrideName = overrideName;
            DefaultValue = defaultValue;

            var ops = InitOptions(type);
            Options = ops;
        }

        private (string[] Show, string[] Value) InitOptions(Type type)
        {
            if (!Cache.TryGetValue(type, out var options))
            {
                var selected = (from field in type.GetFields()
                                where field.FieldType == typeof(string)
                                && field.IsPublic && field.IsStatic
                                select field);

                if (Sort)
                {
                    selected = from f in selected
                               orderby f.Name
                               select f;
                }

                var fields = selected.ToArray();

                var optionShow = new string[fields.Length];
                var optionValue = new string[fields.Length];
                options = (optionShow, optionValue);
                Cache[type] = options;

                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];
                    var value = field.GetValue(null) as string;
                    //var show = value; 使用字段名字而不是值，因为写代码比较时用的也是字段名
                    var show = field.Name;
                    foreach (var attri in field.GetCustomAttributes(typeof(AliasAttribute), true))
                    {
                        if (attri is AliasAttribute alias)
                        {
                            show += $" [{alias.Alias}]";
                        }
                    }

                    optionShow[i] = show;
                    optionValue[i] = value;
                }
            }
            return options;
        }

        public (string[] Show, string[] Value) Options;

        public (string[] Show, string[] Value)? Options2 = null;
        public List<(Type Type, string[] Show, string[] Value)> Options2WhitType;

        public Type[] Types;

        public Options2StringAttribute(params Type[] types)
        {
            Sort = true;
            OverrideName = "";
            DefaultValue = null;

            Types = types;
            if (Types != null && Types.Length > 0)
            {
                Type = Types[0];
                var ops = InitOptions(Type);
                Options = ops;

                Options2WhitType = new List<(Type Type, string[] Show, string[] Value)>();
                foreach (var item in Types)
                {
                    var ops2 = InitOptions(item);
                    Options2WhitType.Add((item, ops2.Show, ops2.Value));
                }

                IEnumerable<string> show2 = new string[0];
                IEnumerable<string> value2 = new string[0];

                foreach (var item in Options2WhitType)
                {
                    show2 = show2.Concat(item.Show);
                    value2 = value2.Concat(item.Value);
                }

                Options2 = (show2.ToArray(), value2.ToArray());
            }
        }
    }
}

#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(Options2StringAttribute))]
#endif
    internal sealed class Options2StringDrawer : PropertyDrawer
    {
        public bool UseEnum = true;
        static readonly Color warning = new Color(1, 0.7568f, 0.0275f, 1);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var togglePosition = position;
                togglePosition.x += togglePosition.width - 14;
                togglePosition.width = 16;

                var valuePosition = position;
                valuePosition.width -= 20;
                //valuePosition.x += 20;

                ///在后面画个小勾，切换字符串还是Enum
                UseEnum = GUI.Toggle(togglePosition, UseEnum, GUIContent.none);

                if (UseEnum)
                {
                    DrawEnum(property, label, valuePosition);
                }
                else
                {
                    EditorGUI.PropertyField(valuePosition, property, label);
                }
            }
            else if (property.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            else
            {
                NotWork(position, property, label);
            }
        }

        private void NotWork(Rect position, SerializedProperty property, GUIContent label)
        {
            //EditorGUI.HelpBox(position, $"{label.text} 字段类型必须是string", MessageType.Error);
            label.tooltip += $"{attribute.GetType().Name}失效！\n{label.text} 字段类型必须是String";
            label.text = $"??? " + label.text;
            var old = GUI.color;
            GUI.color = warning;
            EditorGUI.PropertyField(position, property, label);
            GUI.color = old;
        }

        public void DrawEnum(SerializedProperty property, GUIContent label, Rect valuePosition)
        {
            Options2StringAttribute enum2StringAttribute = (Options2StringAttribute)attribute;
            var overrideName = property.displayName;
            if (!string.IsNullOrEmpty(enum2StringAttribute.OverrideName))
            {
                overrideName = enum2StringAttribute.OverrideName;
            }

            var myOptions = enum2StringAttribute.Options;
            if (enum2StringAttribute.Options2.HasValue)
            {
                myOptions = enum2StringAttribute.Options2.Value;
            }

            if (myOptions.Show?.Length > 0)
            {
                if (myOptions.Show.Length > 30)
                {
                    //TODO GenericMenu
                    //var menu = new GenericMenu();
                    //string m_SelectedValue = null;
                    //foreach (var s in enum2StringAttribute.Options2WhitType)
                    //{
                    //    var localS = s;
                    //    menu.AddItem(new GUIContent((ignoreConvertForGUIContent(options) ? localS.ToString() : convertForGUIContent(localS))),
                    //                 false,
                    //                 () => { m_SelectedValue = localS; }
                    //                 );
                    //}
                    //menu.ShowAsContext();
                }

                string current = property.stringValue;

                var index = Array.IndexOf(myOptions.Value, current);

                if (string.IsNullOrEmpty(current))
                {
                    if (enum2StringAttribute.DefaultValue == null)
                    {
                        //新添加给个初值
                        index = 0;
                        property.stringValue = myOptions.Value[index];
                    }
                    else
                    {
                        current = enum2StringAttribute.DefaultValue;
                        index = Array.IndexOf(myOptions.Value, current);
                        property.stringValue = current;
                    }
                }

                if (index != -1)
                {
                    if (overrideName.StartsWith("Element"))
                    {
                        //容器内不显名字。
                        index = EditorGUI.Popup(valuePosition, index, myOptions.Show);
                    }
                    else
                    {
                        index = EditorGUI.Popup(valuePosition, overrideName, index, myOptions.Show);
                    }
                    property.stringValue = myOptions.Value[index];
                }
                else
                {
                    label.tooltip += $"{attribute.GetType().Name}失效！\n当前值: {current} 无法解析为{enum2StringAttribute.Type.Name}的常量。";
                    label.text = $"!! " + overrideName;
                    EditorGUI.PropertyField(valuePosition, property, label);
                }
            }
            else
            {
                label.tooltip += $"{attribute.GetType().Name}失效！\n 没有常量string！";
                label.text = $"!! " + overrideName;
                EditorGUI.PropertyField(valuePosition, property, label);
            }
        }
    }
}

#endif








