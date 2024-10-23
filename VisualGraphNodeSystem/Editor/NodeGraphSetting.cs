
using UnityEngine;

namespace VisualGraphNodeSystem.Editor
{
    [CreateAssetMenu(fileName = "NodeGraphSetting", menuName = "Create  Node Graph/Node Graph Setting", order = -10)]
    public class NodeGraphSetting : ScriptableObject
    {
        public static NodeGraphSetting Instance
        {
            get
            {
                return NodeHelper.FindScriptableObject<NodeGraphSetting>();
            }
        }

        [Header("标签宽度")]
        public int LabelWidth = 120;
        [Header("默认node大小")]
        public Vector2 NodeDefaultSize = new Vector2(250, 120);
    }
}