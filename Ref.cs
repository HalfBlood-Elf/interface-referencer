using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InterfaceReferencer
{
    [Serializable]
    public struct Ref<T> : ISerializationCallbackReceiver
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

        public Ref(T value)
        {
            this._value = value as Object;
            Value = value;
        }

        private bool Validate(Object obj)
        {
            switch (obj)
            {
                case GameObject gameObject:
                {
                    if (!gameObject.TryGetComponent(out T component))
                        return false;

                    _value = component as Object;
                    return true;
                }
                default:
                    return obj is T;
            }
        }

        public static implicit operator T(Ref<T> a)
        {
            return a.Value;
        }

        public static implicit operator Ref<T>(Object obj)
        {
            return new Ref<T> { Obj = obj };
        }

        public static implicit operator Ref<T>(T obj)
        {
            return new Ref<T>(obj);
        }
    }
}