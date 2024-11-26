using Characters;
using Input;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Walker), typeof(CharacterController)),
 RequireComponent(typeof(AICharacterInput), typeof(ActionRunner), typeof(SphereCollider)),
 RequireComponent(typeof(Animator))]
public class ZombieBehaviour : MonoBehaviour
{
    void Start()
    {
        GetComponent<ActionRunner>().StartAction<ZombieIdle>();
    }
    
}
