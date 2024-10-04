using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
   [field: Header("OST")]
   [field: SerializeField] public EventReference song1 {get; private set;}
   [field: SerializeField] public EventReference song2 {get; private set;}

   [field: Header("Player SFX")]
   [field: SerializeField] public EventReference footsteps {get; private set;}
   [field: SerializeField] public EventReference jump {get; private set;}

   [field: Header("Ambience")]
   [field: SerializeField] public EventReference ambience {get; private set;}
   
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events in the Scene");
            return;
        }
        instance = this;
    }
}
