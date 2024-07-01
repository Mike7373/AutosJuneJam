using System.Collections.Generic;
using Characters;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

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
[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator))]
public class AthenaBehavior : MonoBehaviour
{
    public Vector3 movementAxis = Vector3.right;
    public float speed          = 2.0f;
    public float runSpeed       = 5.0f;
    public float jumpRange      = 8;
    public float jumpSpeed      = 3;

    public PlayerInputAction<Vector2> moveAction;
    public PlayerInputAction<float> jumpAction;
    public PlayerInputAction<float> runModifierAction;
    public PlayerInputAction<float> punchAction;

    public Rigidbody rigidBody;
    public Animator animator;
    
    public bool IsGrounded() => groundingColliders.Count > 0;

    // TODO: Mettere questa gestione del grounded in una componente a parte, se continua a restare
    // uguale anche per altre entità.
    HashSet<Collider> groundingColliders = new ();
    CharacterAction currentAction;
    
    public void StartAction(CharacterAction action)
    {
        currentAction?.Stop();
        currentAction = action;
        currentAction.Start();
    }

    void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        moveAction = new PlayerInputAction<Vector2>(playerInput.actions["Move"]);
        jumpAction = new PlayerInputAction<float>(playerInput.actions["Jump"]);
        runModifierAction = new PlayerInputAction<float>(playerInput.actions["RunModifier"]);
        punchAction = new PlayerInputAction<float>(playerInput.actions["Punch"]);
    }
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        runModifierAction.performed += RunModifierPerformed;
        runModifierAction.canceled += RunModifierCancelled;
        
        StartAction(new AthenaIdle(this));
    }
    
    void RunModifierPerformed(float _)
    {
        animator.SetBool(AnimatorProperties.SpeedModifier, true);
    }

    void RunModifierCancelled()
    {
        animator.SetBool(AnimatorProperties.SpeedModifier, false);
    }

    void OnCollisionEnter(Collision c)
    {
        // Se collido con qualcosa e la normale del punto di contatto è 
        // verso l'alto, allora lo consideriamo come atterraggio.
        AthenaMovement.DebugContacts(c, Color.red);
        for (int i = 0; i < c.contactCount; i++)
        {
            if (c.GetContact(i).normal.y > 0)
            {
                groundingColliders.Add(c.collider);
                // TODO: Sporco, fallo da un'altra parte, tipo in un handler quando cambia "isGrounded"
                animator.SetBool(AnimatorProperties.IsGrounded, IsGrounded());
                return;
            }
        }
    }

    void OnCollisionExit(Collision c)
    {
        groundingColliders.Remove(c.collider);
        animator.SetBool(AnimatorProperties.IsGrounded, IsGrounded());
    }

}
