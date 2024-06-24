using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator))]
public class AthenaMovementV2 : MonoBehaviour
{
    public float walkAcceleration = 0.3f;        // Accelerazione/Decelerazione prima di raggiungere la walkSpeed 
    public float walkSpeed = 1.0f;
    public float runAcceleration = 0.4f;        // Accelerazione/Decelerazione prima di raggiungere la runSpeed
    public float runSpeed = 5.0f;
    public float jumpSpeed = 1.0f;
    public float jumpRange = 5.0f;
    
    InputAction moveAction;
    InputAction jumpAction;
    InputAction runModifierAction;
    InputAction punchAction;
    Rigidbody rigidBody;
    Animator animator;

    static readonly int IsMoving = Animator.StringToHash("IsMoving");
    static readonly int SpeedModifier = Animator.StringToHash("SpeedModifier");
    static readonly int JumpTrigger = Animator.StringToHash("Jump");
    static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    static readonly int IsPunching = Animator.StringToHash("IsPunching");
    static readonly int PunchTrigger = Animator.StringToHash("Punch");


    int movingDirection;
    bool speedModifier;
    bool isGrounded;
    bool isJumping;
    bool isPunching;
    

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        moveAction = GetComponent<PlayerInput>().actions["Move"];
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        runModifierAction = GetComponent<PlayerInput>().actions["RunModifier"];
        punchAction = GetComponent<PlayerInput>().actions["Punch"];
        animator = GetComponent<Animator>();
        
        moveAction.performed += MoveActionPerformed;
        moveAction.canceled += MoveActionEnd;
        jumpAction.performed += JumpActionPerformed;
        jumpAction.canceled += EndJumpActionPerformed;
        runModifierAction.performed += RunModifierPerformed;
        runModifierAction.canceled += RunModifierCancelled;
        punchAction.performed += PunchActionPerformed;

        Application.targetFrameRate = 244;
        
    }

    void MoveActionPerformed(InputAction.CallbackContext evt)
    {
        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        movingDirection = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
        animator.SetBool(IsMoving, true);
        StartCoroutine(movementCoroutine());
    }
    void MoveActionEnd(InputAction.CallbackContext obj)
    {
        movingDirection = 0;
        animator.SetBool(IsMoving, false);
    }

    IEnumerator movementCoroutine()
    {
        while (movingDirection != 0)
        {
            if (isPunching)
            {
                yield return null;
                continue;
            }
            
            Camera.main.ScreenToWorldPoint()
            float speed = speedModifier ? runSpeed : walkSpeed;
            transform.rotation = Quaternion.LookRotation(Vector3.right * movingDirection, Vector3.up);
            rigidBody.velocity = new Vector3(speed*movingDirection, rigidBody.velocity.y, rigidBody.velocity.z);
            yield return null;
        }
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
    }
    
    void RunModifierPerformed(InputAction.CallbackContext obj)
    {
        animator.SetBool(SpeedModifier, true);
        speedModifier = true;
    }

    void RunModifierCancelled(InputAction.CallbackContext obj)
    {
        animator.SetBool(SpeedModifier, false);
        speedModifier = false;
    }
    
    void JumpActionPerformed(InputAction.CallbackContext obj)
    {
        if (isPunching) return;
        
        isJumping = true;
        if (isGrounded)
        {
            animator.SetTrigger(JumpTrigger);
            animator.SetBool(IsGrounded, false);
            isGrounded = false;
            StartCoroutine(JumpCoroutine());
        }
    }
    void EndJumpActionPerformed(InputAction.CallbackContext obj)
    {
        isJumping = false;
    }
    
    /**
     * Il salto imposta la velocità direttamente, finchè si tiene premuto il tasto di salto questa velocità viene
     * mantenuta. Se viene rilasciato prima, si cade prima. Serve a fare anche i saltini.
     */
    IEnumerator JumpCoroutine()
    {
        float jumpDistance = 0;
        float lastPos = rigidBody.position.y;
        while (isJumping && jumpDistance < jumpRange)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpSpeed, rigidBody.velocity.z);
            yield return new WaitForFixedUpdate();
            jumpDistance += rigidBody.position.y - lastPos;
        }
    }
    
    
    void PunchActionPerformed(InputAction.CallbackContext obj)
    {
        if (isGrounded && !isPunching)
        {
            isPunching = true;  
            animator.SetBool(IsPunching, isPunching);
            animator.SetTrigger(PunchTrigger);
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
            StartCoroutine(PunchCoroutine());
        }
    }

    IEnumerator PunchCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        isPunching = false;
        animator.SetBool(IsPunching, isPunching);
    }

    void OnCollisionEnter(Collision c)
    {
        DebugContacts(c, Color.red);
        // Se collido con qualcosa e la normale del punto di contatto è 
        // verso l'alto, allora lo consideriamo come atterraggio.
        for (int i = 0; i < c.contactCount; i++)
        {
            if (c.GetContact(i).normal.y > 0)
            {
                animator.SetBool(IsGrounded, true);
                isGrounded = true;
                return;
            }
        }
    }
    

    void DebugContacts(Collision c, Color color)
    {
        for (int i = 0; i < c.contactCount; i++)
        {
            var contact = c.GetContact(i);
            Debug.DrawRay(contact.point, contact.normal, color, 4);
        }
    }

}
