using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBrainV2 : MonoBehaviour
{
    public readonly UnityEvent<string> AnswerEvent = new();

    DialogueBoxV2 dialogueBox;
    
    Dialogue _currentDialogue;
    Dictionary<string, ActorV2> _actorMapping;
    Dictionary<string, Sentence> _currentDialogueSentences = new();
    Sentence _currentSentence;

    void Start()
    {
        dialogueBox = FindObjectOfType<DialogueBoxV2>();
    }

    void OnEnable()
    {
        AnswerEvent.AddListener(SendAswer);
    }

    void OnDisable()
    {
        AnswerEvent.RemoveListener(SendAswer);
    }

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && _currentSentence.choices.Count <= 1)
        {
            AnswerEvent.Invoke(_currentSentence.choices[0].nextSentence);
        }
    }
    
    public void StartDialogue(Dialogue d, Dictionary<string, ActorV2> actorMapping)
    {
        _currentDialogue = d;
        _actorMapping = actorMapping;
        
        _currentDialogueSentences = _currentDialogue.sentenceList.ToDictionary(sentence => sentence.sentenceID);
        _currentSentence = _currentDialogue.sentenceList[0];
        
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.ShowArrow();
        
        DialogueSetup();
    }

    public void EndDialogue()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    /// <summary>
    /// Carica la frase successiva con le relative risposte. Se non c'è una frase successiva chiude il dialogo.
    /// </summary>
    /// <param name="nextSentence"></param>
    public void SendAswer(string nextSentence)
    {
        if (nextSentence != "")
        {
            _currentSentence = _currentDialogueSentences[nextSentence];
            ChoiceBox.ClearButtons();
            DialogueSetup();
        }
        else
        {
            EndDialogue();
        }
    }

    /// <summary>
    /// Carica dinamicamente i dati del dialogo dentro alla UI
    /// </summary>
    /// <returns></returns>
    private void DialogueSetup()
    {
        
        var playerIcon = _actorMapping.Where(kv => kv.Value._isPlayer)
            .Select(kv => kv.Value.actorDataV2.icon)
            .FirstOrDefault(null!);
        if (playerIcon != null)
        {
            dialogueBox.playerIcon.sprite = playerIcon;
        }
        else
        {
            // TODO: Dialogo senza player ma solo fra due giocatori?
            Debug.LogWarning("Non ho trovato un player fra i partecipanti al dialogo!");
        }
        dialogueBox.characterIcon.sprite = _actorMapping[_currentSentence.actorID].actorDataV2.icon;
        dialogueBox.actorName.text       = _actorMapping[_currentSentence.actorID].actorDataV2.actorName;
        dialogueBox.dialogueText.text    = _currentSentence.text;
        
        if (_currentSentence.choices.Count > 1 || _currentSentence.choices[0].text != "")
        {
            dialogueBox.choiceBox.gameObject.SetActive(true);
            dialogueBox.choiceBox.SpawnButtons(_currentSentence.choices);
        }
        else if (_currentSentence.choices[0].nextSentence == "")
        {
            //All'ultima battuta del dialogo nascondo la freccina
            dialogueBox.choiceBox.gameObject.SetActive(false);
            dialogueBox.HideArrow();
        }
    }
}