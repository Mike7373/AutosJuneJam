using UnityEngine;

public class PistolV2 : MonoBehaviour
{
    public Transform pistolControl;
    
    float pistolMaxAngle = 5;             // Angolo massimo delle braccia rispetto al corpo oltre al quale il giocatore ruota (TODO: Vedere dove deve finire questa proprietà) 
    float lastAngleY;
    
    const float FLOAT_THRESHOLD = 0.001f;
    
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
    }
    
    
    // ============== GESTIONE FUOCO ===============
    /*
    var pistolRay = new Ray(transform.position, transform.forward);
    if (Physics.Raycast(pistolRay, out hit))
    {
        luceMirino.SetActive(true);
        luceMirino.transform.position = hit.point-ray.direction*0.1f;
        luceMirino.transform.LookAt(hit.point);
    }*/
    
}
