using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   
   public AudioSource ost;
    public void Awake()
    {
        ost.Play();
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
	    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator DelaySceneLoad(string name, float delay)
    {
    	yield return new WaitForSeconds(delay);
	    SceneManager.LoadScene(name);
    }

}
