using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;
namespace NodeGraphView
{
    [CreateAssetMenu(fileName = "NodeGraphSetting", menuName = "Create  Node Graph/Node Graph Setting", order = -10)]
    public class NodeGraphSetting : ScriptableObject
    {
        public static NodeGraphSetting Instance
        {
            get
            {
                return NodeHelper.FindScriptableObject<NodeGraphSetting>();
            }
        }

        [InfoBox("NodeGraph 设置")]
        [Header("标签宽度")]
        public int LabelWidth = 120;
        [Header("默认node大小")]
        public Vector2 NodeDefaultSize = new Vector2(250, 120);
    }
}