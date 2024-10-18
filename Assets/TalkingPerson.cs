using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingPerson : MonoBehaviour
{
    [SerializeField]
    float minSecondiPerAnimazione = 3.3f;
    [SerializeField]
    float maxSecondiPerAnimazione = 5.2f;
    

    [SerializeField]
    bool alwaysIdle = false;
    
    Animator animator;

    static List<string> animations = new List<string> { "Talking", "Talking_1", "Talking_2", "Talking_3", "Idle" };

    void Start()
    {
        animator = GetComponent<Animator>();

        if (!alwaysIdle)
        {
            StartCoroutine(TalkingAnimatorController());
        }
    }

    IEnumerator TalkingAnimatorController()
    {
        while (true)
        {
            animator.SetTrigger(animations[Random.Range(0, animations.Count)]);
            yield return new WaitForSeconds(Random.Range(minSecondiPerAnimazione,maxSecondiPerAnimazione));            
        }
    }

}
