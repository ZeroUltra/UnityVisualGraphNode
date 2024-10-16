using VisualGraphRuntime;
using NaughtyAttributes;
using UnityEngine;
using NodeGraphView;
[NodeName("播放音频",iconName = "AudioSource Icon")]
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
public class NodePlayAudio : NodeBase
{
    public AudioClip AudioClip;
}