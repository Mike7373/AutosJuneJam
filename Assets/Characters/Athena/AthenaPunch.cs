using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class AthenaPunch : MonoBehaviour
{
    ActionRunner actionRunner;
    Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        actionRunner = GetComponent<ActionRunner>();
        
        animator.SetTrigger(AnimatorProperties.PunchTrigger);
        animator.SetBool(AnimatorProperties.IsPunching, true);
        StartCoroutine(Punch());
    }

    public void OnDestroy()
    {
        animator.SetBool(AnimatorProperties.IsPunching, false);
    }

    IEnumerator Punch()
    {
        yield return new WaitForSeconds(0.8f);
        actionRunner.StartAction<AthenaIdle>();
    }
}
