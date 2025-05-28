///-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------
using UnityEditor;
using UnityEditor.Callbacks;
using VisualGraphRuntime;

namespace VisualGraphInEditor
{
    [CustomEditor(typeof(VisualGraphBase), true)]
    public class VisualGraphInspector : Editor
    {
        [OnOpenAssetAttribute(1)]
        public static bool OpenVisualGraph(int instanceID, int line)
        {
            VisualGraphBase graph = EditorUtility.InstanceIDToObject(instanceID) as VisualGraphBase;
            if (graph != null)
            {
                VisualGraphEditor.CreateGraphViewWindow(graph, true);
                return true;
            }
            return false;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.EndDisabledGroup();
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}