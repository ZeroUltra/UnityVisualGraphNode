using UnityEngine;
using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    [CreateAssetMenu(fileName = "NewNodeGraph", menuName = "Create Node Graph/New Node Graph", order = -11)]

    [DefaultNodeType(typeof(VisualNodeBase))]
    public class VisualGraph : VisualGraphBase
    {
#if UNITY_EDITOR
        [Multiline(3)]
        [SerializeField] string descaription;
#endif
    }
}
