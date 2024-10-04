using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class LeftDoor : MonoBehaviour
{
    StudioEventEmitter emitter;

    void Awake()
    {
    
    }

    void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.cityNoise2, this.gameObject);
        emitter.Play();
    }
}
