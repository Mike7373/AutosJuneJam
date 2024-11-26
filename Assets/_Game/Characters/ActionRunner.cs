using System;
using UnityEngine;

namespace Characters
{
    

/*
 * Inizia e muove la macchina a stati dei behaviors.
 *
 * TODO: Potrei assegnarle il campo: "InitialState", e.g.: ZombieIdle, AthenaIdle ecc.. Se gli stati riescono a prendere
 * i dati non dall'AthenaBehavior di ora ma da delle componenti di "Dati" allora è più semplice dare adun oggetto può avere comportamenti diversi.
 *   
 */
public class ActionRunner : MonoBehaviour
{
    [NonSerialized]
    public MonoBehaviour currentBehaviour;

    public Component StartAction<T>() where T : MonoBehaviour
    {
        if (currentBehaviour)
        {
            Destroy(currentBehaviour);
        }
        currentBehaviour = gameObject.AddComponent<T>();
        return currentBehaviour;
    }
    
}
}