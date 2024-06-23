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
        StartCoroutine(DelaySceneLoad());
    }
    
    // Quit Game
    public void Quit()
    {
        Application.Quit(); 
        Debug.Log("Il giocatore ha interrotto la partita");
    }

    IEnumerator DelaySceneLoad()
    {
    	yield return new WaitForSeconds(0.5f);
	    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
