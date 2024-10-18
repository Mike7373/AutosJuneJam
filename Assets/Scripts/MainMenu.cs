using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class MainMenu : MonoBehaviour
{
   public EventInstance songMenu, yes, no, duality;
   public float eqLow;
   public float gain;
   
    public void Awake()
    {

    }

    public void Start()
    {
        songMenu = AudioManager.instance.CreateEventInstance(FMODEvents.instance.songMenu);
        songMenu.setParameterByName("EQ-Low", eqLow);
        songMenu.setParameterByName("Gain", gain);
        songMenu.start();

        yes = AudioManager.instance.CreateEventInstance(FMODEvents.instance.yes);
        no = AudioManager.instance.CreateEventInstance(FMODEvents.instance.no);
        duality = AudioManager.instance.CreateEventInstance(FMODEvents.instance.duality);
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
