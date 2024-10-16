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
    [BlackboardPropertyType(typeof(Vector2BlackboardProperty), "Vector2")]
    public class BlackboardVector2PropertyView : BlackboardFieldView
    {
        public override void CreateField(BlackboardField field)
        {
            Vector2BlackboardProperty localProperty = (Vector2BlackboardProperty)property;
            CreatePropertyField<Vector2, UnityEngine.UIElements.Vector2Field>(field, localProperty);
        }
    }
}