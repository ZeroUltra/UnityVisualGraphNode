using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace NodeGraphView
{
    [CustomEditor(typeof(NodeBase), editorForChildClasses: true)]
    public class NodeBaseEditor : Editor
    {
        NodeGraphSetting setting;
        private void OnEnable()
        {
            setting = NodeGraphSetting.Instance;
        }
        public override void OnInspectorGUI()
        {
            //设置标签宽度
            EditorGUIUtility.labelWidth = setting.LabelWidth;
            base.OnInspectorGUI();
        }
    }
}