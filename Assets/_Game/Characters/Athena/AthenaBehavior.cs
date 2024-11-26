using Characters;
using Input;
using UnityEngine;

[RequireComponent(typeof(CharacterInput), typeof(Animator), typeof(ActionRunner))]
public class AthenaBehavior : MonoBehaviour
{
    public enum RigLayers
    {
        Pistol = 0
    }
    
    public Vector3 movementAxis = Vector3.right;
    public float speed          = 2.0f;
    public float runSpeed       = 5.0f;
    public float jumpRange      = 8;
    public float jumpSpeed      = 3;

    [field: SerializeField] public Pistol  pistolPrefab {get; private set;}
    
    void Start()
    {
        GetComponent<ActionRunner>().StartAction<AthenaIdle>();
    }

}
