using VisualGraphNodeSystem;
using VisualGraphRuntime;
[NodeName("选项返回", 1001, iconName = "rotatetool on")]
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.None)]
public class NodeOptionBack : VisualNodeBase
{
    /// <summary>
    /// 返回Node的ID  
    /// </summary>
    public int BackNodeID;
}