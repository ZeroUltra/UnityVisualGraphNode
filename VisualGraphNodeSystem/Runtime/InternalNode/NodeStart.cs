
using VisualGraphNodeSystem;
namespace VisualGraphRuntime
{
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.None, NodePortAggregateAttribute.PortAggregate.Single)]
    [NodeDisplay(name: "Start", order: -10000, icon: "greenlight")]
    [CustomNodeStyle("StartEndNodeStyle")]
    public sealed class NodeStart : VisualNodeBase
    {

    }
}