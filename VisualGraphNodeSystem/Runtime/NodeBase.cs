using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    /// <summary>
    /// Node基类
    /// </summary>
    [CustomNodeStyle("NodeStyle")]
    public abstract class NodeBase : VisualGraphRuntime.VisualGraphNode
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