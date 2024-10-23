using VisualGraphRuntime;
using VisualGraphNodeSystem;

//节点名字 排序(超过10中间会有横线 图标名字(unity中内置图标名字))
[NodeName("node示例", 1, iconName = "d_ContentSizeFitter Icon")]
//输入输出端口类型 (输入输出端口数量)
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
public class NodeSample : NodeBase
{
    public float waitDuration;
}

