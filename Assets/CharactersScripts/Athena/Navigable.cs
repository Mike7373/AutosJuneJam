using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigable : MonoBehaviour
{
    NavigationManager navigationManager;
    // Start is called before the first frame update

    void Awake()
    {
        navigationManager = FindAnyObjectByType<NavigationManager>(); 
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
         //INGRESSO NUOVO STATO PLAYER 
         if(navigationManager.onNavigate!=null){
            navigationManager.onNavigate.Invoke(this);
         }
    }
}
