﻿using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InterfaceReferencer
{
    [CustomPropertyDrawer(typeof(InterfaceReference<>))]
    public class RefDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        private bool _initialized;

        private Type FieldType
        {
            get
            {
                Type type = fieldInfo.FieldType;
                if (type.IsArray) return type.GetElementType();
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type)) return type.GenericTypeArguments[0];
                return type;
            }
        }

        private void Init()
        {
            if (_initialized)
                return;


            Type type = FieldType;
            if (!type.IsGenericType)
                throw new ArgumentException("Type must be generic");

            if (type.GenericTypeArguments.Length != 1)
                throw new ArgumentException("Type must have only 1 generic parameter");

            if (!type.GenericTypeArguments[0].IsClass && !type.GenericTypeArguments[0].IsInterface)
                throw new ArgumentException("Generic parameter of a type must be class or interface");

            _initialized = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init();
            property.Next(true);
            SerializedProperty obj = property.Copy();
            EditorGUI.BeginProperty(position, label, obj);
            {
                EditorGUI.BeginChangeCheck();
                Object newValue = EditorGUI.ObjectField(position, label, obj.objectReferenceValue,
                    FieldType.GenericTypeArguments[0],
                    property.serializedObject.targetObject is Component);

                if (EditorGUI.EndChangeCheck() && (!newValue || Validate(newValue)))
                {
                    obj.objectReferenceValue = newValue;
                }
            }
            EditorGUI.EndProperty();
        }

        private bool Validate(Object obj)
        { 
            return FieldType.GenericTypeArguments[0].IsInstanceOfType(obj);
        }
    }
}