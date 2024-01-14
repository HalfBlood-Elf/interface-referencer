using UnityEngine;

namespace InterfaceReferencer.Sample
{
    public class GreetableMonoBehaviour : MonoBehaviour, IGreetable
    {
        public void Greetings()
        {
            Debug.Log("Hi, I'm " + name);
        }
    }
}