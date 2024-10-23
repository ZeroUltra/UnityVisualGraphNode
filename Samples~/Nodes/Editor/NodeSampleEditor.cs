using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VisualGraphInEditor;
using VisualGraphRuntime;


//使用UIElements自行绘制
[CustomNodeView((typeof(NodeSample)))]
public class NodeSampleEditor : VisualGraphNodeView
{
    //这个特性用于标记是否显示原来的节点inspector
    public override bool ShowNodeProperties => false;
    //大小
    public override Vector2 default_size => new Vector2(200, 200);

    private NodeSample nodeSample;

    public override void InitNode(VisualGraphRuntime.VisualGraphNode graphNode)
    {
        base.InitNode(graphNode);
        nodeSample = this.node as NodeSample;
    }
    public override void DrawNode()
    {
        //需要用UIElement绘制
        FloatField textField = new FloatField("等待时间");
        textField.value = nodeSample.waitDuration;
        textField.style.backgroundColor = new StyleColor(new Color32(55, 55, 55, 255));
        mainContainer.Add(textField);
        textField.RegisterValueChangedCallback((data) => nodeSample.waitDuration = data.newValue);
    }
}



#region 使用IMGUI绘制
/*
 [CustomEditor(typeof(NodeSample))]
 public class NodeSampleEditor2 : Editor
 {
     private void OnEnable()
     {

     }
     public override void OnInspectorGUI()
     {
         base.OnInspectorGUI();
         //gui draw
     }
 }
*/
#endregion

