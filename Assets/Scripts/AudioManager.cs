using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance { get; private set; }

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
  }

  private void Start()
  {
    InitializeOST(FMODEvents.instance.level1);
  }

  void InitializeOST(EventReference OSTEventReference)
  {
    OSTEventInstance = RuntimeManager.CreateInstance(OSTEventReference);
    OSTEventInstance.start();
  }

  public void SetOST(MusicArea mood)
  {
    OSTEventInstance.setParameterByName("Mood", (float)mood);
  }

  void Cleanup()
  {
    OSTEventInstance.release();
    OSTEventInstance.stop(STOP_MODE.IMMEDIATE);
  }

  void OnDestroy()
  {
    Cleanup();
  }
}
