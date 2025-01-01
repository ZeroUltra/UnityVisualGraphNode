using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    public class NodeProcesser
    {
        public static event System.Action<VisualNodeBase> OnChangeNodeEvent;
        public NodeGraphBase TargetNodeGraph { get; private set; }
        public NodeProcesser(NodeGraphBase targetNodeGraph)
        {
            this.TargetNodeGraph = targetNodeGraph;
            targetNodeGraph.InitializeGraph();
        }
        public VisualNodeBase GetFirstNode()
        {
            return TargetNodeGraph.StartNode.GetOutpotPortWithIndex(0).GetConnectNode() as VisualNodeBase;
        }

        /// <summary>
        /// 刷新节点 用于通知编辑器中节点变化
        /// </summary>
        /// <param name="nodeBase"></param>
        public void RefreshNodeChange(VisualNodeBase nodeBase)
        {
            OnChangeNodeEvent?.Invoke(nodeBase);
        }
    }
}
