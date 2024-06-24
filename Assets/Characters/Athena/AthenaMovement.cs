using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(Rigidbody))]
public class AthenaMovement : MonoBehaviour
{
    public float walkAcceleration = 0.3f;        // Accelerazione/Decelerazione prima di raggiungere la walkSpeed 
    public float walkSpeed = 1.0f;
    public float runAcceleration = 0.4f;        // Accelerazione/Decelerazione prima di raggiungere la runSpeed
    public float runSpeed = 5.0f;
    public float jumpSpeed = 1.0f;
    public float jumpRange = 5.0f;
    
    InputAction moveAction;
    InputAction jumpAction;
    Rigidbody rigidBody;
    Animator animator;

    AthenaState state;

    static readonly int WalkTrigger = Animator.StringToHash("walk");
    static readonly int IdleTrigger = Animator.StringToHash("idle");
    static readonly int JumpTrigger = Animator.StringToHash("jump");
    static readonly int IsWalking = Animator.StringToHash("isWalking");
    static readonly int RunTrigger = Animator.StringToHash("Run");

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        moveAction = GetComponent<PlayerInput>().actions["Move"];
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        animator = GetComponent<Animator>();
        
        moveAction.performed += WalkActionPerformed;
        moveAction.canceled += WalkActionEnd;
        jumpAction.performed += JumpActionPerformed;
        jumpAction.canceled += EndJumpActionPerformed;

        Application.targetFrameRate = 244;
    }

    void Walk()
    {
        switch (state)
        {
            case AthenaState.IDLE:
                state = AthenaState.WALKING;
                animator.SetTrigger(WalkTrigger);
                break;
            case AthenaState.RUNNING:
                state = AthenaState.WALKING;
                animator.SetTrigger(WalkTrigger);
                break;
        }
    }

    void EndWalk()
    {
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
        switch (state)
        {
            case AthenaState.WALKING:
                state = AthenaState.IDLE;
                animator.SetTrigger(IdleTrigger);
                break;
        }
    }

    void Jump()
    {
        switch (state)
        {
            case AthenaState.IDLE:
            case AthenaState.WALKING:
            case AthenaState.RUNNING:
                state = AthenaState.JUMPING;
                animator.SetTrigger(JumpTrigger);
                StartCoroutine(ManageJump());
                break;
        }
    }

    void EndJump()
    {
        switch (state)
        {
            case AthenaState.JUMPING:
                state = AthenaState.FALLING;
                break;
        }
    }

    void Grounded()
    {
        switch (state)
        {
            case AthenaState.FALLING:
                if (moveAction.IsInProgress())
                {
                    //state = Ath
                    
                }
                else
                {
                    state = AthenaState.IDLE;
                    animator.SetTrigger(IdleTrigger);
                }
                break;
        }
    }

    void WalkActionPerformed(InputAction.CallbackContext evt)
    {
        Walk();
    }
    void WalkActionEnd(InputAction.CallbackContext obj)
    {
        EndWalk();
    }
    void JumpActionPerformed(InputAction.CallbackContext obj)
    {
        Jump();
    }
    void EndJumpActionPerformed(InputAction.CallbackContext obj)
    {
        EndJump();
    }

    /**
     * Il salto imposta la velocità direttamente, finchè si tiene premuto il tasto di salto questa velocità viene
     * mantenuta. Se viene rilasciato prima, si cade prima. Serve a fare anche i saltini.
     */
    IEnumerator ManageJump()
    {
        float jumpDistance = 0;
        float lastPos = rigidBody.position.y;
        while (state == AthenaState.JUMPING && jumpDistance < jumpRange)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpSpeed, rigidBody.velocity.z);
            yield return new WaitForFixedUpdate();
            jumpDistance += rigidBody.position.y - lastPos;
        }
        EndJump();
    }
    

    void Update()
    {
        if (moveAction.IsInProgress())
        {
            Vector2 inputValue = moveAction.ReadValue<Vector2>();
            int v = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            transform.rotation = Quaternion.LookRotation(Vector3.right * v, Vector3.up);
            rigidBody.velocity = new Vector3(walkSpeed*v, rigidBody.velocity.y, rigidBody.velocity.z);
        }
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
                Grounded();
                return;
            }
        }
    }
    

    void DebugContacts(Collision c, Color color)
    {
        for (int i = 0; i < c.contactCount; i++)
        {
            Debug.Log("Disegno contatto!");
            var contact = c.GetContact(i);
            Debug.DrawRay(contact.point, contact.normal, color, 4);
        }
    }

}
