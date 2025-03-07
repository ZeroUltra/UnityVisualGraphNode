///-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------

using UnityEditor.Experimental.GraphView;
using VisualGraphRuntime;
using UnityEngine;

namespace VisualGraphInEditor
{
    [BlackboardPropertyType(typeof(Vector3BlackboardProperty), "Vector3")]
    public class BlackboardVector3PropertyView : BlackboardFieldView
    {
        public override void CreateField(BlackboardField field)
        {
            Vector3BlackboardProperty localProperty = (Vector3BlackboardProperty)property;
            CreatePropertyField<Vector3, UnityEditor.UIElements.Vector3Field>(field, localProperty);
        }
    }
}