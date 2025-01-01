using System.Linq;
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
        private VisualGraph visualGraph;
        public UnityEngine.Object objectSelection; // Used for enter/exit playmode

        private float scale = 1;
        private Vector3 pos = Vector3.zero;



        /// <summary>
        /// Create a Visual Graph Window to support a VisualGraph object
        /// </summary>
        /// <param name="_visualGraph"></param>
        /// <returns></returns>
        public static VisualGraphEditor CreateGraphViewWindow(VisualGraph _visualGraph, bool forceSet = false)
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
            NodeProcesser.OnChangeNodeEvent += OnChangeNodeEvent;
            graphView.OnEnable();
        }

        //运行时显示
        private void OnChangeNodeEvent(VisualNodeBase nodebase)
        {
            if (!Application.isPlaying) return;
            graphView.ClearSelection();
            if (nodebase != null)
            {
                graphView.AddToSelection(nodebase.graphElement as VisualGraphNodeView);
            }
        }

        private void OnDisable()
        {
            scale = graphView.scale;
            pos = graphView.contentViewContainer.transform.position;
            EditorApplication.playModeStateChanged -= OnPlayModeState;
            NodeProcesser.OnChangeNodeEvent -= OnChangeNodeEvent;
            rootVisualElement.Clear();
            graphView.Clear();
            graphView.OnDisable();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }


        private void OnPlayModeState(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    objectSelection = Selection.activeObject;
                    Selection.activeObject = null;
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    Selection.activeObject = objectSelection;
                    graphView.SetGraph(visualGraph);
                    graphView.UpdateViewTransform(pos, Vector3.one * scale);
                    break;

                case PlayModeStateChange.ExitingPlayMode:
                    objectSelection = Selection.activeObject;
                    Selection.activeObject = null;
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    Selection.activeObject = objectSelection;
                    break;
            }
        }

        /// <summary>
        /// Change the Visual Graph
        /// </summary>
        /// <param name="_visualGraph"></param>
        private void SetVisualGraph(VisualGraph _visualGraph, bool forceSet = false)
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
            //添加空格
            toolbar.Add(new ToolbarSpacer() { flex = true });

            #region 添加搜索
            ToolbarSearchField toolbarSearch = new ToolbarSearchField();
            toolbarSearch.tooltip = "Search nodes by Desc";
            var text = toolbarSearch.Q<TextField>();
            text.isDelayed = true;
            text.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                foreach (var node in visualGraph.Nodes)
                {
                    if (string.IsNullOrEmpty(node.NodeDescription)) continue;
                    if (node.NodeDescription.Contains(evt.newValue))
                    {
                        var targetNode = node.graphElement as VisualGraphNodeView;
                        graphView.ClearSelection();
                        graphView.AddToSelection(targetNode);
                        graphView.FrameSelection();
                        break;
                    }
                }
                toolbarSearch.SetValueWithoutNotify(string.Empty);
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

        /// <summary>
        /// TODO: When this works we can highlight the node that is active great for
        ///		  runtime cases and viewing things like FSM
        /// </summary>
        //private void Update()
        //{
        //    if (Application.isPlaying)
        //    {
        //        if (graphView != null)
        //        {
        //            graphView.Update();
        //        }
        //    }
        //}

        /// <summary>
        /// Handle selection change. This will check the active object to see if it is a
        /// VisualGraph Scriptable object. If it is not then it will see if the selected
        /// object is a GameObject. If the selection is a GameObject then we iterate over
        /// all MonoBehaviour (scripts) to see if one is a has a VisualGraphMonoBehaviour<>.
        /// If we find a Component that is a VisualGraphMonoBehaviour<> then we first check
        /// if there is an InternalGraph (which is used during runtime) otherwise we will
        /// use the Graph itself (needs to change when runtime is invoked in the editor)
        /// </summary>
        //void OnSelectionChange()
        //{
        //    return;
        //    visualGraphComponent = null;

        //    VisualGraph graph = Selection.activeObject as VisualGraph;
        //    if (graph == null && graphView.activeVisualGraph)
        //    {
        //        //SetVisualGraph(null);
        //    }
        //    else if (graph != null && graph != visualGraph)
        //    {
        //        if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(graph)))
        //        {
        //            SetVisualGraph(graph);
        //        }
        //    }
        //    else
        //    {
        //        GameObject go = Selection.activeObject as GameObject;
        //        if (go != null)
        //        {
        //            Component[] components = go.GetComponents(typeof(MonoBehaviour));
        //            foreach (var comp in components)
        //            {
        //                // Because everything in Components is a MonoBehaviour we can get the base type
        //                // If they base type is a generic of type VisualGraphMonoBehaviour<> then we can try and
        //                // get the internal graph (this is for editor runtime). If that doesn't exist use the set graph.
        //                if (comp == null) continue;

        //                Type t = comp.GetType().BaseType;
        //                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(VisualGraphMonoBehaviour<>))
        //                {
        //                    graph = (VisualGraph)t.GetField("internalGraph", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(comp);
        //                    if (graph != null)
        //                    {
        //                        visualGraphComponent = comp;
        //                        SetVisualGraph(graph);
        //                        return;
        //                    }

        //                    //graph = (VisualGraph)t.GetField("graph", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(comp);
        //                    graph = (VisualGraph)t.GetField("graph").GetValue(comp);
        //                    if (graph != null)
        //                    {
        //                        visualGraphComponent = comp;
        //                        SetVisualGraph(graph);
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


    }
}