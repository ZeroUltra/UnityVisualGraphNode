using UnityEngine;
using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    [CreateAssetMenu(fileName = "NewNodeGraph", menuName = "Create  Node Graph/New Node Graph", order = -11)]

    [DefaultNodeType(typeof(VisualNodeBase))]
    public class VisualGraphBase : VisualGraph
    {
#if UNITY_EDITOR
        [TextArea(3, 10)]
        public string desc; 
#endif
    }
}
