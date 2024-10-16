using VisualGraphRuntime;
using NaughtyAttributes;
using UnityEngine;
using NodeGraphView;
[NodeName("NodeWait")]
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
public class NodeWait : NodeBase
{
    public float waitDuration = 1.0f;
}
