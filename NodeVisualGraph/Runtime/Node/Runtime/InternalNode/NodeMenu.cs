using VisualGraphRuntime;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace NodeGraphView
{
    [NodeName("选项面板", -11)]
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Multiple)]
    public class NodeMenu : NodeBase
    {
        public string[] Options()
        { 
            return Outputs.Select(x => x.Name).ToArray();
        }

       public string this[int index]
        {
            get
            {
                return Outputs.ElementAt(index).Name;
            }
        }
    }
}