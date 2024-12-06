using DialogueSystem;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    void Start()
    {

        var dialogueBehaviour = GetComponent<DialogueBehavior>();
        GetComponent<DialogueBrainV2>().StartDialogue(dialogueBehaviour.dialogue, dialogueBehaviour.actorMapping);
    }
}
