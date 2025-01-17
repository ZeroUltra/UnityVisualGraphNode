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
        public virtual Vector2 default_size => GraphSetting.NodeDefaultSize;
        public virtual bool ShowNodeProperties => GraphSetting.IsUseIMGUI;
        public VisualGraphNode Node { get; private set; }
        public bool IsRunning { get; set; }
        protected NodeGraphSetting GraphSetting => NodeGraphSetting.Instance;

        private VisualElement selecedElement;


        public virtual void InitNode(VisualGraphNode graphNode)
        {
            Node = graphNode;
            if (selecedElement == null && this[1] != null)
                selecedElement = this[1];

            //注册鼠标进入和离开事件
            this.RegisterCallback<MouseEnterEvent>(evt =>
            {
                if (IsRunning) return;
                SetBorderColor(GraphSetting.NodeSelectedBorderColor);
                SetBorderWidth(1);
            });

            this.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                if (IsRunning) return;
                SetBorderColor(Color.clear);
            });

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
                if (!Application.isPlaying)
                {
                    SetBorderColor(GraphSetting.NodeSelectedBorderColor);
                    SetBorderWidth(GraphSetting.NodeSelectedBorderWidth);
                }
            }
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            if (!(this is VisualGraphStartNodeView))
            {
                if (!Application.isPlaying)
                {
                    SetBorderColor(Color.clear);
                }
            }
        }

        /// <summary>
        /// 设置边框颜色
        /// </summary>
        /// <param name="color"></param>
        internal void SetBorderColor(Color color)
        {
            if (selecedElement == null) return;
            selecedElement.style.borderBottomColor = selecedElement.style.borderTopColor = selecedElement.style.borderLeftColor = selecedElement.style.borderRightColor = color;
        }
        /// <summary>
        /// 设置边框宽度
        /// </summary>
        /// <param name="width"></param>
        public void SetBorderWidth(int width)
        {
            if (selecedElement == null) return;
            selecedElement.style.borderBottomWidth = selecedElement.style.borderTopWidth = selecedElement.style.borderLeftWidth = selecedElement.style.borderRightWidth = width;
        }
        //public void Reset()
        //{
        //    this.Reset();
        //}
    }
}
