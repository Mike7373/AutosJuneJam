using UnityEngine;

public class Shooter : MonoBehaviour
{
    public enum RigLayers
    {
        Pistol = 0
    }
    
    [field: SerializeField] public Pistol pistolPrefab {get; private set;}
}
