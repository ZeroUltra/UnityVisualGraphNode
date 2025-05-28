namespace VisualGraphNodeSystem
{
    public class NodeProcesser
    {
        public VisualGraphBase NodeGraph { get; private set; }
        public NodeProcesser(VisualGraphBase nodeGraph)
        {
            this.NodeGraph = nodeGraph;
            nodeGraph.InitializeGraph();
        }
        public VisualNodeBase GetFirstNode()
        {
           return NodeGraph.StartNode.GetOutpotPortWithIndex(0).GetConnectNode() as VisualNodeBase;
        }

        /// <summary>
        /// 调用此方法可以刷新编辑器node状态
        /// </summary>
        /// <param name="currNode"></param>
        public virtual void Process(VisualNodeBase currNode)
        {
#if UNITY_EDITOR
            if(currNode == null)
                UnityEngine.Debug.LogError("currNode is null !!!");
            currNode.IsRunning = true;
            var prevNode = GetPrevNodeWithInputPort(currNode, 0);
            if (prevNode != null)
            {
                prevNode.IsRunning = false;
            }
#endif
        }

        /// <summary>
        /// 获取下一个node
        /// </summary>
        /// <param name="currentNode">当前node</param>
        /// <param name="outputPortIndex">port index</param>
        public VisualNodeBase GetNextNodeWithOutputPort(VisualNodeBase currentNode, int outputPortIndex)
        {
            var port = currentNode.GetOutpotPortWithIndex(outputPortIndex);
            if (port != null)
                currentNode = port.GetConnectNode() as VisualNodeBase;
            else
                currentNode = null;
            return currentNode;
        }
        /// <summary>
        /// 获取上一个node
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="inputPortIndex"></param>
        /// <returns></returns>
        public VisualNodeBase GetPrevNodeWithInputPort(VisualNodeBase currentNode, int inputPortIndex)
        {
            var port = currentNode.GetInputPortWithIndex(inputPortIndex);
            if (port != null)
                currentNode = port.GetConnectNode() as VisualNodeBase;
            else
                currentNode = null;
            return currentNode;
        }
    }
}
