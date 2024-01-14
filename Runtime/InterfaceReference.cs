using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InterfaceReferencer
{
    [Serializable]
    public struct InterfaceReference<T> : ISerializationCallbackReceiver
        where T : class
    {
        [SerializeField] private Object _value;

        [NonSerialized] public T Value;

        public Object Obj
        {
            get => _value;
            set
            {
                _value = value switch
                {
                    GameObject gameObject => gameObject.TryGetComponent(out T component) ? component as Object : null,
                    T component => component as Object,
                    _ => null
                };
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR && !ODIN_INSPECTOR
            if (_value)
                Obj = _value;
#endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Value = _value as T;
        }

        public InterfaceReference(T value)
        {
            this._value = value as Object;
            Value = value;
        }

        public static implicit operator T(InterfaceReference<T> a)
        {
            return a.Value;
        }

        public static implicit operator InterfaceReference<T>(Object obj)
        {
            return new InterfaceReference<T> { Obj = obj };
        }

        public static implicit operator InterfaceReference<T>(T obj)
        {
            return new InterfaceReference<T>(obj);
        }
    }
}