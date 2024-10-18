using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [Header("Mood")]
    [SerializeField] private MusicArea mood;

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.SetOST(mood);
    }
}
