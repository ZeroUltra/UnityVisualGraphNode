using System.Linq;
using VisualGraphRuntime;

namespace VisualGraphNodeSystem
{
    [NodeDisplay("选项面板", 1000, icon: "listview on")]
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Multiple)]
    public class NodeOption : VisualNodeBase
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