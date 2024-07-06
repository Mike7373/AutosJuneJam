using System.Collections;
using UnityEngine;

namespace Characters.Zombie
{
    public class ZombiePunch : MonoBehaviour
    {
        ZombieBehaviour zombie;
        
        public void Awake()
        {
            zombie = GetComponent<ZombieBehaviour>();
            zombie.animator.SetBool(AnimatorProperties.IsPunching, true);
            zombie.StartCoroutine(Punch());
        }

        public void OnDestroy()
        {
            zombie.animator.SetBool(AnimatorProperties.IsPunching, false);
        }

        IEnumerator Punch()
        {
            yield return new WaitForSeconds(0.8f);
            zombie.StartAction<ZombieIdle>();
        }
        
    }
}