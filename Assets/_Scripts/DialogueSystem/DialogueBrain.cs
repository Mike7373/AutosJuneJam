using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class DialogueBrain : MonoBehaviour
{
    public DialogueBox dialogueBox;

    [Header("Dialogue List")] [SerializeField]
    private List<TextAsset> _dialogues = new();

    //I seguenti campi sono gestiti da ogni istanza ma solo uno per volta
    //(se c'è un modo migliore beh... please tell me your secrets o.o)
    private static Dictionary<string, Sentence> _currentDialogue = new();
    private static bool skip;
    private static int _dialogueIndex;
    private static Sentence _currentSentence;
    public static UnityEvent<string> AnswerEvent = new();
    private static bool _endDialogue;

    private void Start()
    {
        AnswerEvent.AddListener(SendAswer);
        StartCoroutine(StartDialogue());
    }

    private void OnDestroy()
    {
        AnswerEvent.RemoveListener(SendAswer);
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && _currentSentence.choices.Count <= 1)
        {
            SendAswer(_currentSentence.choices[0].nextSentence);
        }
    }

    public IEnumerator StartDialogue()
    {
        _endDialogue = false;
        List<Sentence> dialogue = LoadCurrentDialogue();
        _currentDialogue = dialogue.ToDictionary(sentence => sentence.sentenceID);
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.ShowArrow();

        _currentSentence =
            _currentDialogue["0"]; //REGOLA: la prima frase di ogni dialogo deve avere "0" come sentenceID

        ChoiceBox choicebox = dialogueBox._choiceBox.GetComponent<ChoiceBox>();
        do
        {
            choicebox.gameObject.SetActive(false);
            dialogueBox.actorIcon.sprite = DialogueActor.FindActorByID(_currentSentence.actorID).GetIcon();
            dialogueBox.actorName.text = DialogueActor.FindActorByID(_currentSentence.actorID).GetActorName();
            dialogueBox.dialogueText.text = _currentSentence.text;
            //All'ultima battuta del dialogo nascono la freccina
            if (_currentSentence.choices.Count > 1)
            {
                choicebox.gameObject.SetActive(true);
                choicebox.SpawnButtons(_currentSentence.choices);
            }else if (_currentSentence.choices[0].nextSentence == "")
            {
                dialogueBox.HideArrow();
            }

            //attendo la pressione di un tasto che mette skip a true
            while (!skip)
            {
                yield return null;
            }
            skip = false;
        } while (!_endDialogue);

        dialogueBox.gameObject.SetActive(false);
        //Prepara il prossimo dialogo
        _dialogueIndex++;
    }

    private List<Sentence> LoadCurrentDialogue()
    {
        if (_dialogues.Count > 0)
        {
            List<Sentence> dialogue = JsonUtility.FromJson<Dialogue>(_dialogues[_dialogueIndex].text).sentenceList;
            return dialogue;
        }

        Debug.LogError("You must assign at least 1 TextAsset to load it!");
        return null;
    }

    public static void SendAswer(string nextSentence)
    {
        if (nextSentence != "")
        {
            _currentSentence = _currentDialogue[nextSentence];
        }
        else
        {
            _endDialogue = true;
        }
        skip = true;
    }

    public static string GetAnswer(string choiceText)
    {
        List<Choice> choices = _currentSentence.choices;
        foreach (Choice choice in choices)
        {
            if (choiceText == choice.text)
            {
                return choice.nextSentence;
            }
        }
        return null;
    }
}