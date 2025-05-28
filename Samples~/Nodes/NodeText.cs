using VisualGraphRuntime;
using UnityEngine;
using VisualGraphNodeSystem;
namespace VisualGraphNodeSystem.Test
{
    [NodeDisplay("(Test)文本")]
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
    public class NodeText : VisualNodeBase
    {
        public string Text;
    }
}