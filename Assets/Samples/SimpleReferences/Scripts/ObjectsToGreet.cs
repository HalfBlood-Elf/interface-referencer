using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterfaceReferencer.Sample
{
    public class ObjectsToGreet : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IGreetable> _simpleReference;
        [SerializeField] private List<InterfaceReference<IGreetable>> _listReference;
        [SerializeField] private InterfaceReference<IGreetable>[] _arrayReference;

        private void Start()
        {
            _simpleReference.Value.Greetings();
            foreach (var objToGreet in _listReference)
            {
                objToGreet.Value.Greetings();
            }
            
            foreach (var objToGreet in _arrayReference)
            {
                objToGreet.Value.Greetings();
            }
        }
    }
}