using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBrain : MonoBehaviour
{
    public readonly UnityEvent<string> AnswerEvent = new();

    DialogueBox dialogueBox;
    
    Dialogue currentDialogue;
    Dictionary<string, Actor> actorMapping;
    Dictionary<string, Sentence> currentDialogueSentences = new();
    Sentence currentSentence;

    void Start()
    {
        dialogueBox = FindObjectOfType<DialogueBox>();
    }

    void OnEnable()
    {
        AnswerEvent.AddListener(SendAswer);
    }

    void OnDisable()
    {
        AnswerEvent.RemoveListener(SendAswer);
    }

    public void StartDialogue(Dialogue d, Dictionary<string, Actor> actors)
    {
        currentDialogue = d;
        this.actorMapping = actors;
        
        currentDialogueSentences = currentDialogue.sentences.ToDictionary(sentence => sentence.sentenceID);
        currentSentence = currentDialogue.sentences[0];
        
        dialogueBox.gameObject.SetActive(true);

        DialogueSetup();
    }

    public void EndDialogue()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    /// <summary>
    /// Carica la frase successiva con le relative risposte. Se non c'è una frase successiva chiude il dialogo.
    /// </summary>
    /// <param name="nextSentenceId"></param>
    public void SendAswer(string nextSentenceId)
    {
        Sentence nextSentence = null;
        if (!string.IsNullOrEmpty(nextSentenceId) && !currentDialogueSentences.TryGetValue(nextSentenceId, out nextSentence))
        {
            Debug.LogError($"SentenceId {nextSentenceId} non trovata nel dialogo {currentDialogue.title}");
        }
        if (nextSentence != null)
        {
            currentSentence = nextSentence;
            dialogueBox.choiceBox.ClearButtons();
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
        var playerIcon = actorMapping.Where(kv => kv.Value._isPlayer)
            .Select(kv => kv.Value.actorData.portrait)
            .First();
        
        if (playerIcon == null)
        {
            // TODO: Dialogo senza player ma solo fra due giocatori?
            //       Non ha senso determinare ogni volta il player, se è fisso va determinato nel mapping.
            Debug.LogWarning("Non ho trovato un player fra i partecipanti al dialogo!");
        }
        else
        {
            dialogueBox.leftPortrait.sprite = playerIcon;
        }
        dialogueBox.rightPortrait.sprite = actorMapping[currentSentence.actorID].actorData.portrait;
        dialogueBox.actorName.text       = actorMapping[currentSentence.actorID].actorData.actorName;
        dialogueBox.dialogueText.text    = currentSentence.text;
        
        if (currentSentence.choices.Count > 0)
        {
            dialogueBox.choiceBox.gameObject.SetActive(true);
            dialogueBox.choiceBox.SpawnButtons(currentSentence.choices);
            dialogueBox.ShowArrow();
        }
        else
        {
            //All'ultima battuta del dialogo nascondo la freccina
            dialogueBox.choiceBox.gameObject.SetActive(false);
            dialogueBox.HideArrow();
        }
    }
}