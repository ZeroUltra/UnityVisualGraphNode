///-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VisualGraphNodeSystem;

namespace VisualGraphRuntime
{
    /// <summary>
    /// Base class for all VisualGraph nodes. Derive from this to create your own nodes. The following attribute 
    /// can be used to customize your Node (otherwise defaults are used):
    /// 
    /// [NodeName(_name: "NAME OF YOUR NODE, OTHERWISE CLASSNAME IS USED")]
    /// OPTIONAL: [NodePortAggregateAttribute(SINGLE OR MULTIPLE INPUT PORTS, SINGLE OR MULTIPLE OUTPUT PORTS)]
    /// OPTIONAL: [PortCapacity(SINGLE OR MULTI INPUT CONNECTIONS PER PORT, SINGLE OR MULTI INPUT CONNECTIONS PER PORT)]
    /// OPTIONAL: [CustomNodeStyle("USS STYLE")]
    /// </summary>
    [Serializable]
    [NodePortAggregate()]
    [PortCapacity()]
    public abstract class VisualGraphNode : ScriptableObject
    {
        public IEnumerable<VisualGraphPort> Inputs { get { foreach (VisualGraphPort port in Ports) { if (port.Direction == VisualGraphPort.PortDirection.Input) yield return port; } } }

        /// <summary>
        /// 输出节点
        /// </summary>
        public IEnumerable<VisualGraphPort> Outputs { get { foreach (VisualGraphPort port in Ports) { if (port.Direction == VisualGraphPort.PortDirection.Output) yield return port; } } }

        public int NodeIndex => graph.Nodes.IndexOf(this);

        public string Guid
        {
            get
            {
                if (string.IsNullOrEmpty(internal_guid))
                    internal_guid = System.Guid.NewGuid().ToString();
                return internal_guid;
            }
        }

        /// <summary>
        /// 节点是否正在运行
        /// </summary>
        public bool IsRunning { get; set; } = false;

        [HideInNormalInspector]
        public int NodeID;
        [HideInNormalInspector]
        public string NodeDescription;

        [HideInInspector][NonSerialized] public VisualGraph graph;
        /// <summary>
        /// List of all ports that belong to this node (ports can be either in or out
        /// </summary>
        [HideInInspector][SerializeReference] public List<VisualGraphPort> Ports = new List<VisualGraphPort>();
        /// <summary>
        /// All Nodes have a guid for references
        /// </summary>
        [HideInInspector][SerializeField] private string internal_guid;

        public virtual void Init()
        {
        }

        public virtual VisualGraphPort AddPort(string name, VisualGraphPort.PortDirection direction)
        {
            DefaultPortTypeAttribute portAttribute = GetType().GetCustomAttribute<DefaultPortTypeAttribute>();
            Type portType = typeof(VisualGraphPort);
            if (portAttribute != null)
            {
                portType = portAttribute.type;
            }

            VisualGraphPort graphPort = Activator.CreateInstance(portType) as VisualGraphPort;
            graphPort.Name = name;
            graphPort.guid = System.Guid.NewGuid().ToString();
            graphPort.Direction = direction;
            Ports.Add(graphPort);

            return graphPort;
        }

        /// <summary>
        /// 当移除节点时调用
        /// </summary>
        /// <param name="socket_port"></param>
        public virtual void RmovePort(VisualGraphPort socket_port)
        {
        }
        public virtual void Update()
        {

        }
        /// <summary>
        /// Find the first port based off guid Id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public VisualGraphPort FindPortByGuid(string guid)
        {
            return Ports.Where(p => p.guid.Equals(guid) == true).FirstOrDefault();
        }

        /// <summary>
        /// Find the first port based off the name. If multiple ports exist the first one will be returned
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public VisualGraphPort FindPortByName(string name)
        {
            return Ports.Where(p => p.Name.Equals(name) == true).FirstOrDefault();
        }

        /// <summary>
        /// Remove all connections this node has
        /// </summary>
        public void ClearConnections()
        {
            foreach (VisualGraphPort port in Ports)
            {
                port.ClearConnections();
            }
        }

        #region UNITY_EDITOR
#if UNITY_EDITOR
        [HideInInspector] public Vector2 position;
        /// <summary>
        /// 编辑器中的节点视图
        /// </summary>
        [HideInInspector][NonSerialized] public UnityEditor.Experimental.GraphView.Node nodeView;

        public virtual System.Type InputType => typeof(bool);
        public virtual System.Type OutputType => typeof(bool);
#endif
        #endregion

        #region 自己添加

        /// <summary>
        /// 获取Outputs输出点
        /// </summary>
        /// <param name="index">索引 从0开始</param>
        /// <returns></returns>
        public VisualGraphPort GetOutpotPortWithIndex(int index)
        {
            return Outputs.ElementAtOrDefault(index);
        }
        public VisualGraphPort GetInputPortWithIndex(int index)
        {
            return Inputs.ElementAtOrDefault(index);
        }
        #endregion
    }
}