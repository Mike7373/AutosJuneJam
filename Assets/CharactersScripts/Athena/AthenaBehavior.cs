using System;
using Characters;
using Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
 */
[RequireComponent(typeof(CharacterInput), typeof(Rigidbody),typeof(Animator)),
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

    [field: SerializeField] public PistolV2    pistolPrefab {get; private set;}
    
    Animator animator;
    GroundChecker groundChecker;
    ActionRunner actionRunner;
    CharacterInputAction runModifierAction;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        groundChecker = GetComponent<GroundChecker>();
        actionRunner = GetComponent<ActionRunner>();
        
        var characterInput = GetComponent<CharacterInput>();
        runModifierAction = characterInput.GetAction("RunModifier");
        runModifierAction.performed += RunModifierPerformed;
        runModifierAction.canceled += RunModifierCancelled;
        actionRunner.StartAction<AthenaIdle>();
    }
    

    void FixedUpdate()
    {
        // TODO: Setta l'animator solo quando cambia il valore di "IsGrounded()"
        animator.SetBool(AnimatorProperties.IsGrounded, groundChecker.IsGrounded());
    }

    void OnDestroy()
    {
        runModifierAction.performed -= RunModifierPerformed;
        runModifierAction.canceled -= RunModifierCancelled;
    }
    
    void RunModifierPerformed(object _)
    {
        animator.SetBool(AnimatorProperties.SpeedModifier, true);
    }

    void RunModifierCancelled()
    {
        animator.SetBool(AnimatorProperties.SpeedModifier, false);
    }
    
    
    void OnAnimatorMove()
    {
        // WARNING: Non rimuovere questo metodo, altrimenti il personaggio non si muove più quando
        // ha un rig con un MultiParentConstraint.
        // Serve a segnare come abilitato il root motion.
    }
}
