using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBrain : MonoBehaviour
{
    public DialogueBox dialogueBox;

    [Header("Dialogue List")]
    [SerializeField] private List<TextAsset> _dialogues = new();

    private Dialogue _currentDialogue;
    private int _dialogueIndex;
    private bool skip;

    private void Start()
    {
        StartCoroutine(StartDialogue());
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            skip = true;
        }
    }

    public IEnumerator StartDialogue()
    {
        _currentDialogue = LoadCurrentDialogue();
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.ShowArrow();
        for(int i = 0; i < _currentDialogue.dialogue.Count; i++)
        {
            Sentence sentence = _currentDialogue.dialogue[i];
            dialogueBox.actorIcon.sprite = DialogueActor.FindActorByID(sentence.actorID).GetIcon();
            dialogueBox.actorName.text = DialogueActor.FindActorByID(sentence.actorID).GetActorName();
            dialogueBox.dialogueText.text = sentence.text;
            
            //attendo la pressione di un tasto che mette skip a true
            while (!skip)
            {
                yield return null;
            }
            skip = false;
            
            //All'ultima battuta del dialogo nascono l'animazione della freccina
            if (i + 2 >= _currentDialogue.dialogue.Count)
            {
                dialogueBox.HideArrow();
            }
        }
        dialogueBox.gameObject.SetActive(false);
        //Preparo il prossimo dialogo
        _dialogueIndex++;
    }

    private Dialogue LoadCurrentDialogue()
    {
        if (_dialogues.Count > 0)
        {
            Dialogue dialogue = JsonUtility.FromJson<Dialogue>(_dialogues[_dialogueIndex].text);
            return dialogue;
        }
        Debug.LogError("You must assign at least 1 TextAsset to load it!");
        return null;
    }
    
}
