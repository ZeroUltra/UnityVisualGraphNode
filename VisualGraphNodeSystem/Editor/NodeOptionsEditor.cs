using UnityEngine;
using VisualGraphInEditor;
using VisualGraphNodeSystem;

[CustomNodeView((typeof(NodeOption)))]
public class NodeOptionsEditor : VisualGraphNodeView
{
    public override Vector2 default_size => new Vector2(300, 200);

}