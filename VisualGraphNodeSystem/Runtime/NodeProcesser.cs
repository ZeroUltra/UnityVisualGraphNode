using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    public class NodeProcesser : MonoBehaviour
    {
        public static event System.Action< NodeBase> OnChangeNodeEvent;
        public NodeGraphBase targetNodeGraph;
        public NodeBase InitProcesser()
        {
           
            targetNodeGraph.InitializeGraph();
            return targetNodeGraph.StartNode.GetOutpotPortWithIndex(0).GetConnectNode() as NodeBase;
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
