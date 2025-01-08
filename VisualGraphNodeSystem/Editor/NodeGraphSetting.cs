
using System.Diagnostics;
using UnityEngine;

namespace VisualGraphNodeSystem.Editor
{
    [CreateAssetMenu(fileName = "NodeGraphSetting", menuName = "Create  Node Graph/Node Graph Setting", order = -10)]
    public class NodeGraphSetting : ScriptableObject
    {
        private static NodeGraphSetting _instance;
        public static NodeGraphSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Helper.FindScriptableObject<NodeGraphSetting>("NodeGraphSetting");
                }
                return _instance;
            }
        }

        [Header("标签宽度")]
        public int LabelWidth = 120;
        [Header("默认node大小")]
        public Vector2 NodeDefaultSize = new Vector2(250, 120);
        [Header("是否使用IMGUI")]
        public bool IsUseIMGUI = true;
        [Header("是否显示node的Index")]
        public bool IsShowIndex = false;
        [Header("是否显示描述")]
        public bool IsShowDesc = true;
        [Header("是否显示ID")]
        public bool IsShowID = true;
        [Header("默认node的背景颜色")]
        public Color DefaultNodeTitleBgColor = Color.black;
        [Header("是否显示node的Icon(如果有)")]
        public bool IsShowNodeIcon = true;
    }
}