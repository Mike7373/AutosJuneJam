using System;
using System.Collections;
using Characters;
using Characters.Zombie;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator))]
public class ZombieMovement : MonoBehaviour
{
    public float wanderingDuration;
    public float walkSpeed = 1.0f;
    public float alarmDistance = 5.0f;
    public float punchDistance = 2.0f;
    public float runSpeed = 6.0f;
    public float attackDelay = 0.6f;
    
    GameObject target;
    Animator animator;
    Rigidbody rigidBody;

    Unity.Mathematics.Random rng;

    int movingDirection;
    bool isPunching;
    bool isFollowing;
    AIState state;
    IEnumerator currentAction;
    UnityEvent<GameObject> targetNear = new UnityEvent<GameObject>();

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        
        rng = new Unity.Mathematics.Random((uint)DateTime.Now.ToFileTime());
        target = GameObject.FindWithTag("Player");
        StartCoroutine(TargetDistanceTrigger());
        
        targetNear.AddListener(TargetIsNear);
    }
    

    void TargetIsNear(GameObject target)
    {
        switch (state)
        {
            case AIState.WALKING:
                StopCoroutine(currentAction);
                currentAction = Chase();
                StartCoroutine(currentAction);
                break;
        }
    }

    IEnumerator WalkOrIdle()
    {
        int move = (int) (rng.NextUInt() % 3) - 1;
        if (move == 0)
        {
            yield return Idle(wanderingDuration);
        }
        else
        {
            yield return Walk(wanderingDuration);
        }
    }
    
    
    IEnumerator Walk(float duration)
    {
        state = AIState.WALKING;
        Debug.Log("ZOMBIE-WALK");
        animator.SetBool(AnimatorProperties.IsMoving, movingDirection != 0);
        while (true)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.right * movingDirection, Vector3.up);
            rigidBody.velocity = new Vector3(walkSpeed*movingDirection, rigidBody.velocity.y, rigidBody.velocity.z);
            yield return null;
        }
    }

    IEnumerator Idle(float duration)
    {
        Debug.Log("ZOMBIE-IDLE");
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
        while (true)
        {
            yield return null;
        }
    }
    
    
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        // TODO: DANNEGGIA SE IL GIOCATORE è VICINO.
    }

    IEnumerator Chase()
    {
        Debug.Log("ZOMBIE-CHASE");
        state = AIState.CHASING;
        movingDirection = 1;    // TODO.  
        while (true)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.right * movingDirection, Vector3.up);
            rigidBody.velocity = new Vector3(walkSpeed*movingDirection, rigidBody.velocity.y, rigidBody.velocity.z);
            yield return null;
        }
    }
    
    
    // Trigger per l'evento targetNear.
    IEnumerator TargetDistanceTrigger()
    {
        while (true)
        {
            if (Vector3.Distance(rigidBody.position, target.transform.position) <= alarmDistance)
            {
                targetNear.Invoke(target);
            }
            yield return new WaitForFixedUpdate();
        }
    }

   

    
}
