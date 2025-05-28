using UnityEngine;
using UnityEngine.UIElements;
using VisualGraphInEditor;

namespace VisualGraphNodeSystem.Test
{
    //使用UIElements自行绘制
    [CustomNodeView((typeof(NodeWait)))]
    public class NodeWaitEditor : VisualGraphNodeView
    {
        //这个特性用于标记是否显示原来的节点inspector
        public override bool ShowNodeProperties => false;
        //大小
        public override Vector2 default_size => new Vector2(200, 200);

        private NodeWait nodeSample;

        public override void InitNode(VisualGraphRuntime.VisualGraphNode graphNode)
        {
            base.InitNode(graphNode);
            nodeSample = this.Node as NodeWait;
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

}