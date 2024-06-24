using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator))]
public class ZombieMovement : MonoBehaviour
{
    GameObject target;
    Animator animator;
    
    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
