using VisualGraphRuntime;
using UnityEngine;
using VisualGraphNodeSystem;
namespace VisualGraphNodeSystem.Test
{
    [NodeDisplay("(Test)播放音频", icon: "AudioSource Icon")]
    [NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
    public class NodePlayAudio : VisualNodeBase
    {
        public AudioClip AudioClip;
    }
}