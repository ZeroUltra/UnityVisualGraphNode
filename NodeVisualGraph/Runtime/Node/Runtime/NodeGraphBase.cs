using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

namespace NodeGraphView
{
    [CreateAssetMenu(fileName = "NewNodeGraph", menuName = "Create  Node Graph/New Node Graph", order = -11)]

    [DefaultNodeType(typeof(NodeBase))]
    public class NodeGraphBase : VisualGraph
    {
       
    }
}
