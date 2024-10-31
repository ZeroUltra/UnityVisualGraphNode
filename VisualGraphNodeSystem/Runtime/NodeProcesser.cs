using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    public class NodeProcesser
    {
        public static event System.Action<NodeBase> OnChangeNodeEvent;
        public NodeGraphBase TargetNodeGraph { get; private set; }
        public NodeProcesser(NodeGraphBase targetNodeGraph)
        {
            this.TargetNodeGraph = targetNodeGraph;
            targetNodeGraph.InitializeGraph();
        }
        public NodeBase GetFirstNode()
        {
            return TargetNodeGraph.StartNode.GetOutpotPortWithIndex(0).GetConnectNode() as NodeBase;
        }

        /// <summary>
        /// 刷新节点 用于通知编辑器中节点变化
        /// </summary>
        /// <param name="nodeBase"></param>
        public void RefreshNodeChange(NodeBase nodeBase)
        {
            OnChangeNodeEvent?.Invoke(nodeBase);
        }
    }
}
