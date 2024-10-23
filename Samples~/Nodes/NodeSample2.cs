
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
using VisualGraphNodeSystem;

[NodeName("node示例2", 11, iconName = "UnityLogo")]
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.None)]
public class NodeSample2 : NodeBase
{
    public string str;
    public int num;
}


