using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// description:      
/// </summary
namespace VisualGraphNodeSystem
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class HideInNormalInspectorAttribute : PropertyAttribute
    {
        public HideInNormalInspectorAttribute()
        {

        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HideInNormalInspectorAttribute))]
    public class HideInNormalInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }
#endif

}