using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class Galleria : MonoBehaviour
{
   
   public EventInstance songGalleria, no;
   public float gain;

    public void Awake()
    {

    }

    public void Start()
    {
        songGalleria = AudioManager.instance.CreateEventInstance(FMODEvents.instance.song2);
        no = AudioManager.instance.CreateEventInstance(FMODEvents.instance.no);
        songGalleria.setParameterByName("Gain", gain);
        songGalleria.start();
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
