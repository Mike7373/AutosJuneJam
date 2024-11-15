using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
   [field: Header("OST")]
   [field: SerializeField] public EventReference song1 {get; private set;}
   [field: SerializeField] public EventReference song2 {get; private set;}
   [field: SerializeField] public EventReference songMenu {get; private set;}
   [field: SerializeField] public EventReference level1 {get; private set;}


   [field: Header("Player SFX")]
   [field: SerializeField] public EventReference footsteps {get; private set;}
   [field: SerializeField] public EventReference run {get; private set;}
   [field: SerializeField] public EventReference jump {get; private set;}
   [field: SerializeField] public EventReference punch {get; private set;}
   [field: SerializeField] public EventReference shoot
   {get; private set;}

   [field: Header("UI SFX")]
   [field: SerializeField] public EventReference yes {get; private set;}
   [field: SerializeField] public EventReference no {get; private set;}
   [field: SerializeField] public EventReference duality {get; private set;}

   [field: Header("Ambience")]
   [field: SerializeField] public EventReference ambience {get; private set;}
   [field: SerializeField] public EventReference cityNoise {get; private set;}
   [field: SerializeField] public EventReference cityNoise2 {get; private set;}
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one FMOD Events in the Scene");
            //return;
        }
        instance = this;
    }
}
