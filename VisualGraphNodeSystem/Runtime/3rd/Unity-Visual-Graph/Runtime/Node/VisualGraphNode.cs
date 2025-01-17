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


        [HideInInspector][NonSerialized] public VisualGraph graph;

        public int NodeIndex => graph.Nodes.IndexOf(this);
        /// <summary>
        /// Get the guid for the Node
        /// </summary>
        public string guid
        {
            get
            {
                if (string.IsNullOrEmpty(internal_guid))
                    internal_guid = Guid.NewGuid().ToString();
                return internal_guid;
            }
        }
        [HideInNormalInspector]
        public int NodeID;
        [HideInNormalInspector]
        public string NodeDescription;
        /// <summary>
        /// List of all ports that belong to this node (ports can be either in or out
        /// </summary>
        [HideInInspector][SerializeReference] public List<VisualGraphPort> Ports = new List<VisualGraphPort>();

        /// <summary>
        /// All Nodes have a guid for references
        /// </summary>
        [HideInInspector][SerializeField] private string internal_guid;

        ///// <summary>
        ///// Node Setup
        ///// </summary>
        //private void OnEnable()
        //{
        //    if (string.IsNullOrEmpty(internal_guid))
        //    {
        //        internal_guid = Guid.NewGuid().ToString();
        //    }
        //}

        /// <summary>
        /// Called when created. Use this to add required ports and additional setup
        /// </summary>
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
            graphPort.guid = Guid.NewGuid().ToString();
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

        /// <summary>
        /// If set the node will highlight in the editor for visual feedback at runtime. It is up to the user to disable
        /// other nodes that are active otherwise you will get undesired results in the view.
        /// </summary>
        [HideInInspector] public bool editor_ActiveNode;

#if UNITY_EDITOR
        #region Graph View Editor Values
        [HideInInspector] public Vector2 position;
        // [HideInInspector][NonSerialized] public object graphElement;
        /// <summary>
        /// 编辑器中的节点视图
        /// </summary>
        [HideInInspector][NonSerialized] public UnityEditor.Experimental.GraphView.Node nodeView;
        #endregion

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



        #endregion
    }
}