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
    [BlackboardPropertyType(typeof(RectBlackboardProperty), "Rect")]
    public class BlackboardRectPropertyView : BlackboardFieldView
    {
        public override void CreateField(BlackboardField field)
		{
            RectBlackboardProperty localProperty = (RectBlackboardProperty)property;
            CreatePropertyField<Rect, UnityEditor.UIElements.RectField>(field, localProperty);
		}
    }
}