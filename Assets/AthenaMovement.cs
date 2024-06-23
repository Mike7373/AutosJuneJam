using System;
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

    bool isJumping = false;
    float jumpDistance = 0;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        moveAction = GetComponent<PlayerInput>().actions["Move"];
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        
        moveAction.performed += OnMove;
        moveAction.canceled += EndMove;
        jumpAction.performed += OnJump;
        jumpAction.canceled += EndJump;
        
    }

    void EndJump(InputAction.CallbackContext obj)
    {
        isJumping = false;
    }

    void OnJump(InputAction.CallbackContext obj)
    {
        if (isJumping) return;
        
        isJumping = true;
        StartCoroutine(Jump());
    }

    /**
     * Il salto imposta la velocità direttamente, finchè si tiene premuto il tasto di salto questa velocità viene
     * mantenuta. Se viene rilasciato prima, si cade prima. Serve a fare anche i saltini.
     */
    IEnumerator Jump()
    {
        float jumpDistance = 0;
        float lastPos = rigidBody.position.y;
        while (isJumping && jumpDistance < jumpRange)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpSpeed, rigidBody.velocity.z);
            yield return new WaitForFixedUpdate();
            jumpDistance += rigidBody.position.y - lastPos;
        }
        yield return null;
    }

    void EndMove(InputAction.CallbackContext obj)
    {
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
    }

    void OnMove(InputAction.CallbackContext obj)
    {
        Vector2 inputValue = obj.ReadValue<Vector2>();
        int v = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
        rigidBody.velocity = new Vector3(walkSpeed*v, rigidBody.velocity.y, rigidBody.velocity.z);
    }

}
