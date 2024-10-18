using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MainMenu : MonoBehaviour
{
   public EventInstance songMenu, yes, no, duality;
   public float eqLow;
   public float gain;
   

    public void Start()
    {
        songMenu =  RuntimeManager.CreateInstance(FMODEvents.instance.songMenu);
        songMenu.setParameterByName("EQ-Low", eqLow);
        songMenu.setParameterByName("Gain", gain);
        songMenu.start();

        yes = RuntimeManager.CreateInstance(FMODEvents.instance.yes);
        no = RuntimeManager.CreateInstance(FMODEvents.instance.no);
        duality = RuntimeManager.CreateInstance(FMODEvents.instance.duality);
    }

    public void OnDestroy()
    {
        songMenu.stop(STOP_MODE.IMMEDIATE);

        songMenu.release();
        yes.release();
        no.release();
        duality.release();
    }

    // Load Scene
    public void Play()
    {
        StartCoroutine(DelaySceneLoad(2.0f));
    }

    public void Gallery()
    {
        StartCoroutine(DelaySceneLoad(("Galleria"), 0.5f));  
    }
    
    // Quit Game
    public void Quit()
    {
        Application.Quit(); 
        Debug.Log("Il giocatore ha interrotto la partita");
    }

    IEnumerator DelaySceneLoad(float delay)
    {
    	yield return new WaitForSeconds(delay);
	    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    IEnumerator DelaySceneLoad(string name, float delay)
    {
    	yield return new WaitForSeconds(delay);
	    SceneManager.LoadScene(name);
    }

    public void UIYesSound()
    {
        yes.start();
    }

    public void UINoSound()
    {
        no.start();
    }

    public void UIDualitySound()
    {
        duality.start();
    }
}
