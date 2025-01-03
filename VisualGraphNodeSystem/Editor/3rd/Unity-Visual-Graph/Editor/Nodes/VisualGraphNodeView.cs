using System;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using VisualGraphNodeSystem;
using VisualGraphNodeSystem.Editor;
using VisualGraphRuntime;
namespace VisualGraphInEditor
{
    public class VisualGraphNodeView : Node
    {
        [HideInInspector] public virtual Vector2 default_size => GraphSetting.NodeDefaultSize;
        [HideInInspector] public virtual bool ShowNodeProperties => GraphSetting.IsUseIMGUI;

        [HideInInspector] public VisualGraphNode Node { get; private set; }

        protected NodeGraphSetting GraphSetting => NodeGraphSetting.Instance;

        public virtual void InitNode(VisualGraphNode graphNode)
        {
            Node = graphNode;
        }
        public virtual void DrawNode()
        {
            if (GraphSetting.IsShowDesc)
            {
                //添加描述
                if (this.Node is VisualNodeBase visualNodeBase)
                {
                    if (this.Node is NodeEnd) return;
                    TextField text = new TextField();
                    text.label = "Desc:";
                    text.value = Node.NodeDescription;
                    text.style.minHeight = 18;
                    text.multiline = true;
                    text.Q<Label>().style.minWidth = 50;
                    text.style.whiteSpace = WhiteSpace.Normal;//自动换行
                    mainContainer.Add(text);
                    text.RegisterCallback<ChangeEvent<string>>(e =>
                    {
                        Node.NodeDescription = e.newValue;
                    });
                }
            }
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
                    title = "►" + GetGraphNodeName(Node.GetType());
                }
            }

        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            if (!(this is VisualGraphStartNodeView))
            {
                if (Application.isPlaying)
                {
                    title = GetGraphNodeName(Node.GetType());
                }
            }
        }
        private string GetGraphNodeName(Type type)
        {
            string display_name = "";
            if (type.GetCustomAttribute<NodeNameAttribute>() != null)
            {
                display_name = type.GetCustomAttribute<NodeNameAttribute>().name;
            }
            else
            {
                display_name = type.Name;
            }
            return display_name;
        }
    }
}
