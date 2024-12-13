using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using UnityEngine;

/**
 * Engine per lo svolgimento di un dialogo.
 *
 * Contiene l'interfaccia funzionale per la navigazione di un dialogo.
 *
 * La history contiene le interazioni del dialogo: Sentences e ChoiceDone
 * 
 */
public class DialogueEngine
{
    public class DialogueState
    {
        public Dialogue dialogue;
        public Sentence currentSentence;
        public List<object> history;
        public Dictionary<string, Actor> actorMapping;
    }
    
    public DialogueState dialogueState;
    Dictionary<string, Sentence> currentDialogueSentences;
    
    public DialogueEngine(DialogueState ds)
    {
        dialogueState = ds;
        currentDialogueSentences = ds.dialogue.sentences.ToDictionary(sentence => sentence.sentenceID);
    }

    public DialogueEngine(Dialogue d, Dictionary<string, Actor> actors) 
        : this(new DialogueState {
            dialogue = d,
            currentSentence = d.sentences.FirstOrDefault(null!),
            history = new List<object>(),
            actorMapping = actors
        })
    {
    }
    
    public DialogueState Answer(Choice choice)
    {
        dialogueState.history.Add(dialogueState.currentSentence);
        dialogueState.history.Add(new ChoiceDone
        {
            choiceId = choice.choiceId,
            sentenceId = dialogueState.currentSentence.sentenceID
        });
        
        Sentence nextSentence = null;
        string nextSentenceId = choice.nextSentence;
        if (!string.IsNullOrEmpty(nextSentenceId) && !currentDialogueSentences.TryGetValue(nextSentenceId, out nextSentence))
        {
            Debug.LogError($"SentenceId {nextSentenceId} non trovata nel dialogo {dialogueState.dialogue.title}");
        }
        dialogueState.currentSentence = nextSentence;
        return dialogueState;
    }
    
}