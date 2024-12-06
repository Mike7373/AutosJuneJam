using System.Collections;
using Characters;
using UnityEngine;
using UnityEngine.AI;

public class AthenaNavigation : MonoBehaviour
{
    public Navigable navigable;
    public Vector3 startPosition;
    
    ActionRunner actionRunner;
    Animator animator;
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update

    void Awake()
    {
        actionRunner = GetComponent<ActionRunner>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;

        //animator.SetBool(AnimatorProperties.IsMoving, true);
        //navigationPlayer = GetComponent<NavigationPlayer>();
        //navigationPlayer.enabled = true;
        //navigationPlayer.navigable = navigable.GetComponent<Transform>();
    }
    void Start()
    {
        StartCoroutine(Navigate());
    }

    public void OnDestroy()
    {
        animator.SetBool(AnimatorProperties.IsMoving, false);
    }

    IEnumerator Navigate()
    {
        navMeshAgent.stoppingDistance = navigable.stoppingDistance;
        navMeshAgent.destination = navigable.transform.position;

        animator.SetBool(AnimatorProperties.IsMoving, true);
        yield return WaitForReach();
        navMeshAgent.enabled = false;
        animator.SetBool(AnimatorProperties.IsMoving, false);

        //TODO QUI STATO INTERAZIONE TESTUALE
        yield return new WaitForSeconds(1.0f);

        navMeshAgent.enabled = true;
        navMeshAgent.stoppingDistance = 0;
        navMeshAgent.destination = startPosition;
        animator.SetBool(AnimatorProperties.IsMoving, true);
        yield return WaitForReach();
        animator.SetBool(AnimatorProperties.IsMoving, false);
        transform.position = startPosition; 

        actionRunner.StartAction<AthenaIdle>();
        navMeshAgent.enabled = false;
    }

    IEnumerator WaitForReach()
    {      
        yield return null;

        
        bool condition = (navMeshAgent.remainingDistance<=navMeshAgent.stoppingDistance);
        while (!(condition)) {

            condition = navMeshAgent.remainingDistance<=navMeshAgent.stoppingDistance;
            Debug.Log(navMeshAgent.remainingDistance);
            yield return null;
        }
    }
    IEnumerator WaitDummy()
    {
        
        int i = 1000;
        while (i>=0) {
            i--;
            //Debug.Log(navMeshAgent.remainingDistance);
            //Debug.Log(navMeshAgent.pathStatus);
            yield return null;
        }
    }
}
