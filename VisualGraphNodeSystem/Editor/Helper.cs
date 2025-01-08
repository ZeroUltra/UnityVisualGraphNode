using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
namespace VisualGraphNodeSystem.Editor
{
    public class Helper
    {

        /// <summary>
        /// 加载ScriptableObject,返回第一个找到的
        /// </summary>
        public static T FindScriptableObject<T>(string extraArgs = null) where T : ScriptableObject
        {
            //FindAssets 返回结果是GUID
            string filter = "t:ScriptableObject" + (extraArgs == null ? "" : (" " + extraArgs));
            var assetGUID = AssetDatabase.FindAssets(filter);

            foreach (var item in assetGUID)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(item);
                var obj = AssetDatabase.LoadMainAssetAtPath(assetPath);

                //找到目标
                if (obj is T)
                {
                    return (obj as T);
                }
            }
            Debug.LogError($"没有加载到该[{typeof(T).Name}]");
            return null;
        }

        #region UIElement Draw
        public static TextField DrawMutliTextField(VisualElement mainContainer, string lableName, string baseInputValue, System.Action<string> onChangeMsg)
        {
            Label label = new Label(lableName);
            mainContainer.Add(label);
            TextField msg = new TextField();
            msg.multiline = true;
            msg.value = baseInputValue;
            msg.style.minHeight = 40f;
            msg.style.whiteSpace = WhiteSpace.Normal;//换行
            mainContainer.Add(msg);
            msg.RegisterValueChangedCallback(data => onChangeMsg(data.newValue));
            return msg;
        }

        public static TextField DrawTextField(VisualElement mainContainer, string lableName, string baseInputValue, System.Action<string> onChangeMsg)
        {
            TextField field = new TextField(lableName);
            field.Q<Label>().style.minWidth = NodeGraphSetting.Instance.LabelWidth;
            field.multiline = true;
            field.value = baseInputValue;
            field.style.minHeight = 20f;
            //msg.style.whiteSpace = WhiteSpace.Normal;//换行
            mainContainer.Add(field);
            field.RegisterValueChangedCallback(data => onChangeMsg(data.newValue));
            return field;
        }

        public static IntegerField DrawIntField(VisualElement mainContainer, string lableName, int baseInputValue, System.Action<int> onChangeMsg)
        {
            IntegerField field = new IntegerField(lableName);
            field.Q<Label>().style.minWidth = NodeGraphSetting.Instance.LabelWidth;
            field.value = baseInputValue;
            field.style.minHeight = 20f;
            //msg.style.whiteSpace = WhiteSpace.Normal;//换行
            mainContainer.Add(field);
            field.RegisterValueChangedCallback(data => onChangeMsg(data.newValue));
            return field;
        }
        public static FloatField DrawFloatField(VisualElement mainContainer, string lableName, float baseInputValue, System.Action<float> onChangeMsg)
        {
            FloatField field = new FloatField(lableName);
            field.value = baseInputValue;
            field.style.minHeight = 20f;
            field.Q<Label>().style.minWidth = NodeGraphSetting.Instance.LabelWidth;
            //msg.style.whiteSpace = WhiteSpace.Normal;//换行
            mainContainer.Add(field);
            field.RegisterValueChangedCallback(data => onChangeMsg(data.newValue));
            return field;
        }
        public static Toggle DrawToggleField(VisualElement mainContainer, string lableName, bool baseInputValue, System.Action<bool> onChange)
        {
            Toggle field = new Toggle(lableName);
            field.value = baseInputValue;
            field.style.minHeight = 20f;
            field.Q<Label>().style.minWidth = NodeGraphSetting.Instance.LabelWidth;
            //msg.style.whiteSpace = WhiteSpace.Normal;//换行
            mainContainer.Add(field);
            field.RegisterValueChangedCallback(data => onChange(data.newValue));
            return field;
        }
        public static ObjectField DrawObjectField<T>(VisualElement mainContainer, string lableName, T t, System.Action<T> onChangeObj) where T : UnityEngine.Object
        {
            ObjectField objectField = new ObjectField(lableName);
            objectField.value = t;
            objectField.labelElement.style.minWidth = 100;
            objectField.objectType = typeof(T);
            objectField.allowSceneObjects = false;
            mainContainer.Add(objectField);
            objectField.RegisterValueChangedCallback(data => onChangeObj((T)data.newValue));
            return objectField;
        }
        public static PopupField<string> DrawPopupItem(VisualElement mainContainer, string lableName, List<string> options, ref string strValue, System.Action<string> onChangeValue)
        {
            int index = 0;
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i] == strValue)
                {
                    index = i;
                    break;
                }
            }

            PopupField<string> popup = new PopupField<string>(lableName, options, index);
            strValue = popup.value;
            popup.labelElement.style.minWidth = 100;
            mainContainer.Add(popup);
            popup.RegisterValueChangedCallback(data =>
            {
                onChangeValue(data.newValue);
            });
            return popup;
        }
        public static Label DrawLabel(VisualElement mainContainer, string lableName, Color? color = null, float minHeight = 20)
        {
            Label label = new Label(lableName);
            label.style.height = minHeight;
            if (color != null)
                label.style.color = (Color)color;
            mainContainer.Add(label);
            return label;
        }
        public static EnumField DrawEnumFidld(VisualElement mainContainer, string lableName, Enum baseEnum, System.Action<Enum> onChangeValue)
        {
            EnumField enumField = new EnumField(lableName, baseEnum);
            enumField.Q<Label>().style.minWidth = NodeGraphSetting.Instance.LabelWidth;
            enumField.RegisterValueChangedCallback((data) =>
            {
                onChangeValue(data.newValue);
            });
            mainContainer.Add(enumField);
            return enumField;
        }
        #endregion
    }
}