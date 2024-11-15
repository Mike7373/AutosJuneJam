using UnityEngine;

public class Navigable : MonoBehaviour
{
    NavigationManager navigationManager;

    void Awake()
    {
        navigationManager = FindAnyObjectByType<NavigationManager>(); 
    }

    void OnMouseUp()
    {
         // Invocazione evento
         if(navigationManager.onNavigate!=null){
            navigationManager.onNavigate.Invoke(this);
         }
    }
}
