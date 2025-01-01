using VisualGraphRuntime;
using UnityEngine;
using VisualGraphNodeSystem;
[NodeName("播放音频",iconName = "AudioSource Icon")]
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
public class NodePlayAudio : VisualNodeBase
{
    public AudioClip AudioClip;
}