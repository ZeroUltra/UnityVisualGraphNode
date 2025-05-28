using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
///-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VisualGraphNodeSystem;
using VisualGraphRuntime;
using static VisualGraphRuntime.VisualGraphPort;
namespace VisualGraphInEditor
{
    public sealed class VisualGraphEditor : EditorWindow
    {

        private VisualGraphView graphView;
        private VisualGraphBase visualGraph;
        // public UnityEngine.Object objectSelection; // Used for enter/exit playmode

        private float scale = 1;
        private Vector3 pos = Vector3.zero;



        /// <summary>
        /// Create a Visual Graph Window to support a VisualGraph object
        /// </summary>
        /// <param name="_visualGraph"></param>
        /// <returns></returns>
        public static VisualGraphEditor CreateGraphViewWindow(VisualGraphBase _visualGraph, bool forceSet = false)
        {
            var window = GetWindow<VisualGraphEditor>();
            window.SetVisualGraph(_visualGraph, forceSet);
            return window;
        }

        /// <summary>
        /// Create visual elements using Unity GraphView (Experiemental)
        /// </summary>
        private void OnEnable()
        {
            rootVisualElement.Clear();

            // Create the GraphView
            graphView = new VisualGraphView(this)
            {
                name = "Visual Graph View",
            };
            graphView.StretchToParentSize();
            graphView.SetGraph(visualGraph);
            graphView.UpdateViewTransform(pos, Vector3.one * scale);
            rootVisualElement.Add(graphView);

            // Add Toolbar to Window
            GenerateToolbar();
            //WaitUpdateView();
            //黑板
            //graphView.CreateBlackboard();

            EditorApplication.playModeStateChanged += OnPlayModeState;

            graphView.OnEnable();
        }

        private void OnDisable()
        {
            scale = graphView.scale;
            pos = graphView.contentViewContainer.transform.position;
            EditorApplication.playModeStateChanged -= OnPlayModeState;

            rootVisualElement.Clear();
            graphView.Clear();
            graphView.OnDisable();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        //private void UpdatePos(GraphView.FrameType frameType, VisualElement contentViewContainer, VisualElement graphElement)
        //{
        //    Rect rectToFit = contentViewContainer.layout;
        //    Vector3 frameTranslation = Vector3.zero;
        //    Vector3 frameScaling = Vector3.one;

        //    //if (frameType == GraphView.FrameType.Selection &&
        //    //    (graphView.selection.Count == 0 || !graphView.selection.Any(e => e.IsSelectable() && !(e is Edge))))
        //    //    frameType = GraphView.FrameType.All;

        //    // if (frameType == GraphView.FrameType.Selection)
        //    // {
        //    //  VisualElement graphElement = graphView.selection[0] as GraphElement;
        //    if (graphElement != null)
        //    {
        //        var rect = new Rect(0.0f, 0.0f, graphElement.layout.width, graphElement.layout.height);
        //        // Edges don't have a size. Only their internal EdgeControl have a size.
        //        if (graphElement is Edge)
        //            graphElement = (graphElement as Edge).edgeControl;
        //        rectToFit = graphElement.ChangeCoordinatesTo(contentViewContainer, rect);
        //    }
        //    var array = new VisualElement[] { graphElement };
        //    rectToFit = array.Cast<GraphElement>()
        //        .Aggregate(rectToFit, (current, currentGraphElement) =>
        //        {
        //            var rect = new Rect(0.0f, 0.0f, currentGraphElement.layout.width, currentGraphElement.layout.height);

        //            VisualElement currentElement = currentGraphElement;
        //            if (currentGraphElement is Edge)
        //                currentElement = (currentGraphElement as Edge).edgeControl;
        //            return RectUtils.Encompass(current, currentElement.ChangeCoordinatesTo(contentViewContainer, rect));
        //        });
        //    GraphView.CalculateFrameTransform(rectToFit, graphView.layout, 30, out frameTranslation, out frameScaling);

        //    // }
        //    //else if (frameType == GraphView.FrameType.All)
        //    //{
        //    // rectToFit = graphView.CalculateRectToFitAll(contentViewContainer);
        //    // GraphView.CalculateFrameTransform(rectToFit, graphView.layout, 30, out frameTranslation, out frameScaling);
        //    //} // else keep going if (frameType == FrameType.Origin)
        //    bool m_FrameAnimate = false;

        //    if (m_FrameAnimate)
        //    {

        //    }
        //    else
        //    {
        //        Matrix4x4.TRS(frameTranslation, Quaternion.identity, frameScaling);
        //        //  graphView.UpdateViewTransform(frameTranslation, frameScaling);

        //        graphView.UpdateViewTransform(frameTranslation * 1.6f, frameScaling * 0.5f);
        //    }

        //    contentViewContainer.MarkDirtyRepaint();

        //    //  GraphView.UpdatePersistedViewTransform();

        //}

        private void OnPlayModeState(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    // objectSelection = Selection.activeObject;
                    Selection.activeObject = null;
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    //进入运行时保持原来的样子
                    // Selection.activeObject = objectSelection;
                    graphView.SetGraph(visualGraph);
                    graphView.UpdateViewTransform(pos, Vector3.one * scale);
                    break;

                case PlayModeStateChange.ExitingPlayMode:
                    //objectSelection = Selection.activeObject;
                    //Selection.activeObject = null;
                    foreach (var item in visualGraph.Nodes)
                    {
                        item.IsRunning = false;
                        if (item.IsRunning)
                        {
                            item.IsRunning = false;
                            (item.nodeView as VisualGraphNodeView).SetBorderColor(Color.clear);
                        }
                    }
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    // Selection.activeObject = objectSelection;
                    break;
            }
        }

        /// <summary>
        /// Change the Visual Graph
        /// </summary>
        /// <param name="_visualGraph"></param>
        private void SetVisualGraph(VisualGraphBase _visualGraph, bool forceSet = false)
        {
            visualGraph = _visualGraph;
            if (visualGraph == null)
            {
                titleContent = new GUIContent("Empty");
            }
            else
            {
                titleContent = new GUIContent(visualGraph.name, EditorGUIUtility.IconContent("ScriptableObject Icon").image);
            }
            graphView.SetGraph(visualGraph);
        }

        /// <summary>
        /// Window toolbar 绘制toolbar
        /// </summary>
        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            #region 按钮->显示minimap
            ToolbarToggle minimap_toggle = new ToolbarToggle();

            //Toggle minimap_toggle = new Toggle();
            minimap_toggle.text = "Show MiniMap";
            minimap_toggle.SetValueWithoutNotify(false);
            minimap_toggle.RegisterCallback<ChangeEvent<bool>>(
                (evt) =>
                {
                    graphView.ShowMinimap(evt.newValue);
                }
            );
            toolbar.Add(minimap_toggle);
            #endregion

            #region 按钮->显示网格
            ToolbarToggle tog = new ToolbarToggle();
            tog.text = "Show Grid";
            tog.SetValueWithoutNotify(false);
            tog.RegisterCallback<ChangeEvent<bool>>(
                (evt) =>
                {
                    graphView.ShowGrid(evt.newValue);
                });
            toolbar.Add(tog);
            #endregion

            #region 按钮->序列化保存
            ToolbarButton btnSave = new ToolbarButton();
            btnSave.text = "Serialize And Save";
            btnSave.tooltip = "序列化保存所有节点";
            btnSave.style.width = 135;
            btnSave.style.unityTextAlign = TextAnchor.MiddleRight;
            Image icon = new Image();
            icon.image = EditorGUIUtility.IconContent("d_SaveAs").image;
            icon.style.paddingRight = 105;
            icon.style.paddingTop = 1;
            btnSave.Add(icon);
            btnSave.RegisterCallback<ClickEvent>((evt) =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var node in visualGraph.Nodes)
                {
                    if (node is VisualNodeBase nodeBase)
                    {
                        string code = nodeBase.ToSerialize();
                        if (!string.IsNullOrEmpty(code))
                        {
                            sb.AppendLine(code);
                        }
                    }
                }
                string selePath = EditorUtility.SaveFilePanel(btnSave.text, Application.dataPath, visualGraph.name, null);
                if (string.IsNullOrEmpty(selePath) == false)
                {
                    System.IO.File.WriteAllText(selePath, sb.ToString());
                }
            });

            toolbar.Add(btnSave);
            #endregion

            #region 按钮->排序id
            ToolbarButton btnSortID = new ToolbarButton();
            btnSortID.text = "Sort ID";
            btnSortID.tooltip = "将所有节点的ID排序";
            btnSortID.style.width = 60;
            btnSortID.style.unityTextAlign = TextAnchor.MiddleCenter;
            btnSortID.RegisterCallback<ClickEvent>((evt) =>
            {
                var startNode = visualGraph.StartNode;
                int id = 1;
                if (startNode != null)
                {
                    SortRecursive(startNode);
                }
                void SortRecursive(VisualGraphNode node)
                {
                    if (node == null) return;
                    var outputPort = node.Ports.FirstOrDefault(x => x.Direction == PortDirection.Output);
                    if (outputPort == null)
                    {
                        return;
                    }
                    var nextNode = outputPort.GetConnectNode();
                    if (nextNode == null) return;
                    nextNode.NodeID = id;
                    ++id;
                    SortRecursive(nextNode);
                }
                OnDisable();
                OnEnable();
                EditorUtility.SetDirty(visualGraph);
            });
            toolbar.Add(btnSortID);
            #endregion

            #region 按钮->删除联系
            ToolbarButton btnDelPort = new ToolbarButton();
            btnDelPort.tooltip = "当打开找不到联系时,可以尝试删除所有联系,重新连接";
            btnDelPort.text = "ClearConnections";
            btnDelPort.style.width = 120;
            btnDelPort.style.unityTextAlign = TextAnchor.MiddleCenter;
            btnDelPort.RegisterCallback<ClickEvent>((evt) =>
            {
                if (UnityEditor.EditorUtility.DisplayDialog("警告", "是否删除所有联系?", "确认", "取消"))
                {
                    foreach (var node in visualGraph.Nodes)
                    {
                        node.ClearConnections();
                    }
                    EditorUtility.SetDirty(visualGraph);
                }
            });
            toolbar.Add(btnDelPort);
            #endregion

            //添加空格
            toolbar.Add(new ToolbarSpacer() { flex = true });

            #region 添加搜索
            ToolbarSearchField toolbarSearch = new ToolbarSearchField();
            toolbarSearch.tooltip = "Search nodes (t:类型 n:名字 d:描述)";
            var text = toolbarSearch.Q<TextField>();
            text.isDelayed = true;
            text.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                string searchText = evt.newValue.ToLower();

                List<VisualGraphNode> findNodes = new List<VisualGraphNode>();
                if (searchText.StartsWith("t:"))
                {
                    findNodes = visualGraph.Nodes.FindAll(x => x.GetType().Name.ToLower().Contains(searchText.Substring(2)));
                }
                else if (searchText.StartsWith("n:"))
                {
                    findNodes = visualGraph.Nodes.FindAll(x => x.GetType().GetCustomAttribute<NodeDisplayAttribute>().Name.ToLower().Contains(searchText.Substring(2)));
                }
                else if (searchText.StartsWith("d:"))
                {
                    findNodes = visualGraph.Nodes.FindAll(x => x.NodeDescription.ToLower().Contains(searchText.Substring(2)));
                }
                Debug.Log("搜索结果:" + findNodes.Count);
                if (findNodes.Count > 0)
                {
                    //排序
                    findNodes.Sort((x, y) => x.NodeID.CompareTo(y.NodeID));
                    graphView.ClearSelection();
                    graphView.AddToSelection(findNodes[0].nodeView);
                    graphView.FrameSelection();
                    if (findNodes.Count > 1)
                    {
                        //想在这添加两个按钮表示上一个下一个，中间是索引文本
                        var btnNext = new Label();
                        btnNext.text = "↓";
                        btnNext.style.width = 20;
                        btnNext.style.unityTextAlign = TextAnchor.MiddleCenter;

                        var btnPrev = new Label();
                        btnPrev.text = "↑";
                        btnPrev.style.width = 20;
                        btnPrev.style.unityTextAlign = TextAnchor.MiddleCenter;

                        var indexText = new Label();
                        indexText.text = "1/" + findNodes.Count;
                        indexText.style.width = 40;
                        indexText.style.unityTextAlign = TextAnchor.MiddleCenter;

                        toolbarSearch.Add(btnPrev);
                        toolbarSearch.Add(indexText);
                        toolbarSearch.Add(btnNext);

                        int currentIndex = 1;
                        btnNext.RegisterCallback<ClickEvent>((evt) =>
                        {
                            currentIndex++;
                            if (currentIndex > findNodes.Count)
                            {
                                currentIndex = 1;
                            }
                            indexText.text = currentIndex + "/" + findNodes.Count;
                            graphView.ClearSelection();
                            graphView.AddToSelection(findNodes[currentIndex - 1].nodeView);
                            graphView.FrameSelection();
                        });
                        btnPrev.RegisterCallback<ClickEvent>((evt) =>
                        {
                            currentIndex--;
                            if (currentIndex < 1)
                            {
                                currentIndex = findNodes.Count;
                            }
                            indexText.text = currentIndex + "/" + findNodes.Count;
                            graphView.ClearSelection();
                            graphView.AddToSelection(findNodes[currentIndex - 1].nodeView);
                            graphView.FrameSelection();
                        });
                    }
                }
                if (findNodes.Count == 0)
                {
                    //移除最后三个 本身有6个
                    if (toolbarSearch.childCount == 6)
                    {
                        toolbarSearch.RemoveAt(5);
                        toolbarSearch.RemoveAt(4);
                        toolbarSearch.RemoveAt(3);
                    }
                    graphView.ClearSelection();
                    //toolbarSearch.SetValueWithoutNotify(string.Empty);
                }
            });
            toolbar.Add(toolbarSearch);
            #endregion

            #region 按钮->重新加载
            ToolbarButton btnReload = new ToolbarButton();
            btnReload.text = "Reload";
            btnReload.style.width = 60;
            btnReload.style.unityTextAlign = TextAnchor.MiddleCenter;
            btnReload.RegisterCallback<ClickEvent>((evt) =>
            {
                OnDisable();
                OnEnable();
            });
            toolbar.Add(btnReload);
            #endregion

            #region MyRegion
            //黑板
            //ToolbarToggle blackboard_toggle = new ToolbarToggle();
            //blackboard_toggle.text = "Show Blackboard";
            //blackboard_toggle.SetValueWithoutNotify(true);
            //blackboard_toggle.RegisterCallback<ChangeEvent<bool>>(
            //    (evt) =>
            //    {
            //        graphView.Blackboard.visible = evt.newValue;
            //    }
            //);
            //toolbar.Add(blackboard_toggle);

            //Button btnTest = new Button();
            //btnTest.text = "增加10个";
            //btnTest.RegisterCallback<ClickEvent>((evt) =>
            //{
            //    for (int i = 0; i < 20; i++)
            //    {
            //        var node = graphView.CreateNode(new Vector2(posX,(i * 60)) , typeof(Node_SayOther));
            //    }
            //    posX += 300;
            //});
            //toolbar.Add(btnTest);

            //绘制一个按钮
            //Button btnasset = new Button();
            //btnasset.text = "Open Assets";
            //btnasset.RegisterCallback<ClickEvent>((evt) =>
            //{
            //    var obj = (visualGraph as AVGGraph).graphAssets;
            //    if (obj != null)
            //        AssetDatabase.OpenAsset(obj);
            //});
            //toolbar.Add(btnasset);

            //绘制obj区域
            //ObjectField assetobj = new ObjectField();
            //assetobj.allowSceneObjects = false;
            //assetobj.objectType = typeof(AVGGraphAssets);
            //assetobj.style.width = 300;
            ////if (visualGraph == null) Debug.Log("sdfasd");
            //if ((visualGraph as AVGGraph).graphAssets != null)
            //    assetobj.value = (visualGraph as AVGGraph).graphAssets;
            ////改变事件
            //assetobj.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            //{
            //    if (evt.newValue != null)
            //        (visualGraph as AVGGraph).graphAssets = evt.newValue as AVGGraphAssets;
            //});
            //toolbar.Add(assetobj); 
            #endregion

            rootVisualElement.Add(toolbar);
        }
        /// <summary>
        /// When the GUI changes update the view (this positions the blackboard and minimap)
        /// </summary>
        private void OnGUI()
        {
            if (graphView != null)
            {
                graphView.OnGUI();
            }
        }


        private void Update()
        {
            //运行时高亮显示正在运行时的节点
            if (Application.isPlaying)
            {
                if (visualGraph != null)
                {
                    foreach (var node in visualGraph.Nodes)
                    {
                        if (node.nodeView is VisualGraphNodeView nodeview)
                        {
                            if (node.IsRunning)
                            {
                                //graphView.AddToSelection(nodebase.nodeView);
                                //graphView.FrameSelection();
                                nodeview.SetBorderColor(Color.green);
                                nodeview.SetBorderWidth(4);
                            }
                            else
                            {
                                nodeview.SetBorderColor(Color.clear);
                            }
                        }
                    }
                }
            }
        }
    }
}