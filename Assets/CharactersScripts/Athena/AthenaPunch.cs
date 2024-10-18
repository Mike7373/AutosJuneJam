using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AthenaPunch : MonoBehaviour
{
    ActionRunner actionRunner;
    Animator animator;
    EventInstance punchSound;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        actionRunner = GetComponent<ActionRunner>();
        
        animator.SetTrigger(AnimatorProperties.PunchTrigger);
        animator.SetBool(AnimatorProperties.IsPunching, true);
        StartCoroutine(Punch());
    }

    void Start()
    {
        punchSound = RuntimeManager.CreateInstance(FMODEvents.instance.punch);
        punchSound.release();
        punchSound.start();    
    }

    public void OnDestroy()
    {
        punchSound.stop(STOP_MODE.ALLOWFADEOUT);
        animator.SetBool(AnimatorProperties.IsPunching, false);
    }

    IEnumerator Punch()
    {
        yield return new WaitForSeconds(0.8f);
        actionRunner.StartAction<AthenaIdle>();
    }
}
