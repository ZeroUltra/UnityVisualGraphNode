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
    [BlackboardPropertyType(typeof(Vector4BlackboardProperty), "Vector4")]
    public class BlackboardVector4PropertyView : BlackboardFieldView
    {
        public override void CreateField(BlackboardField field)
        {
            Vector4BlackboardProperty localProperty = (Vector4BlackboardProperty)property;
            CreatePropertyField<Vector4, UnityEditor.UIElements.Vector4Field>(field, localProperty);
        }
    }
}