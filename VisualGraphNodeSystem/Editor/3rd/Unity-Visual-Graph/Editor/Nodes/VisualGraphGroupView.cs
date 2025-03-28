using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using VisualGraphRuntime;

namespace VisualGraphInEditor
{
    public class VisualGraphGroupView : Group
    {
        protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        {
            base.OnElementsAdded(elements);

            VisualGraphGroup group = userData as VisualGraphGroup;
            foreach (var element in elements)
            {
                VisualGraphRuntime.VisualGraphNode node = element.userData as VisualGraphRuntime.VisualGraphNode;
                group.node_guids.Add(node.Guid);
            }
        }

        protected override void OnElementsRemoved(IEnumerable<GraphElement> elements)
        {
            base.OnElementsRemoved(elements);
            //VisualGraphGroup group = userData as VisualGraphGroup;
            //foreach (var element in elements)
            //{
            //    VisualGraphNode node = element.userData as VisualGraphNode;
            //    group.node_guids.Remove(node.guid);
            //}
        }

        protected override void OnGroupRenamed(string oldName, string newName)
        {
            base.OnGroupRenamed(oldName, newName);

            VisualGraphGroup group = userData as VisualGraphGroup;
            group.title = newName;
        }
    }
}