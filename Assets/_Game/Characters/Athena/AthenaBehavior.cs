using Characters;
using Input;
using UnityEngine;

/**
 * E' un movimento bidimensionale che avviene lungo una direzione stabilita.
 *
 * La fisica sull'oggetto è abilitata solo nella parte di gestione delle collisioni.
 * La gravità è disabilitata.
 * Materiale fisico e proprietà del RigidBody sono impostate in modo da renderlo quasi del tutto cinematico.
 *
 * NOTA: La gestione del grounded funziona bene solo con un solo collider. Se si aggiungono altri collider, bisogna
 * scegliere quale collider lavora con la gravità, oppure modificarne il comportamento.
 *
 * TODO: RequireComponent(RigidBody) or CharacterController
 * 
 */
[RequireComponent(typeof(CharacterInput), typeof(Animator)),
 RequireComponent(typeof(GroundChecker), typeof(ActionRunner))]
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

    [field: SerializeField] public Pistol    pistolPrefab {get; private set;}
    
    ActionRunner actionRunner;
    
    void Start()
    {
        actionRunner = GetComponent<ActionRunner>();
        actionRunner.StartAction<AthenaIdle>();
    }

}
