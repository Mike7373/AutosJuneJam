using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance { get; private set; }

  private EventInstance OSTEventInstance;

  void Awake()
  {
    if (instance != null)
    {
        Debug.LogWarning("Found more than one Audio Manager in the Scene");
    }
    instance = this;
  }

  void Start()
  {
    InitializeOST(FMODEvents.instance.level1);
  }

  void InitializeOST(EventReference OSTEventReference)
  {
    OSTEventInstance = RuntimeManager.CreateInstance(OSTEventReference);
    //OSTEventInstance.start();
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
  
  /**
   * https://docs.unity3d.com/2022.3/Documentation/Manual/DomainReloading.html
   *
   * Questo metodo serve per far funzionare lo script quando in fase di sviluppo disattiviamo l'opzione:
   *  "Edit->Project Settings->Editor->Enter Play Mode Settings->Reload Domain"
   *
   * Il compito del "Reload Domain" quando si preme play è quello di resettare lo stato di tutti gli script.
   * Se lo disattiviamo i membri statici non vengono più resettati e quando ripremiamo play assumono il valore
   * della precedente run.
   *
   * Con il metodo di sotto, resettiamo lo stato dell'AudioManager in fase di avvio.
   */
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
  static void ResetStaticFieldsOnInit()
  {
    instance = null;
  }
  
}
