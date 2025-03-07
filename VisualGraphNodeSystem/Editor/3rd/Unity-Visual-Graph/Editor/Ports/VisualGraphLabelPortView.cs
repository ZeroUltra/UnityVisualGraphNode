using UnityEngine.UIElements;
using VisualGraphRuntime;

namespace VisualGraphInEditor
{
    public class VisualGraphLabelPortView : VisualGraphPortView
    {
        public override void CreateView(VisualGraphPort port)
        {
            Label field = new Label(port.Name);
            Add(field);
        }
    }
}
