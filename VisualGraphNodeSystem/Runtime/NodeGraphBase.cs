using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

namespace VisualGraphNodeSystem
{
    [CreateAssetMenu(fileName = "NewNodeGraph", menuName = "Create  Node Graph/New Node Graph", order = -11)]

    [DefaultNodeType(typeof(VisualNodeBase))]
    public class NodeGraphBase : VisualGraph
    {
 
    }
}
