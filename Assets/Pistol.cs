using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pistol : MonoBehaviour
{
    [NonSerialized]
    public Transform pistolControl;
    public DecalProjector laserDecal;

    // TODO: Mettere in scriptable object
    public EventReference shootSound;
    public float aimSensibility;
    
    // TODO: Questa roba invece dipende dal materiale di contatto, va in uno scriptable object diverso da quello della pistola
    public DecalProjector prefabBulletDecal;
    public ParticleSystem prefabBulletHitParticles;
    // Fine scriptable object -----------------
    
    // Cose a runtime come l'ammo, non vanno nello scriptable.
    int ammo;
    float lastAngleY;
    // Fine cose a runtime
    
    const float FLOAT_THRESHOLD = 0.001f;                         // TODO: Mettere questa proprietà da qualche parte
    [SerializeField] float pistolMaxAngle   = 5;                  // Angolo massimo delle braccia rispetto al corpo oltre al quale il giocatore ruota (TODO: Vedere dove deve finire questa proprietà)

    public void AimTo(Vector3 target)
    {
        pistolControl.LookAt(target);
        /*float diff = pistolControl.localRotation.eulerAngles.y - transform.parent.localRotation.eulerAngles.y;
        transform.parent.Rotate(new Vector3(0, diff, 0));
        pistolControl.Rotate(new Vector3(0, -diff,0));
        lastAngleY = pistolControl.localRotation.eulerAngles.y;*/
    }
    
    /**
     * Spara lungo la direzione del pistolControl. 
     */
    public void Shoot()
    {
        var shootSoundInstance = RuntimeManager.CreateInstance(shootSound);
        shootSoundInstance.start();
        shootSoundInstance.release();
        
        var pistolRay = new Ray(pistolControl.position, pistolControl.forward);
        if (Physics.Raycast(pistolRay, out var hit))
        {
            // Decal, lo istanzio  sul punto colpito e che guarda verso il punto colpito. L'offset è gestibile 
            // tramite la proprietà "pivot" del decal.
            Instantiate(prefabBulletDecal, hit.point, Quaternion.LookRotation(-hit.normal));
            // Sistema di particelle che guarda verso l'esterno 
            Instantiate(prefabBulletHitParticles, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    void AdjustAim()
    {
        // La pistola può ruotare al massimo di "pistolMaxAngle" gradi sulla Y, se la rotazione eccede allora
        // ruotiamo il personaggio sulla Y. Per stabilire il verso della rotazione consideriamo il precedente angleY.
        float angleY = pistolControl.localRotation.eulerAngles.y;
        var rightBound = pistolMaxAngle;
        var leftBound  = 360-pistolMaxAngle;
        if (angleY > rightBound && angleY < leftBound)
        {
            if (lastAngleY >= 0 && lastAngleY <= pistolMaxAngle + FLOAT_THRESHOLD)
            {
                // ES: Prima stavo a 15° ora sono a 30°, il margine destro è 20°. ruoto il pistolControl di 10° in senso antiorario e il player in senso orario
                var diff = angleY-rightBound;
                pistolControl.Rotate(new Vector3(0, -diff,0));
                transform.parent.Rotate(new Vector3(0, diff, 0));
            }
            else
            {
                // ES: Prima stavo a 350° e ora sono a 340°, ruoto il pistolControl in senso orario e il player in senso antiorario
                var diff = leftBound-angleY;
                pistolControl.Rotate(new Vector3(0, diff,0));
                transform.parent.Rotate(new Vector3(0, -diff, 0));
            }
        }
        lastAngleY = pistolControl.localRotation.eulerAngles.y;
    }

    void Update()
    {
        //Damping Torso
        //Vector3 newForward = Vector3.SmoothDamp(transform.parent.forward)
        //transform.parent.rotation = Quaternion.LookRotation(newForward, Vector3.up);
        
        // Decal Laser
        var pistolRay = new Ray(pistolControl.position, pistolControl.forward);
        if (Physics.Raycast(pistolRay, out var hit))
        {
            Debug.DrawLine(hit.point - Vector3.forward*0.1f, hit.point + Vector3.forward*0.1f);
            Debug.DrawLine(hit.point - Vector3.right*0.1f,   hit.point + Vector3.right*0.1f);
            laserDecal.gameObject.SetActive(true);
            laserDecal.transform.position = hit.point;
            laserDecal.transform.rotation = Quaternion.LookRotation(pistolControl.forward);
        }
        else
        {
            laserDecal.gameObject.SetActive(false);                
        }
    }
    
    
}
