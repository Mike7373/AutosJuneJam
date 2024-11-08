using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PistolAim : MonoBehaviour
{
    Animator animator;
    PistolIKBinder pistolIKBinder;
    RigBuilder rigBuilder;
    
    public  Pistol pistolPrefab;
    public Transform targetToShoot;
    
    static readonly int IsAiming = Animator.StringToHash("IsAiming");

    void Awake()
    {
        animator       = GetComponent<Animator>();
        pistolIKBinder = GetComponent<PistolIKBinder>();
        rigBuilder     = GetComponent<RigBuilder>();
    }

    void Start()
    {
        StartCoroutine(WaitAndMira());
    }

    IEnumerator WaitAndMira()
    {
        yield return new WaitForSeconds(2.0f);
        
        Pistol pistol = Instantiate(pistolPrefab, transform);
        pistolIKBinder.BindTo(pistol);
        rigBuilder.Build();
        
        // TwoBoneIKConstraintData data; mmnhh 

        animator.SetBool(IsAiming, true);
        rigBuilder.layers[pistolIKBinder.handRigIndex].active = true;
        rigBuilder.layers[pistolIKBinder.dampRigIndex].active = true;
        
        float elapsed = 0;
        while (elapsed <= 1000.0f)
        {
            pistol.AimTo(targetToShoot.position);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        /*pistol.AimTo(targetToShoot.position);
        yield return new WaitForSeconds(1000);*/

        
        pistol.MettiVia();  // Se lo faccio diventare coroutine e aspetto che mette via?

        yield return new WaitForSeconds(0.1f);
        
        animator.SetBool(IsAiming, false);
        rigBuilder.layers[pistolIKBinder.handRigIndex].active = false;
        rigBuilder.layers[pistolIKBinder.dampRigIndex].active = false;
        
        Destroy(pistol.gameObject);
        
        
    }
    
    
}
