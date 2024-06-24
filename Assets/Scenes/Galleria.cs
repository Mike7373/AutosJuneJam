using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Galleria : MonoBehaviour
{
   
   public AudioSource ost;
    public void Awake()
    {
        ost.Play();
    }
    // Load Scene
    public void Play()
    {
        StartCoroutine(DelaySceneLoad());
    }

    public void Gallery()
    {
        StartCoroutine(DelaySceneLoad("Galleria"));   
    }
    
    // Quit Game
    public void Quit()
    {
        Application.Quit(); 
        Debug.Log("Il giocatore ha interrotto la partita");
    }

    IEnumerator DelaySceneLoad()
    {
    	yield return new WaitForSeconds(2.0f);
	    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator DelaySceneLoad(string name)
    {
    	yield return new WaitForSeconds(2.0f);
	    SceneManager.LoadScene(name);
    }

}
