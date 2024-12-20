using VisualGraphRuntime;
using UnityEngine;
using System.Linq;

namespace VisualGraphNodeSystem
{
    [NodeName("选项面板", 1000,iconName= "listview on")]
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Multiple)]
    public class NodeOption: NodeBase
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