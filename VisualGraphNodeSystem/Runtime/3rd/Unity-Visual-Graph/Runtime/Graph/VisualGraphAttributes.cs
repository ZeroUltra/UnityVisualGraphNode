///-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------
using System;
using UnityEngine;
namespace VisualGraphRuntime
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NodeNameAttribute : Attribute
    {
        public string name;
        public int orderID;
        public string iconName;
        public Color titleBgColor = default;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">节点名</param>
        /// <param name="order">节点排序</param>
        /// <param name="iconName">节点图标名 必须是untiy内置图标</param>
        /// <param name="titleBgColorString">标题背景颜色 格式为 #FFFFFFFF</param>
        public NodeNameAttribute(string name, int order = 0, string iconName = null, string titleBgColorString = null)
        {
            this.name = name;
            this.orderID = order;
            this.iconName = iconName;
            if (!string.IsNullOrEmpty(titleBgColorString))
            {
                if (!ColorUtility.TryParseHtmlString(titleBgColorString, out Color color))
                {
                    Debug.LogError($"Converted color error: [{titleBgColorString}]");
                }
                else
                {
                    titleBgColor = color;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultNodeTypeAttribute : Attribute
    {
        public Type type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name"></param>
        public DefaultNodeTypeAttribute(Type _type)
        {
            type = _type;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultPortTypeAttribute : Attribute
    {
        public Type type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name"></param>
        public DefaultPortTypeAttribute(Type _type)
        {
            type = _type;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NodePortAggregateAttribute : Attribute
    {
        public enum PortAggregate
        {
            None,
            Single,
            Multiple
        };
        public PortAggregate InputPortAggregate = PortAggregate.Single;
        public PortAggregate OutputPortAggregate = PortAggregate.Multiple;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name"></param>
        public NodePortAggregateAttribute(PortAggregate InputPortDynamics = PortAggregate.Single, PortAggregate OutputPortDynamics = PortAggregate.Multiple)
        {
            this.InputPortAggregate = InputPortDynamics;
            this.OutputPortAggregate = OutputPortDynamics;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PortCapacityAttribute : Attribute
    {
        //
        // Summary:
        //     Specify how many edges a port can have connected.
        public enum Capacity
        {
            //
            // Summary:
            //     Port can only have a single connection.
            Single = 0,
            //
            // Summary:
            //     Port can have multiple connections.
            Multi = 1
        }
        public Capacity InputPortCapacity = Capacity.Multi;
        public Capacity OutputPortCapacity = Capacity.Single;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name"></param>
        public PortCapacityAttribute(Capacity InputPortCapacity = Capacity.Multi, Capacity OutputPortCapacity = Capacity.Single)
        {
            this.InputPortCapacity = InputPortCapacity;
            this.OutputPortCapacity = OutputPortCapacity;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GraphOrientationAttribute : Attribute
    {
        //
        // Summary:
        //     Graph element orientation.
        public enum Orientation
        {
            // Summary:
            //     Horizontal orientation used for nodes and connections flowing to the left or
            //     right.
            Horizontal = 0,
            //
            // Summary:
            //     Vertical orientation used for nodes and connections flowing up or down.
            Vertical = 1
        }
        public Orientation GrapOrientation = Orientation.Horizontal;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name"></param>
        public GraphOrientationAttribute(Orientation GrapOrientation = Orientation.Horizontal)
        {
            this.GrapOrientation = GrapOrientation;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomNodeStyleAttribute : Attribute
    {
        public string style;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name"></param>
        public CustomNodeStyleAttribute(string style)
        {
            this.style = style;
        }
    }
}