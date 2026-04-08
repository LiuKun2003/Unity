using System;
using LK.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace LK.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDateTime))]
    public class DateTimeDrawer : PropertyDrawer
    {
        private const float Spacing = 4f;
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
        
            // 获取ticks属性并转换为DateTime
            var ticksProp = property.FindPropertyRelative("ticks");
            var currentDate = new DateTime(ticksProp.longValue);
            // if (currentDate.Ticks == 0) currentDate = DateTime.Now; // 默认值处理
        
            // 绘制主标签
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
            // 保存并重置缩进
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
        
            // 计算布局：总宽度减去间距后平分给日期和时间
            var halfWidth = (position.width - Spacing) / 2f;
        
            var dateRect = new Rect(position.x, position.y, halfWidth, position.height);
            var timeRect = new Rect(position.x + halfWidth + Spacing, position.y, halfWidth, position.height);
        
            // 临时修改标签宽度
            var originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 32f;
        
            // 日期输入 (yyyy-MM-dd)
            EditorGUI.BeginChangeCheck();
            var dateStr = EditorGUI.TextField(dateRect, "Date", currentDate.ToString("yyyy-MM-dd"));
            if (EditorGUI.EndChangeCheck())
            {
                if (DateTime.TryParse(dateStr, out var parsedDate))
                {
                    // 保持原有时间部分，只更新日期
                    var newDate = new DateTime(
                        parsedDate.Year, parsedDate.Month, parsedDate.Day,
                        currentDate.Hour, currentDate.Minute, currentDate.Second
                    );
                    ticksProp.longValue = newDate.Ticks;
                }
            }
        
            // 时间输入 (HH:mm:ss)
            EditorGUI.BeginChangeCheck();
            var timeStr = EditorGUI.TextField(timeRect, "Time", currentDate.ToString("HH:mm:ss"));
            if (EditorGUI.EndChangeCheck())
            {
                if (DateTime.TryParseExact(timeStr, "HH:mm:ss", null, 
                        System.Globalization.DateTimeStyles.None, out var parsedTime) ||
                    DateTime.TryParse(timeStr, out parsedTime))
                {
                    // 保持原有日期部分，只更新时间
                    var newDate = new DateTime(
                        currentDate.Year, currentDate.Month, currentDate.Day,
                        parsedTime.Hour, parsedTime.Minute, parsedTime.Second
                    );
                    ticksProp.longValue = newDate.Ticks;
                }
            }
        

            EditorGUIUtility.labelWidth = originalLabelWidth;
            EditorGUI.indentLevel = indent;
        
            EditorGUI.EndProperty();
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}