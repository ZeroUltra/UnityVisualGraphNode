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
    [BlackboardPropertyType(typeof(Vector2IntBlackboardProperty), "Vector2Int")]
    public class BlackboardVector2IntPropertyView : BlackboardFieldView
    {
        public override void CreateField(BlackboardField field)
        {
            Vector2IntBlackboardProperty localProperty = (Vector2IntBlackboardProperty)property;
            CreatePropertyField<Vector2Int, UnityEditor.UIElements.Vector2IntField>(field, localProperty);
        }
    }
}