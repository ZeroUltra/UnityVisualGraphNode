//-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------
using UnityEditor.Experimental.GraphView;

using VisualGraphRuntime;

namespace VisualGraphInEditor
{
	[BlackboardPropertyType(typeof(FloatBlackboardProperty), "float")]
	public class BlackboardFloatPropertyView : BlackboardFieldView
	{
		public override void CreateField(BlackboardField field)
		{
			FloatBlackboardProperty localProperty = (FloatBlackboardProperty)property;
			CreatePropertyField<float, UnityEditor.UIElements.FloatField>(field, localProperty);
		}
	}
}