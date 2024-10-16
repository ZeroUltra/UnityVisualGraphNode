///-------------------------------------------------------------------------------------------------
// author: William Barry
// date: 2020
// Copyright (c) Bus Stop Studios.
///-------------------------------------------------------------------------------------------------
using UnityEditor.Experimental.GraphView;

using VisualGraphRuntime;

namespace VisualGraphInEditor
{
	[BlackboardPropertyType(typeof(IntBlackboardProperty), "int")]
	public class BlackboardIntPropertyView : BlackboardFieldView
	{
		public override void CreateField(BlackboardField field)
		{
			IntBlackboardProperty localProperty = (IntBlackboardProperty)property;
			CreatePropertyField<int, UnityEngine.UIElements.IntegerField>(field, localProperty);
		}
	}
}