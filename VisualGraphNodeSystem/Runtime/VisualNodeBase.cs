using VisualGraphRuntime;
namespace VisualGraphNodeSystem
{
    /// <summary>
    /// Node基类
    /// </summary>
    [CustomNodeStyle("NodeStyle")]
    public abstract class VisualNodeBase : VisualGraphRuntime.VisualGraphNode
    {
        public virtual string ToSerialize()
        {
            return null;
        }
        public virtual void FromSerialize(string str)
        {

        }
    }
}