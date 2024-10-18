using Characters;
using Input;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Animator),typeof(GroundChecker)),
 RequireComponent(typeof(CharacterInput), typeof(ActionRunner))]
public class ZombieBehaviour : MonoBehaviour
{
    public float speed = 1.0f;
    public float runSpeed = 6.0f;
    public Vector3 movementAxis = Vector3.right;

    void Start()
    {
        GetComponent<ActionRunner>().StartAction<ZombieIdle>();
    }
    
}
