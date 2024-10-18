using System.Collections;
using UnityEngine;

namespace Characters.Zombie
{
    public class ZombiePunch : MonoBehaviour
    {
        Animator animator;
        ActionRunner actionRunner;
        
        public void Awake()
        {
            animator = GetComponent<Animator>();
            actionRunner = GetComponent<ActionRunner>();
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
            actionRunner.StartAction<ZombieIdle>();
        }
        
    }
}