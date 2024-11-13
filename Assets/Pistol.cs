using System;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [field: SerializeField] public Transform leftHandIK  {get; private set;}
    [field: SerializeField] public Transform rightHandIK {get; private set;}
    [field: SerializeField] public Transform aimHeadIK   {get; private set;}

    [field: SerializeField] public float pistolMaxAngle { get; private set; } = 5.0f;
   
    public Transform phantomPivot;      // Il pivot fantastma attorno al quale ruota la pistola, appartiene al rig del giocatore.

    public Transform pivotAtRest;

    
    float lastAngleY;
    public void AimTo(Vector3 point)
    {
        // ========== GESTIONE ROTAZIONE ============
        // La pistola può ruotare al massimo di "pistolMaxAngle" gradi sulla Y.
        // Se la rotazione eccede, ruotiamo il personaggio sulla Y.
        // Per stabilire il verso della rotazione consideriamo il precedente angleY
        phantomPivot.LookAt(point);
        /*float angleY = phantomPivot.localRotation.eulerAngles.y;
        var rightBound = pistolMaxAngle;
        var leftBound  = 360-pistolMaxAngle;
        if (angleY > rightBound && angleY < leftBound)
        {
            if (lastAngleY >= 0 && lastAngleY <= pistolMaxAngle + 0.001)
            {
                // ES: Prima stavo a 15° ora sono a 30°, il margine destro è 20°. ruoto il pivot di 10° in senso antiorario e il player in senso orario
                var diff = angleY-rightBound;
                phantomPivot.Rotate(new Vector3(0, -diff,0));
                transform.Rotate(new Vector3(0, diff, 0));
            }
            else
            {
                // ES: Prima stavo a 350° e ora sono a 340°, ruoto il pivot in senso orario e il player in senso antiorario
                var diff = leftBound-angleY;
                phantomPivot.Rotate(new Vector3(0, diff,0));
                transform.Rotate(new Vector3(0, -diff, 0));
            }
        }
        lastAngleY = phantomPivot.localRotation.eulerAngles.y;*/
    }

    public void MettiVia()
    {
        phantomPivot.position = pivotAtRest.position;
        phantomPivot.rotation = pivotAtRest.rotation;
    }
    
    
}
