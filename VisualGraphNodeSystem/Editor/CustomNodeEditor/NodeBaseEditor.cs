
using UnityEditor;
namespace VisualGraphNodeSystem.Editor
{
    [CustomEditor(typeof(VisualNodeBase), editorForChildClasses: true)]
    public class NodeBaseEditor : UnityEditor.Editor
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