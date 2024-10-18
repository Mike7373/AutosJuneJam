using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance { get; private set; }

  private List<EventInstance> eventInstances;

  private List<StudioEventEmitter> eventEmitters;

  private EventInstance ambienceEventInstance;

  private EventInstance OSTEventInstance;

  private void Awake()
  {
    if (instance != null)
    {
        Debug.LogWarning("Found more than one Audio Manager in the Scene");
        //return;
    }
    instance = this;
    //DontDestroyOnLoad(gameObject);

    eventInstances = new List<EventInstance>();
    eventEmitters = new List<StudioEventEmitter>();
  }

  private void Start()
  {
    InitializeAmbience(FMODEvents.instance.ambience);
    InitializeOST(FMODEvents.instance.level1);
  }

  private void InitializeAmbience(EventReference ambienceEventReference)
  {
    ambienceEventInstance = CreateEventInstance(ambienceEventReference);
    //ambienceEventInstance.start();
  }

  private void InitializeOST(EventReference OSTEventReference)
  {
    OSTEventInstance = CreateEventInstance(OSTEventReference);
    OSTEventInstance.start();
  }

  public void SetAmbienceParameter(string parameterName, float parameterValue)
  {
    ambienceEventInstance.setParameterByName(parameterName, parameterValue);
  }

  public void SetOST(MusicArea mood)
  {
    OSTEventInstance.setParameterByName("Mood", (float)mood);
  }
  // one time single sounds
  public void PlayOneShot(EventReference sound, Vector3 worldPos)
  {
    RuntimeManager.PlayOneShot(sound, worldPos);
  }

  // cyclic, conditioned sounds, like footsteps
  public EventInstance CreateEventInstance(EventReference eventReference)
  {
    EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
    eventInstances.Add(eventInstance);
    return eventInstance;
  }

  // sounds dependent on distance
  public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
  {
    StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
    emitter.EventReference = eventReference;
    eventEmitters.Add(emitter);
    return emitter;
  }

  private void Cleanup()
  {
    foreach(EventInstance eventInstance in eventInstances)
    {
      eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
      eventInstance.release();
    }

    foreach(StudioEventEmitter emitter in eventEmitters)
    {
      emitter.Stop();
    }
  }

  private void OnDestroy()
  {
    Cleanup();
  }
}
