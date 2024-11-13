using System.Collections;
using Characters.Rigging;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PistolAim : MonoBehaviour
{
    Animator animator;
    PistolRig pistolRig;
    RigBuilder rigBuilder;
    
    public Pistol pistolPrefab;
    public Transform targetToShoot;
    
    static readonly int IsAiming = Animator.StringToHash("IsAiming");

    void Awake()
    {
        animator  = GetComponent<Animator>();
        pistolRig = GetComponent<PistolRig>();
        rigBuilder= GetComponent<RigBuilder>();
    }

    void Start()
    {
        StartCoroutine(WaitAndMira());
    }

    IEnumerator WaitAndMira()
    {
        yield return new WaitForSeconds(2.0f);
        
        Pistol pistol = Instantiate(pistolPrefab, transform);
        pistolRig.Bind(pistol);
        
        animator.SetBool(IsAiming, true);
        
        float elapsed = 0;
        while (elapsed <= 1000.0f)
        {
            pistol.AimTo(targetToShoot.position);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(0.1f);
        
        animator.SetBool(IsAiming, false);
        Destroy(pistol.gameObject);
    }
    
    
}
