using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VisualGraphRuntime;

namespace VisualGraphInEditor
{
    [CustomNodeView((typeof(VisualGraphStartNode)))]
    public sealed class VisualGraphStartNodeView : VisualGraphNodeView
    {
        public override bool ShowNodeProperties => false;
        public override Vector2 default_size => new Vector2(80, 150);
        public override Capabilities SetCapabilities(Capabilities capabilities)
        {
            //capabilities &= ~UnityEditor.Experimental.GraphView.Capabilities.Movable;
            capabilities &= ~UnityEditor.Experimental.GraphView.Capabilities.Deletable;
            return capabilities;
        }
        public override void DrawNode()
        {
            //base.DrawNode();
        }
    }
}
