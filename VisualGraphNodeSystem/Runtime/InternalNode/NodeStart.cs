///-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------
using VisualGraphNodeSystem;
namespace VisualGraphRuntime
{
    /// <summary>
    /// Start node for the VisualGraph. All Graphs will have this node added at the beginning.
    /// Possible future development will remove this and give the option to create one.
    /// </summary>
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.None, NodePortAggregateAttribute.PortAggregate.Single)]
    [NodeDisplay(name: "Start")]
    [CustomNodeStyle("StartEndNodeStyle")]
    public sealed class NodeStart : VisualGraphNode
    {

    }
}