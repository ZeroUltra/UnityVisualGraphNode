using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VisualGraphRuntime;
using NodeGraphView;
using UnityEngine.UIElements;
namespace VisualGraphInEditor
{
    public class VisualGraphNodeView : Node
    {
        [HideInInspector] public virtual Vector2 default_size => NodeGraphSetting.Instance.NodeDefaultSize;
        [HideInInspector] public virtual bool ShowNodeProperties => true;

        [HideInInspector] public VisualGraphNode node;

        public virtual void DrawNode()
        {

        }

        public virtual void InitNode(VisualGraphNode graphNode)
        {
            node = graphNode;
        }

        public virtual Capabilities SetCapabilities(Capabilities capabilities)
        {
            return capabilities;
        }
        public override void OnSelected()
        {
            base.OnSelected();
            if (!(this is VisualGraphStartNodeView))
            {
                if (Application.isPlaying)
                {
                    titleContainer.style.backgroundColor = new UnityEngine.UIElements.StyleColor(new Color32(0, 120, 0, 255));
                }
                else
                {
                    titleContainer.style.backgroundColor = Color.black;
                }
            }
        }
    }
}
