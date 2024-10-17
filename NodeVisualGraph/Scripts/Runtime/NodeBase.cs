using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
namespace NodeGraphView
{
    /// <summary>
    /// Node基类
    /// </summary>
    [CustomNodeStyle("NodeStyle")]
    public abstract class NodeBase : VisualGraphNode
    {
        public virtual string ToSerialize()
        {
            return null;
        }
        public virtual void FromSerialize(string str)
        { 
            
        }
    }
}