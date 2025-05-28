using UnityEngine;
using VisualGraphRuntime;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace VisualGraphNodeSystem
{
    [CreateAssetMenu(fileName = "NewNodeGraph", menuName = "Create Node Graph/New Node Graph", order = -11)]

    [DefaultNodeType(typeof(VisualNodeBase))]
    public class VisualGraph : VisualGraphBase
    {
#if UNITY_EDITOR
        [Multiline(3)]
        [SerializeField][HideInInspector] string desc;
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(VisualGraph))]
    public class VisualGraphEditor : Editor
    {
        private SerializedProperty serDesc;
        private void OnEnable()
        {
            serDesc = serializedObject.FindProperty("desc");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(serDesc);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
