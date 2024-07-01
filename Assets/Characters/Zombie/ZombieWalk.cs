using UnityEngine;
using UnityEngine.InputSystem;

public class ZombieWalk : MonoBehaviour
{
    Vector3 movementDirection;
    ZombieBehaviour zombie;

    void Start()
    {
        zombie = GetComponent<ZombieBehaviour>();
    }
    
    void FixedUpdate()
    {
        // TRANSAZIONE
        if (!zombie.IsGrounded())
        {
            zombie.StartAction<ZombieFalling>();
            return;
        }
        
        // LAVORO SPORCO
        bool speedModifier = zombie.runModifierAction.IsInProgress();
        float speed = speedModifier ? zombie.runSpeed : zombie.speed;
        Vector2 inputValue = zombie.moveAction.ReadValue();
        int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
        if (axisDirection != 0)
        {
            zombie.rigidBody.rotation = Quaternion.LookRotation(zombie.movementAxis * axisDirection, Vector3.up);
            zombie.rigidBody.velocity = speed * axisDirection * zombie.movementAxis;
        }
        else
        {
            zombie.rigidBody.velocity = Vector3.zero;
        }
    }
    
    void MoveActionOncanceled(InputAction.CallbackContext obj)
    {
        zombie.StartAction<ZombieIdle>();
    }
}
