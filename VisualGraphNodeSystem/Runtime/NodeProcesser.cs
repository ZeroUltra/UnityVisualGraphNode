namespace VisualGraphNodeSystem
{
    public class NodeProcesser
    {
        public VisualGraphBase TargetNodeGraph { get; private set; }

        public NodeProcesser(VisualGraphBase nodeGraph)
        {
            this.TargetNodeGraph = nodeGraph;
            nodeGraph.InitializeGraph();
        }
        public VisualNodeBase GetFirstNode()
        {
            return TargetNodeGraph.StartNode.GetOutpotPortWithIndex(0).GetConnectNode() as VisualNodeBase;
        }

        public virtual void Process(VisualNodeBase currNode)
        {
#if UNITY_EDITOR
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
