using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    [NodeDisplay("结束", order: 10000,icon: "redlight")]
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.None)]
    [CustomNodeStyle("StartEndNodeStyle")]
    public sealed class NodeEnd : VisualNodeBase
    {

    }
}