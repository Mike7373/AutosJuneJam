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
}
