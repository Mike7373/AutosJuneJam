using System.Collections;
using UnityEngine;

namespace Characters
{
    
public class AthenaIdle : CharacterAction
{
    AthenaBehavior player;
    IEnumerator idleCoro;
    
    public AthenaIdle(AthenaBehavior p)
    {
        player = p;
    }
    
    public void Start()
    {
        // Qui devo prima registrare gli handlers, perchè la coroutine mi fa un passaggio di stato 
        // nello stesso frame e non capisco perchè, la Stop non deregistra gli handlers
        // Forse con la versione a componenti con Start()  e Update() si risolve.
        player.jumpAction.performed += JumpActionOnperformed;
        player.punchAction.performed += PunchActionOnperformed;
        idleCoro = Idle();
        player.StartCoroutine(idleCoro);
    }

    public void Stop()
    {
        player.jumpAction.performed -= JumpActionOnperformed;
        player.punchAction.performed -= PunchActionOnperformed;
        player.StopCoroutine(idleCoro);
    }
    
    void PunchActionOnperformed(float f)
    {
        player.StartAction(new AthenaPunch(player));
    }

    
    public void JumpActionOnperformed(float f)
    {
        player.StartAction(new AthenaJump(player));
    }

    IEnumerator Idle()
    {
        while (true)
        {
            if (!player.IsGrounded())
            {
                player.StartAction(new AthenaFalling(player));
            }
            else
            {
                if (player.moveAction.IsInProgress())
                {
                    player.StartAction(new AthenaWalk(player));
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}

public class AthenaPunch : CharacterAction
{
    AthenaBehavior player;

    public AthenaPunch(AthenaBehavior p)
    {
        this.player = p;
    }
    
    public void Start()
    {
        player.animator.SetTrigger(AnimatorProperties.PunchTrigger);
        player.animator.SetBool(AnimatorProperties.IsPunching, true);
        player.StartCoroutine(Punch());
    }

    public void Stop()
    {
        player.animator.SetBool(AnimatorProperties.IsPunching, false);
    }

    IEnumerator Punch()
    {
        yield return new WaitForSeconds(0.8f);
        player.StartAction(new AthenaIdle(player));
    }
}

public class AthenaWalk : CharacterAction
{
    AthenaBehavior player;
    IEnumerator walkCoro;
    
    public AthenaWalk(AthenaBehavior p)
    {
        player = p;
    }
    
    public void Start()
    {
        player.moveAction.canceled += MoveActionOncanceled;
        player.jumpAction.performed += JumpActionOnperformed;
        player.punchAction.performed += PunchActionOnperformed;
        walkCoro = Walking();
        player.StartCoroutine(walkCoro);
        player.animator.SetBool(AnimatorProperties.IsMoving, true);
    }

    public void Stop()
    {
        player.rigidBody.velocity = Vector3.zero;
        player.moveAction.canceled -= MoveActionOncanceled;
        player.jumpAction.performed -= JumpActionOnperformed;
        player.punchAction.performed -= PunchActionOnperformed;
        player.StopCoroutine(walkCoro);
        player.animator.SetBool(AnimatorProperties.IsMoving, false);
    }
    
    void PunchActionOnperformed(float f)
    {
        player.StartAction(new AthenaPunch(player));
    }
    
    void JumpActionOnperformed(float f)
    {
        player.StartAction(new AthenaJump(player));
    }
    
    void MoveActionOncanceled()
    {
        player.StartAction(new AthenaIdle(player));
    }
    
    IEnumerator Walking()
    {
        while (true)
        {
            // TRANSAZIONE
            if (!player.IsGrounded())
            {
                player.StartAction(new AthenaFalling(player));
                break;
            }

            // LAVORO
            bool speedModifier = player.runModifierAction.IsInProgress();
            float speed = speedModifier ? player.runSpeed : player.speed;
            Vector2 inputValue = player.moveAction.ReadValue();
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            if (axisDirection != 0)
            {
                player.rigidBody.rotation = Quaternion.LookRotation(player.movementAxis * axisDirection, Vector3.up);
                player.rigidBody.velocity = speed * axisDirection * player.movementAxis;
            }
            else
            {
                player.rigidBody.velocity = Vector3.zero;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}

public class AthenaJump : CharacterAction
{
    AthenaBehavior player;
    IEnumerator jumpCoro;
    IEnumerator jumpMovementCoro;

    public AthenaJump(AthenaBehavior p)
    {
        player = p;
    }

    public void Start()
    {
        player.jumpAction.canceled += EndJumpInput;
        player.animator.SetTrigger(AnimatorProperties.JumpTrigger);
        jumpCoro = JumpCoroutine();
        player.StartCoroutine(jumpCoro);
    }
    
    public void Stop()
    {
        player.jumpAction.canceled -= EndJumpInput;
        player.rigidBody.velocity = Vector3.zero;
        player.StopCoroutine(jumpCoro);
    }

    void EndJumpInput()
    {
        player.StartAction(new AthenaFalling(player));
    }
        
    /**
     * Il salto imposta la velocità direttamente, finchè si tiene premuto il tasto di salto questa velocità viene
     * mantenuta. Se viene rilasciato prima, si cade prima. Serve a fare anche i saltini.
     * TODO: Forse meglio sostituire jumpDistance con jumpTime.
     */
    IEnumerator JumpCoroutine()
    {
        float jumpDistance = 0;
        float lastPos = player.rigidBody.position.y;
        while (jumpDistance < player.jumpRange)
        {
            // In volo mi muovo
            Vector3 velocity = Vector3.zero;
            Vector2 inputValue = player.moveAction.ReadValue();
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            bool speedModifier = player.runModifierAction.IsInProgress();
            float speed = speedModifier ? player.runSpeed : player.speed;
            if (axisDirection != 0)
            {
                player.rigidBody.rotation = Quaternion.LookRotation(player.movementAxis * axisDirection, Vector3.up);
                velocity = speed * axisDirection * player.movementAxis;
            }

            // Finchè sto saltando alzo la mia posizione
            velocity += new Vector3(0, player.jumpSpeed, 0);
            player.rigidBody.velocity = velocity;
            yield return new WaitForFixedUpdate();
            jumpDistance += player.rigidBody.position.y - lastPos;
            lastPos = player.rigidBody.position.y;
        }
        player.StartAction(new AthenaFalling(player));
    }
    
}



/**
 * Quando sto cadendo posso muovermi ma piano, e attendo di atterrare.
 */
public class AthenaFalling : CharacterAction
{
    AthenaBehavior player;
    IEnumerator fallingCoro;
    
    public AthenaFalling(AthenaBehavior p)
    {
        player = p;
    }
    
    public void Start()
    {
        fallingCoro = Falling();
        player.StartCoroutine(fallingCoro);
    }

    public void Stop()
    {
        player.rigidBody.useGravity = false;
        player.rigidBody.velocity = Vector3.zero;
        player.StopCoroutine(fallingCoro);
    }

    IEnumerator Falling()
    {
        player.rigidBody.useGravity = true;
        while (true)
        {
            if (player.IsGrounded())
            {
                player.StartAction(new AthenaIdle(player));
            }
            else
            {
                // In volo mi muovo
                Vector2 inputValue = player.moveAction.ReadValue();
                int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
                bool speedModifier = player.runModifierAction.IsInProgress();
                float speed = speedModifier ? player.runSpeed : player.speed;
                if (axisDirection != 0)
                {
                    Vector3 velocity = speed * axisDirection * player.movementAxis;
                    player.rigidBody.rotation = Quaternion.LookRotation(player.movementAxis * axisDirection, Vector3.up);
                    player.rigidBody.velocity = new Vector3(velocity.x, player.rigidBody.velocity.y, velocity.z);
                }
                else
                {
                    player.rigidBody.velocity = new Vector3(0, player.rigidBody.velocity.y, 0);
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}


    
}