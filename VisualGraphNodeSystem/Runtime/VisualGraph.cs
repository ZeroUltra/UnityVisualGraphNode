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
        [Tooltip("是否进入编辑模式")]
        [SerializeField][HideInInspector] bool editMode = false;
        [TextArea(1, 5)]
        [SerializeField][HideInInspector] string desc;
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(VisualGraph))]
    public class VisualGraphEditor : Editor
    {
        private SerializedProperty serDesc;
        private SerializedProperty serEditMode;
        private SerializedProperty serNodes;
        private void OnEnable()
        {
            serEditMode = serializedObject.FindProperty("editMode");
            serDesc = serializedObject.FindProperty("desc");
            serNodes = serializedObject.FindProperty("Nodes");
        }
        public override void OnInspectorGUI()
        {

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", obj: MonoScript.FromScriptableObject(target as ScriptableObject), typeof(ScriptableObject), allowSceneObjects: true);
            EditorGUI.EndDisabledGroup();

            serializedObject.Update();

            EditorGUILayout.PropertyField(serEditMode);
            EditorGUI.BeginDisabledGroup(!serEditMode.boolValue);
            EditorGUILayout.PropertyField(serDesc);
            EditorGUILayout.PropertyField(serNodes);
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
