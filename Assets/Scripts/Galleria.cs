using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class Galleria : MonoBehaviour
{
   
   public EventInstance songGalleria, no;
   public float gain;
   
    void Start()
    {
        songGalleria = RuntimeManager.CreateInstance(FMODEvents.instance.song2);
        no =  RuntimeManager.CreateInstance(FMODEvents.instance.no);
        songGalleria.setParameterByName("Gain", gain);
        songGalleria.start();
    }

    void OnDestroy()
    {
        songGalleria.stop(STOP_MODE.IMMEDIATE);
        songGalleria.release();
        
        no.stop(STOP_MODE.IMMEDIATE);
        no.release();
    }

    // Load Scene
    public void BackToMain()
    {
        StartCoroutine(DelaySceneLoad(("MainMenu"), 0.5f));
    }

    IEnumerator DelaySceneLoad(float delay)
    {
    	yield return new WaitForSeconds(delay);
	    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator DelaySceneLoad(string name, float delay)
    {
    	yield return new WaitForSeconds(delay);
	    SceneManager.LoadScene(name);
    }

    public void UINoSound()
    {
        no.start();
    }
}
