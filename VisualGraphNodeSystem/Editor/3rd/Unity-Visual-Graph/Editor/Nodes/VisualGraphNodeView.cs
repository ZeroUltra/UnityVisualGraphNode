using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VisualGraphRuntime;
using VisualGraphNodeSystem;
using UnityEngine.UIElements;
using VisualGraphNodeSystem.Editor;
using System;
using System.Reflection;
namespace VisualGraphInEditor
{
    public class VisualGraphNodeView : Node
    {
        [HideInInspector] public virtual Vector2 default_size => NodeGraphSetting.Instance.NodeDefaultSize;
        [HideInInspector] public virtual bool ShowNodeProperties => true;

        [HideInInspector] public VisualGraphRuntime.VisualGraphNode node;

        Label runingLabel;
        public virtual void DrawNode()
        {
            //添加描述
            if (this.node is NodeBase)
            {
                if (this.node is NodeEnd) return;
                TextField text = new TextField();
                text.label = "Desc:";
                text.value = node.NodeDescription;
                text.style.minHeight = 18;
                text.multiline = true;
                text.Q<Label>().style.minWidth = 35;
                text.style.whiteSpace = WhiteSpace.Normal;//自动换行
                mainContainer.Add(text);
                text.RegisterCallback<ChangeEvent<string>>(e =>
                {
                    node.NodeDescription = e.newValue;
                });

                //runingLabel = new Label("►");
                //runingLabel.style.color = Color.green;
                //runingLabel.style.width = 10;
                //runingLabel.style.height = 10;
                //runingLabel.style.marginTop = 8;
                //runingLabel.style.textField.style.marginTop = 4; = TextAnchor.MiddleLeft;  // 文本左对齐
                //runingLabel.style.justifyContent = Justify.FlexStart;         // 居中显示
            }
            // titleContainer.style.backgroundColor = Color.red;
        }

        public virtual void InitNode(VisualGraphRuntime.VisualGraphNode graphNode)
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
                    title = "►" + GetGraphNodeName(node.GetType());
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
                    title =  GetGraphNodeName(node.GetType());
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
