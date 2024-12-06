using UnityEngine;

public class Navigable : MonoBehaviour
{
    NavigationManager navigationManager;
    public float stoppingDistance = 1.4f;

    void Awake()
    {
        navigationManager = FindAnyObjectByType<NavigationManager>(); 
    }
    void OnMouseUp()
    {
        //INGRESSO NUOVO STATO PLAYER 
        if(navigationManager.onNavigate!=null){
            navigationManager.onNavigate.Invoke(this);
        }
    }
}