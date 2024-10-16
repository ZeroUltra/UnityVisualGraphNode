using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class NaughtyInspector : UnityEditor.Editor
    {
        private List<SerializedProperty> _serializedProperties = new();
        private List<FieldInfo> _nonSerializedFields = new();
        private List<PropertyInfo> _nativeProperties = new();
        private List<MethodInfo> _methods;
        private readonly Dictionary<string, SavedBool> _foldouts = new();
        private Object objectWithDefaultValues; // Object to pull non-serialized default values from.
        private Component componentWithDefaultValues; // Component to pull non-serialized default values from.

        private bool _anyNaughtyAttribute;

        private List<SerializedProperty> _nonGroupedSerializedProperties;
        private List<IGrouping<string, SerializedProperty>> _groupedSerializedProperties;
        private List<IGrouping<string, SerializedProperty>> _foldoutSerializedProperties;

        private List<FieldInfo> _nonGroupedNonSerializedFields;
        private List<IGrouping<string, FieldInfo>> _groupedNonSerializedFields;
        private List<IGrouping<string, FieldInfo>> _foldoutNonSerializedFields;

        private List<PropertyInfo> _nonGroupedSerializedNativeProperties;
        private List<PropertyInfo> _groupedSerializedNativeProperties;
        private List<PropertyInfo> _foldoutSerializedNativeProperties;

        private List<PropertyInfo> _nonGroupedNonSerializedNativeProperties;
        private List<PropertyInfo> _groupedNonSerializedNativeProperties;
        private List<PropertyInfo> _foldoutNonSerializedNativeProperties;

        private List<PropertyInfo> _otherNativeProperties;

        protected virtual void OnEnable()
        {
            // Ignore null targets because they will result in SerializedObjectNotCreatableException when accessing the SerializedObject
            if (target == null) return;

            // Unity Editor may execute OnEnable prior to setting isPlaying, so use isPlayingOrWillChangePlaymode instead
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (target is MonoBehaviour monoBehaviour)
                {
                    // Create a temporary game object to pull non-serialized default values from.
                    var temporaryGameObject = Instantiate(monoBehaviour.gameObject); // Instantiate() only copies serialized fields

                    // Disable Duplicated object so that components with ExecuteInEditMode do not run.
                    temporaryGameObject.SetActive(false);

                    // Setup temporary object
                    temporaryGameObject.tag = "EditorOnly";
                    objectWithDefaultValues = temporaryGameObject;
                    objectWithDefaultValues.hideFlags = HideFlags.HideAndDontSave;

                    // Get inspected component type
                    componentWithDefaultValues = temporaryGameObject.GetComponent(target.GetType());
                }
                else if (target is ScriptableObject)
                {
                    // Create temporary ScriptableObject
                    objectWithDefaultValues = CreateInstance(target.GetType());
                }
            }

            GetSerializedProperties(ref _serializedProperties);

            _nonSerializedFields = ReflectionUtility.GetAllFields(
                target, static f => f.GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute), true).Length > 0).ToList();

            _nativeProperties = ReflectionUtility.GetAllProperties(
                target, static p => p.GetCustomAttributes(typeof(ShowNativePropertyAttribute), true).Length > 0).ToList();

            _methods = ReflectionUtility.GetAllMethods(
                target, static m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0).ToList();

            // Check each serialized field for ShowNonSerializedField attributes
            foreach (var property in _serializedProperties)
            {
                if (_nonSerializedFields.Find(x => x.Name == property.name) is { } field)
                {
                    // Try to get path
                    string path = null;
                    if (target is MonoBehaviour monoBehaviour)
                        path = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(monoBehaviour));
                    else if (target is ScriptableObject scriptableObject)
                        path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(scriptableObject));

                    // Build warning message
                    var message = "<color=red>Warning</color>: Ignoring <color=cyan>" + typeof(ShowNonSerializedFieldAttribute) + "</color> on serialized field <color=yellow>" + target.GetType() + "." + field.Name + "</color>";
                    if (path != null)
                        message += " in <a href=\"" + path + "\">" + path + "</a>!";
                    else
                        message += "!";

                    // Print warning message
                    Debug.Log(message);

                    // Remove field from non-serialized field list
                    _nonSerializedFields.Remove(field);
                }
            }

            _anyNaughtyAttribute = _serializedProperties.Any(static p => PropertyUtility.GetAttribute<INaughtyAttribute>(p) != null) ||
                                   _nonSerializedFields.Count != 0 || _nativeProperties.Count != 0 || _methods.Count != 0;

            _nonGroupedSerializedProperties = GetNonGroupedProperties(_serializedProperties);
            _groupedSerializedProperties = GetGroupedProperties(_serializedProperties);
            _foldoutSerializedProperties = GetFoldoutProperties(_serializedProperties);

            _nonGroupedNonSerializedFields = GetNonGroupedProperties(_nonSerializedFields);
            _groupedNonSerializedFields = GetGroupedProperties(_nonSerializedFields);
            _foldoutNonSerializedFields = GetFoldoutProperties(_nonSerializedFields);

            _nonGroupedSerializedNativeProperties = FindMatchingNativeProperties(_nonGroupedSerializedProperties.ConvertAll(static x => x.name));
            _groupedSerializedNativeProperties = FindMatchingNativeProperties(_groupedSerializedProperties.ConvertAll(static x => x.Key));
            _foldoutSerializedNativeProperties = FindMatchingNativeProperties(_foldoutSerializedProperties.ConvertAll(static x => x.Key));

            _nonGroupedNonSerializedNativeProperties = FindMatchingNativeProperties(_nonGroupedNonSerializedFields.ConvertAll(static x => x.Name));
            _groupedNonSerializedNativeProperties = FindMatchingNativeProperties(_groupedNonSerializedFields.ConvertAll(static x => x.Key));
            _foldoutNonSerializedNativeProperties = FindMatchingNativeProperties(_foldoutNonSerializedFields.ConvertAll(static x => x.Key));

            var allExcludedProperties = _nonGroupedSerializedNativeProperties
                .Union(_groupedSerializedNativeProperties)
                .Union(_foldoutSerializedNativeProperties)
                .Union(_nonGroupedNonSerializedNativeProperties)
                .Union(_groupedNonSerializedNativeProperties)
                .Union(_foldoutNonSerializedNativeProperties)
                .ToList();

            _otherNativeProperties = _nativeProperties.Except(allExcludedProperties).ToList();
        }

        protected virtual void OnDisable()
        {
            // Destroy temporary game object with non-serialized default values on disable
            if (objectWithDefaultValues != null) DestroyImmediate(objectWithDefaultValues);
            ReorderableListPropertyDrawer.Instance.ClearCache();
        }

        public override void OnInspectorGUI()
        {
            if (!_anyNaughtyAttribute)
            {
                DrawDefaultInspector();
            }
            else
            {
                DrawSerializedProperties();
                DrawNonSerializedFields();
                DrawNativeProperties();
                DrawButtons();
            }

            // Destroy temporary game object with non-serialized default values after first update of inspector gui
            if (objectWithDefaultValues != null) DestroyImmediate(objectWithDefaultValues);
        }

        protected void GetSerializedProperties(ref List<SerializedProperty> outSerializedProperties)
        {
            outSerializedProperties.Clear();
            using (var iterator = serializedObject.GetIterator())
            {
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        outSerializedProperties.Add(serializedObject.FindProperty(iterator.name));
                    }
                    while (iterator.NextVisible(false));
                }
            }
        }

        protected List<PropertyInfo> FindMatchingNativeProperties(List<string> names)
        {
            List<PropertyInfo> nativeProperties = new(names.Count);
            for (var i = 0; i < names.Count; i++)
            {
                var matchedIndex = _nativeProperties.FindIndex(s => ObjectNames.NicifyVariableName(s.Name) == ObjectNames.NicifyVariableName(names[i]));
                nativeProperties.Add(matchedIndex != -1 ? _nativeProperties[matchedIndex] : null);
            }

            return nativeProperties;
        }

        protected void DrawSerializedProperties()
        {
            serializedObject.Update();

            // Draw non-grouped serialized properties
            for (var i = 0; i < _nonGroupedSerializedProperties.Count; i++)
            {
                var property = _nonGroupedSerializedProperties[i];
                if (property.name.Equals("m_Script", System.StringComparison.Ordinal))
                {
                    using (new EditorGUI.DisabledScope(disabled: true))
                    {
                        EditorGUILayout.PropertyField(property);
                    }
                }
                else
                {
                    var nativeProperty = _nonGroupedSerializedNativeProperties[i];
                    if (nativeProperty != null && EditorApplication.isPlayingOrWillChangePlaymode) // display matching native property if in play mode instead of field
                        NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, nativeProperty, property.name);
                    else
                        NaughtyEditorGUI.PropertyField_Layout(property, includeChildren: true);
                }
            }

            // Draw grouped serialized properties
            for (var i = 0; i < _groupedSerializedProperties.Count; i++)
            {
                var group = _groupedSerializedProperties[i];
                var visibleProperties = group.Where(static p => PropertyUtility.IsVisible(p)).ToArray();
                if (!visibleProperties.Any())
                {
                    continue;
                }

                NaughtyEditorGUI.BeginBoxGroup_Layout(group.Key);
                foreach (var property in visibleProperties)
                {
                    var nativeProperty = _groupedSerializedNativeProperties[i];
                    if (nativeProperty != null && EditorApplication.isPlayingOrWillChangePlaymode) // display matching native property if in play mode instead of field
                        NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, nativeProperty, property.name);
                    else
                        NaughtyEditorGUI.PropertyField_Layout(property, includeChildren: true);
                }

                NaughtyEditorGUI.EndBoxGroup_Layout();
            }

            // Draw foldout serialized properties
            for (var i = 0; i < _foldoutSerializedProperties.Count; i++)
            {
                var group = _foldoutSerializedProperties[i];
                var visibleProperties = group.Where(static p => PropertyUtility.IsVisible(p)).ToArray();
                if (!visibleProperties.Any())
                {
                    continue;
                }

                if (!_foldouts.ContainsKey(group.Key))
                {
                    _foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);
                }

                _foldouts[group.Key].Value = EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true);
                if (_foldouts[group.Key].Value)
                {
                    EditorGUI.indentLevel++;
                    foreach (var property in visibleProperties)
                    {
                        var nativeProperty = _foldoutSerializedNativeProperties[i];
                        if (nativeProperty != null && EditorApplication.isPlayingOrWillChangePlaymode) // display matching native property if in play mode instead of field
                            NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, nativeProperty, property.name);
                        else
                            NaughtyEditorGUI.PropertyField_Layout(property, includeChildren: true);
                    }

                    EditorGUI.indentLevel--;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawNonSerializedStructOrField(object targetObj, FieldInfo field, PropertyInfo nativeProperty)
        {
            // Set defaults for non-serialized component fields
            if (!EditorApplication.isPlayingOrWillChangePlaymode && objectWithDefaultValues && !field.IsLiteral)
            {
                if (targetObj.GetType().IsSubclassOf(typeof(MonoBehaviour)) && componentWithDefaultValues)
                    field.SetValue(targetObj, field.GetValue(componentWithDefaultValues));
                else if (targetObj.GetType().IsSubclassOf(typeof(ScriptableObject)))
                    field.SetValue(targetObj, field.GetValue(objectWithDefaultValues));
            }

            if (field.FieldType is { IsValueType: true, IsPrimitive: false, IsEnum: false } && field.FieldType != typeof(LayerMask))
            {
                var subtarget = field.GetValue(targetObj);

                if (!_foldouts.ContainsKey(field.Name))
                    _foldouts[field.Name] = new SavedBool($"{targetObj.GetHashCode()}.{field.Name}", false);

                _foldouts[field.Name].Value = EditorGUILayout.Foldout(_foldouts[field.Name].Value, ObjectNames.NicifyVariableName(field.Name), true);
                if (_foldouts[field.Name].Value)
                {
                    EditorGUI.indentLevel++;
                    foreach (var subfield in field.FieldType.GetFields())
                    {
                        if (subfield.FieldType is { IsValueType: true, IsPrimitive: false, IsEnum: false } && subfield.FieldType != typeof(LayerMask))
                        {
                            DrawNonSerializedStructOrField(subtarget, subfield, null);
                        }
                        else
                        {
                            EditorGUI.BeginChangeCheck();
                            NaughtyEditorGUI.NonSerializedField_Layout(subtarget, subfield);
                            if (EditorGUI.EndChangeCheck() && targetObj != null)
                                field.SetValue(targetObj, subtarget);
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }
            else if (targetObj is Object targetObject)
            {
                if (nativeProperty != null && EditorApplication.isPlayingOrWillChangePlaymode) // display matching native property if in play mode instead of field
                    NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, nativeProperty, field.Name);
                else
                    NaughtyEditorGUI.NonSerializedField_Layout(targetObject, field);
            }
            else
            {
                NaughtyEditorGUI.NonSerializedField_Layout(targetObj, field);
            }
        }

        protected void DrawNonSerializedFields(bool drawHeader = false)
        {
            if (_nonSerializedFields.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGUIStyle());
                    NaughtyEditorGUI.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
                }

                // Draw non-grouped serialized fields
                for (var i = 0; i < _nonGroupedNonSerializedFields.Count; i++)
                {
                    var field = _nonGroupedNonSerializedFields[i];
                    DrawNonSerializedStructOrField(serializedObject.targetObject, field, _nonGroupedNonSerializedNativeProperties[i]);
                }

                // Draw grouped non-serialized fields
                for (var i = 0; i < _groupedNonSerializedFields.Count; i++)
                {
                    var group = _groupedNonSerializedFields[i];
                    NaughtyEditorGUI.BeginBoxGroup_Layout(group.Key);
                    foreach (var field in group)
                    {
                        DrawNonSerializedStructOrField(serializedObject.targetObject, field, _groupedNonSerializedNativeProperties[i]);
                    }

                    NaughtyEditorGUI.EndBoxGroup_Layout();
                }

                // Draw foldout non-serialized fields
                for (var i = 0; i < _foldoutNonSerializedFields.Count; i++)
                {
                    var group = _foldoutNonSerializedFields[i];
                    if (!_foldouts.ContainsKey(group.Key))
                    {
                        _foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);
                    }

                    _foldouts[group.Key].Value = EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true);
                    if (_foldouts[group.Key].Value)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var field in group)
                        {
                            DrawNonSerializedStructOrField(serializedObject.targetObject, field, _foldoutNonSerializedNativeProperties[i]);
                        }

                        EditorGUI.indentLevel--;
                    }
                }
            }
        }

        protected void DrawNativeProperties(bool drawHeader = false)
        {
            if (_nativeProperties.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Native Properties", GetHeaderGUIStyle());
                    NaughtyEditorGUI.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
                }

                // Don't display native properties that match existing serialized or non serialized fields - these are displayed inline
                foreach (var property in _otherNativeProperties)
                {
                    NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, property);
                }
            }
        }

        protected void DrawButtons(bool drawHeader = false)
        {
            if (_methods != null && _methods.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Buttons", GetHeaderGUIStyle());
                    NaughtyEditorGUI.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
                }

                foreach (var method in _methods)
                {
                    NaughtyEditorGUI.Button(serializedObject.targetObject, method);
                }
            }
        }

        private static List<SerializedProperty> GetNonGroupedProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties.Where(static p => PropertyUtility.GetAttribute<IGroupAttribute>(p) == null).ToList();
        }

        private static List<FieldInfo> GetNonGroupedProperties(IEnumerable<FieldInfo> fieldInfos)
        {
            return fieldInfos.Where(static f => PropertyUtility.GetAttribute<IGroupAttribute>(f) == null).ToList();
        }

        private static List<IGrouping<string, SerializedProperty>> GetGroupedProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties
                .Where(static p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p) != null)
                .GroupBy(static p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p).Name).ToList();
        }

        private static List<IGrouping<string, FieldInfo>> GetGroupedProperties(IEnumerable<FieldInfo> properties)
        {
            return properties
                .Where(static f => PropertyUtility.GetAttribute<BoxGroupAttribute>(f) != null)
                .GroupBy(static f => PropertyUtility.GetAttribute<BoxGroupAttribute>(f).Name).ToList();
        }

        private static List<IGrouping<string, SerializedProperty>> GetFoldoutProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties
                .Where(static p => PropertyUtility.GetAttribute<FoldoutAttribute>(p) != null)
                .GroupBy(static p => PropertyUtility.GetAttribute<FoldoutAttribute>(p).Name).ToList();
        }

        private static List<IGrouping<string, FieldInfo>> GetFoldoutProperties(IEnumerable<FieldInfo> properties)
        {
            return properties
                .Where(static f => PropertyUtility.GetAttribute<FoldoutAttribute>(f) != null)
                .GroupBy(static f => PropertyUtility.GetAttribute<FoldoutAttribute>(f).Name).ToList();
        }

        private static GUIStyle GetHeaderGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.UpperCenter;

            return style;
        }
    }
}
