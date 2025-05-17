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
    public class NodeDisplayAttribute : Attribute
    {
        public string Name { get; private set; }
        public int Order { get; private set; }
        public string Icon { get; private set; }
        public bool VisableDesc { get; private set; }
        public Color TitleColor { get; private set; } = default;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">节点名</param>
        /// <param name="order">节点排序</param>
        /// <param name="icon">节点图标名 必须是untiy内置图标</param>
        /// <param name="visableDesc">是否可见描述,如果想部分不要描述设置它为false</param>
        /// <param name="titleColorString">标题背景颜色 格式为 #FFFFFFFF</param>
        public NodeDisplayAttribute(string name, int order = 0, string icon = null, bool visableDesc = true, string titleColorString = null)
        {
            this.Name = name;
            this.Order = order;
            this.Icon = icon;
            this.VisableDesc = visableDesc;
            if (!string.IsNullOrEmpty(titleColorString))
            {
                if (!ColorUtility.TryParseHtmlString(titleColorString, out Color color))
                {
                    Debug.LogError($"Converted color error: [{titleColorString}]");
                }
                else
                {
                    TitleColor = color;
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
        /// 自定义节点样式
        /// </summary>
        /// <param name="_name"></param>
        public CustomNodeStyleAttribute(string style)
        {
            this.style = style;
        }
    }
}