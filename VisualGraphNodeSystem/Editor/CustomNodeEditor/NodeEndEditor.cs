using UnityEngine;
using VisualGraphInEditor;
namespace VisualGraphNodeSystem.Editor
{
    [CustomNodeView((typeof(NodeEnd)))]
    public sealed class NodeEndEditor : VisualGraphNodeView
    {
        public override bool ShowNodeProperties => false;
        public override Vector2 default_size => new Vector2(120, 120);

        public override void DrawNode()
        {
            //base.DrawNode();
        }
    }
}