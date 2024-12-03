using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pistol : MonoBehaviour
{
    [NonSerialized]
    public Transform pistolControl;

    // TODO: Mettere in scriptable object
    
    public DecalProjector prefabBulletDecal;  
    public EventReference shootSound;       
    
    // Fine scriptable object -----------------
    
    // Cose a runtime come l'ammo, non vanno nello scriptable.
    int ammo;
    
    const float FLOAT_THRESHOLD = 0.001f;   // TODO: Mettere questa proprietà da qualche parte
    
    [SerializeField]
    float pistolMaxAngle = 5;               // Angolo massimo delle braccia rispetto al corpo oltre al quale il giocatore ruota (TODO: Vedere dove deve finire questa proprietà)
    [SerializeField]
    float decalOffset = 0.05f;              // TODO: Idem, questo resta qui?
    
    float lastAngleY;
    
    
    public void AimTo(Vector3 target)
    {
        // La pistola può ruotare al massimo di "pistolMaxAngle" gradi sulla Y, se la rotazione eccede allora
        // ruotiamo il personaggio sulla Y. Per stabilire il verso della rotazione consideriamo il precedente angleY.
        pistolControl.LookAt(target);
            
        float angleY = pistolControl.localRotation.eulerAngles.y;
        var rightBound = pistolMaxAngle;
        var leftBound  = 360-pistolMaxAngle;
        if (angleY > rightBound && angleY < leftBound)
        {
            if (lastAngleY >= 0 && lastAngleY <= pistolMaxAngle + FLOAT_THRESHOLD)
            {
                // ES: Prima stavo a 15° ora sono a 30°, il margine destro è 20°. ruoto il pivot di 10° in senso antiorario e il player in senso orario
                var diff = angleY-rightBound;
                pistolControl.Rotate(new Vector3(0, -diff,0));
                transform.parent.Rotate(new Vector3(0, diff, 0));
            }
            else
            {
                // ES: Prima stavo a 350° e ora sono a 340°, ruoto il pivot in senso orario e il player in senso antiorario
                var diff = leftBound-angleY;
                pistolControl.Rotate(new Vector3(0, diff,0));
                transform.parent.Rotate(new Vector3(0, -diff, 0));
            }
        }
        lastAngleY = pistolControl.localRotation.eulerAngles.y;
        
        // TODO: Gestione raycast hit e luce mirino. (Il mirino potrebbe essere un decal unlit oppure
        /*var pistolRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(pistolRay, out hit))
        {
            // Decal
            //luceMirino.SetActive(true);
            //luceMirino.transform.position = hit.point-ray.direction*0.1f;
            //luceMirino.transform.LookAt(hit.point);
        }*/
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
            // Decal, lo istanzio leggermente dietro il punto colpito e che guarda verso il punto 
            Instantiate(prefabBulletDecal, hit.point + decalOffset * hit.normal, Quaternion.LookRotation(-hit.normal));
        }
    }
    
    
}
