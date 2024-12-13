using DialogueSystem;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    void Start()
    {
        var dialogueBehaviour = GetComponent<DialogueBehavior>();
        //GetComponent<DialogueBox>().StartDialogue(dialogueBehaviour.dialogue, dialogueBehaviour.actorMapping);
    }
}
