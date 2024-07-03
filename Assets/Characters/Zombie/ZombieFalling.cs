using UnityEngine;

public class ZombieFalling : MonoBehaviour
{
    ZombieBehaviour zombie;
    void Start()
    {
        zombie = GetComponent<ZombieBehaviour>();
        
        zombie.rigidBody.useGravity = true;
    }

    void OnDisable()
    {
        zombie.rigidBody.useGravity = false;
        zombie.rigidBody.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (zombie.IsGrounded())
        {
            zombie.StartAction<ZombieIdle>();
        }
        else
        {
            // In volo mi muovo
            Vector2 inputValue = zombie.moveAction.ReadValue();
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            bool speedModifier = zombie.runModifierAction.IsInProgress();
            float speed = speedModifier ? zombie.runSpeed : zombie.speed;
            if (axisDirection != 0)
            {
                Vector3 velocity = speed * axisDirection * zombie.movementAxis;
                zombie.rigidBody.rotation = Quaternion.LookRotation(zombie.movementAxis * axisDirection, Vector3.up);
                zombie.rigidBody.velocity = new Vector3(velocity.x, zombie.rigidBody.velocity.y, velocity.z);
            }
            else
            {
                zombie.rigidBody.velocity = new Vector3(0, zombie.rigidBody.velocity.y, 0);
            }
        }
    }
}
