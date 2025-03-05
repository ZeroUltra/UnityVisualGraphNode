using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
///-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------
using VisualGraphRuntime;

namespace VisualGraphInEditor
{
    public class VisualGraphNodeTreeWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow window;
        private VisualGraphView graphView;
        private List<Type> nodeTypes = new List<Type>();
        //private Texture2D lineTexture2d;

        public void Configure(EditorWindow window, VisualGraphView graphView)
        {
            this.window = window;

            this.graphView = graphView;

            var result = new List<System.Type>();
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            DefaultNodeTypeAttribute typeAttrib = graphView.VisualGraph.GetType().GetCustomAttribute<DefaultNodeTypeAttribute>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                //将所有节点添加到 列表
                foreach (var type in types)
                {
                    if (typeAttrib != null && (type.IsAssignableFrom(typeAttrib.type) == true || type.IsSubclassOf(typeAttrib.type))
                        && type.IsSubclassOf(typeof(VisualGraphRuntime.VisualGraphNode)) == true
                        && type.IsAbstract == false)
                    {
                        nodeTypes.Add(type);
                    }
                }
            }
        }

        //创建搜索tree
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Create Node"), 0));

            List<(int orderID, string disName, string iconName, Type Node)> listMenu = new List<(int orderID, string disName, string iconName, Type node)>();
            //遍历node 菜单
            foreach (var type in nodeTypes)
            {
                var nameAttribute = type.GetCustomAttribute<NodeDisplayAttribute>();
                if (nameAttribute != null)
                {
                    string display_name = nameAttribute.name;
                    int orderID = nameAttribute.orderID;
                    string iconName = nameAttribute.iconName;
                    listMenu.Add((orderID, display_name, iconName, type));
                }
            }
            //排序
            listMenu.Sort((a, b) =>
            {
                var o = a.orderID - b.orderID;
                return o;
            });

            int lastOrderIndex = int.MinValue;
            int index = 0;
            foreach (var item in listMenu)
            {
                if (lastOrderIndex != int.MinValue && item.orderID - lastOrderIndex >= 10)//超过10 加一个横线
                    tree.Add(new SearchTreeEntry(new GUIContent("——————————————————————————————————————————"))
                    {
                        level = 1
                    });
                index++;
                var treeEntry = new SearchTreeEntry(GUIContent.none);
                treeEntry.level = 1;
                treeEntry.userData = item.Node;
                var tex = EditorGUIUtility.IconContent(string.IsNullOrEmpty(item.iconName) ? "ArrowNavigationRight" : item.iconName).image;
                if (tex != null)
                    treeEntry.content = new GUIContent(item.disName, tex);
                else
                    treeEntry.content = new GUIContent(item.disName);
                tree.Add(treeEntry);
                lastOrderIndex = item.orderID;
            }

            #region 名字多级分组处理 例如:AAA/BB 现在不用了
            //foreach (var type in nodeTypes)
            //{
            //    string display_name = "";
            //    int orderID = 0;
            //    if (type.GetCustomAttribute<NodeNameAttribute>() != null)
            //    {
            //        display_name = type.GetCustomAttribute<NodeNameAttribute>().name;
            //        orderID = type.GetCustomAttribute<NodeNameAttribute>().orderID;
            //    }
            //    else
            //    {
            //        display_name = type.Name;
            //    }
            //    if (display_name.Contains("/"))
            //    {
            //        string[] names = display_name.Split('/');
            //        for (int i = 0; i < names.Length; i++)
            //        {
            //            //最后一个添加数据
            //            if (i == names.Length - 1)
            //            {
            //                var treeEntry = new SearchTreeEntry(new GUIContent(names[i]));
            //                treeEntry.level = i+1;
            //                treeEntry.userData = type;
            //                Debug.Log(names[i] + " " + (i + 1));
            //                tree.Add(treeEntry);
            //            }
            //            //添加group
            //            else
            //            {
            //                var tex = EditorGUIUtility.IconContent("ArrowNavigationRight").image;
            //                var newSearchGroup = new SearchTreeGroupEntry(new GUIContent(names[i],tex), i + 1);


            //                bool isExists = tree.Exists(item => item.CompareTo(newSearchGroup) == 0); /*0是相等*/
            //                //不存在就添加
            //                if (isExists == false)
            //                    tree.Add(newSearchGroup);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        tree.Add(new SearchTreeEntry(new GUIContent(display_name))
            //        {
            //            level = 1,
            //            userData = type
            //        });
            //    }
            //};
            #endregion
            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var mousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
            var graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);
            switch (SearchTreeEntry.userData)
            {
                case Type nodeData:
                    {
                        graphView.CreateNode(graphMousePosition, nodeData);
                        return true;
                    }
                    //case Group group:
                    //    graphView.CreateGroupBlock(graphMousePosition);
                    //    return true;
            }
            return false;
        }
    }
}