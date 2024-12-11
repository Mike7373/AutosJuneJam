using DialogueSystem;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    void Start()
    {
        var dialogueBehaviour = GetComponent<DialogueBehavior>();
        GetComponent<DialogueBrain>().StartDialogue(dialogueBehaviour.dialogue, dialogueBehaviour.actorMapping);
    }
}
