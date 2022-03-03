using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Megumin;

namespace Megumin
{
    /// <summary>
    /// 在帧数后面提示时长
    /// </summary>
    public class FrameAndTimeAttribute : PropertyAttribute
    {
        public enum TimeUnit
        {
            Second,
            Millisecond,
        }

        public enum SaveData
        {
            Frame,
            Time,
        }

        public TimeUnit ShowMode { get; set; } = TimeUnit.Second;
        public SaveData Data { get; set; } = SaveData.Frame;

        public int FrameRate { get; set; } = 60;

        public FrameAndTimeAttribute(int frameRate = 60,
                                     SaveData data = SaveData.Frame,
                                     TimeUnit timeUnit = TimeUnit.Second)
        {
            FrameRate = frameRate;
            Data = data;
            ShowMode = timeUnit;
        }
    }

    /// <summary>
    /// 默认60帧,保存为int毫秒数
    /// </summary>
    public class FrameAndTime2Attribute : FrameAndTimeAttribute
    {
        public FrameAndTime2Attribute(int frameRate = 60,
                                 SaveData data = SaveData.Time,
                                 TimeUnit timeUnit = TimeUnit.Millisecond)
        {
            FrameRate = frameRate;
            Data = data;
            ShowMode = timeUnit;
        }
    }
}


#if UNITY_EDITOR

namespace UnityEditor.Megumin
{

#if !DISABLE_MEGUMIN_PROPERTYDRWAER
    [CustomPropertyDrawer(typeof(FrameAndTimeAttribute), true)]
#endif
    internal sealed class Frame2TimeDrawer : PropertyDrawer
    {
        public bool inputmode = false;
        

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer
                || property.propertyType == SerializedPropertyType.Float)
            {
                var propertyPosition = position;
                propertyPosition.width -= 100;

                var togglePosition = position;
                togglePosition.x += togglePosition.width - 14;
                togglePosition.width = 16;

                var exShow = position;
                exShow.x += position.width - 100;
                exShow.width = 80;

                //const int ShowUnitLen = 16;
                //int offset = 24;
                //var exShowUnitPos = position;
                //exShowUnitPos.x += position.width - (offset + ShowUnitLen);
                //exShowUnitPos.width = ShowUnitLen;


                inputmode = GUI.Toggle(togglePosition, inputmode, GUIContent.none);

                FrameAndTimeAttribute attri = (FrameAndTimeAttribute)attribute;

                var isInputFrameMode = true;
                if (attri.Data == FrameAndTimeAttribute.SaveData.Frame)
                {
                    isInputFrameMode = !inputmode;
                }
                if (attri.Data == FrameAndTimeAttribute.SaveData.Time)
                {
                    isInputFrameMode = inputmode;
                }

                var rate = attri.FrameRate;

                switch (attri.Data)
                {
                    case FrameAndTimeAttribute.SaveData.Frame:
                        {
                            var frame = property.intValue;
                            var time = ((float)frame / rate);
                            var mstime = (int)((float)frame * 1000 / rate + 0.5f);

                            if (isInputFrameMode)
                            {
                                //帧数输入模式
                                EditorGUI.PropertyField(propertyPosition, property, label);
                                using (new EditorGUI.DisabledGroupScope(true))
                                {
                                    switch (attri.ShowMode)
                                    {
                                        case FrameAndTimeAttribute.TimeUnit.Second:
                                            EditorGUI.FloatField(exShow, time);
                                            break;
                                        case FrameAndTimeAttribute.TimeUnit.Millisecond:
                                            EditorGUI.IntField(exShow, mstime);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                //时间输入模式

                                using (new EditorGUI.DisabledGroupScope(true))
                                {
                                    EditorGUI.PropertyField(propertyPosition, property, label);
                                }

                                switch (attri.ShowMode)
                                {
                                    case FrameAndTimeAttribute.TimeUnit.Second:
                                        {
                                            time = EditorGUI.FloatField(exShow, time);
                                            var f = (int)(time * rate + 0.5f);
                                            property.intValue = f;
                                        }
                                        break;
                                    case FrameAndTimeAttribute.TimeUnit.Millisecond:
                                        {
                                            mstime = EditorGUI.IntField(exShow, mstime);
                                            var f = (int)(mstime * rate / 1000 + 0.5f);
                                            property.intValue = f;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        break;
                    case FrameAndTimeAttribute.SaveData.Time:
                        {
                            if (isInputFrameMode)
                            {
                                switch (attri.ShowMode)
                                {
                                    case FrameAndTimeAttribute.TimeUnit.Second:
                                        {
                                            var mstime = property.floatValue;
                                            var frame = (int)(mstime * rate  + 0.5f);
                                            //帧数输入模式
                                            using (new EditorGUI.DisabledGroupScope(true))
                                            {
                                                EditorGUI.PropertyField(propertyPosition, property, label);
                                            }

                                            frame = EditorGUI.IntField(exShow, frame);
                                            mstime = (float)frame / rate;
                                            property.floatValue = mstime;
                                        }
                                        break;
                                    case FrameAndTimeAttribute.TimeUnit.Millisecond:
                                        {
                                            var mstime = property.intValue;
                                            var frame = (int)(mstime * rate / 1000 + 0.5f);
                                            //帧数输入模式
                                            using (new EditorGUI.DisabledGroupScope(true))
                                            {
                                                EditorGUI.PropertyField(propertyPosition, property, label);
                                            }

                                            frame = EditorGUI.IntField(exShow, frame);
                                            mstime = (int)((float)frame * 1000 / rate + 0.5f);
                                            property.intValue = mstime;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                //时间输入模式
                                switch (attri.ShowMode)
                                {
                                    case FrameAndTimeAttribute.TimeUnit.Second:
                                        {
                                            EditorGUI.PropertyField(propertyPosition, property, label);
                                            using (new EditorGUI.DisabledGroupScope(true))
                                            {
                                                var mstime = property.floatValue;
                                                var frame = (int)(mstime * rate + 0.5f);
                                                EditorGUI.IntField(exShow, frame);
                                            }
                                        }
                                        break;
                                    case FrameAndTimeAttribute.TimeUnit.Millisecond:
                                        {
                                            EditorGUI.PropertyField(propertyPosition, property, label);
                                            using (new EditorGUI.DisabledGroupScope(true))
                                            {
                                                var mstime = property.intValue;
                                                var frame = (int)(mstime * rate / 1000 + 0.5f);
                                                EditorGUI.IntField(exShow, frame);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }

                //GUI.enabled = true;
                //EditorGUI.LabelField(exShowUnitPos, "t");
                //GUI.enabled = true;
            }
            else
            {
                this.NotMatch(position, property, label, "字段类型必须是Int");
            }
        }
    }
}

#endif








