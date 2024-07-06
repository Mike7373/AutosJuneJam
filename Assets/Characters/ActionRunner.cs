using UnityEngine;

namespace Characters
{
public class ActionRunner : MonoBehaviour
{
    MonoBehaviour currentBehaviour;
    
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