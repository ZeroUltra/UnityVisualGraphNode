using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
namespace VisualGraphNodeSystem.Editor
{
    public class NodeScriptsGenerate : MonoBehaviour
    {
       public const string scriptTemplates = @"using VisualGraphRuntime;
using UnityEngine;
using VisualGraphNodeSystem;
[NodeName(""#SCRIPTNAME#"")]
[NodePortAggregate(NodePortAggregateAttribute.PortAggregate.Single, NodePortAggregateAttribute.PortAggregate.Single)]
public class #SCRIPTNAME# : NodeBase
{

}";
        [MenuItem("Assets/Create/Visual Node C# Script", false, 82)]
        static void CreateMyMonoBehaviourScrtip()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<NodeEndNameEditAction>(), "NewNodeScript.cs",
                                                                    EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D, scriptTemplates);
        }
        #region 废弃
        //private static void CreateScript()
        //{
        //    ProjectWindowUtil.CreateAssetWithContent("NewNodeScript.cs", scriptTemplates, EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D);
        //}

        ///// <summary>
        ///// 给脚本添加标题头
        ///// </summary>
        //class AddFileHeadComment : UnityEditor.AssetModificationProcessor
        //{
        //    /// <summary>
        //    /// 此函数在asset被创建，文件已经生成到磁盘上，生成了.meta文件没有正式创建完成之间调用(我觉得) 和import之前被调用
        //    /// </summary>
        //    /// <param name="newFileMeta">newfilemeta 是由创建文件的path加上.meta组成的</param>
        //    public static void OnWillCreateAsset(string newFileMeta)
        //    {
        //        Debug.Log(newFileMeta);

        //        //把meta去掉
        //        string newFilePath = newFileMeta.Replace(".meta", "");
        //        //得到扩展名
        //        string fileExt = Path.GetExtension(newFilePath);

        //        if (fileExt != ".cs") return;
        //        string realPath = Application.dataPath.Replace("Assets", "") + newFilePath;
        //        string scriptContent = File.ReadAllText(realPath);
        //        Debug.Log(scriptContent.Contains("NodeNase"));
        //        //这里实现自定义的一些规则
        //        scriptContent = scriptContent.Replace("#SCRIPTNAME#", Path.GetFileName(Path.GetFileNameWithoutExtension(newFilePath)));

        //        File.WriteAllText(realPath, scriptContent);
        //        //一定要加这句话 不然 在创建之后点击脚本预览发现还是原来模板效果
        //        //一开始就是没加这句话 所以有bug 这就导致了第二个方法产生
        //        AssetDatabase.ImportAsset(newFilePath);
        //    }
        //}
        #endregion

    }
    class NodeEndNameEditAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            if(resourceFile== NodeScriptsGenerate.scriptTemplates)
            resourceFile = resourceFile.Replace("#SCRIPTNAME#", Path.GetFileNameWithoutExtension(pathName));
            File.WriteAllText(pathName, resourceFile);
            AssetDatabase.ImportAsset(pathName);
        }
    }
}
