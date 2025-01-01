using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    [NodeName("结束", orderID = 10000/*,iconName="save"*/)]
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.None)]
    [CustomNodeStyle("StartNodeStyle")]
    public class NodeEnd : VisualNodeBase
    {

    }
}