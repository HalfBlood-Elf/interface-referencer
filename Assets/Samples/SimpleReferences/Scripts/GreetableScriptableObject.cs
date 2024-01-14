using UnityEngine;

namespace InterfaceReferencer.Sample
{
    public class GreetableScriptableObject : ScriptableObject, IGreetable
    {
        public void Greetings()
        {
            Debug.Log("Hi, I'm " + name);
        }
    }
}