using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    [NodeDisplay("结束", orderID = 10000)]
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.None)]
    [CustomNodeStyle("StartEndNodeStyle")]
    public class NodeEnd : VisualNodeBase
    {

    }
}