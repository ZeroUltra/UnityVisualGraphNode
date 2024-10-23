using VisualGraphRuntime;
using UnityEngine;
using VisualGraphNodeSystem;
[NodeName("NodeWait")]
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
public class NodeWait : NodeBase
{
    public float waitDuration = 1.0f;

    public override string ToSerialize()
    {
        return $"@NodeWait|{waitDuration}";
    }
    public override void FromSerialize(string str)
    {
        waitDuration = float.Parse(str.Split("|")[1]);
    }
}
