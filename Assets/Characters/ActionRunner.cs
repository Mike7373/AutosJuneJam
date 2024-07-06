using System;
using UnityEngine;

namespace Characters
{
public class ActionRunner : MonoBehaviour
{
    [NonSerialized]
    public MonoBehaviour currentBehaviour;
    
    public Component StartAction<T>() where T : MonoBehaviour
    {
        if (currentBehaviour)
        {
            Destroy(currentBehaviour);
        }
        currentBehaviour = gameObject.AddComponent<T>();
        return currentBehaviour;
    }
    
}
}