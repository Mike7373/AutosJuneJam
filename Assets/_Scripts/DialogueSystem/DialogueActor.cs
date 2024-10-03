using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DialogueActor : MonoBehaviour
{
    [HorizontalGroup("ActorData",75)]
    [PreviewField(75)]
    [HideLabel]
    [SerializeField] private Sprite _icon;
    [VerticalGroup("ActorData/Info")]
    [LabelWidth(100)]
    [SerializeField] private string _actorName;
    [VerticalGroup("ActorData/Info")]
    [LabelWidth(100)]
    [SerializeField] private ActorType _actorType;
    [VerticalGroup("ActorData/Info")]
    [LabelWidth(100)]
    [SerializeField] private List<DialogueData> _dialogues;
    public int _dialogueIndex { get; private set; }

    public Sprite GetIcon() { return _icon; }
    public string GetActorName() { return _actorName; }
    
    public void NextDialogue()
    {
        _dialogueIndex++;
    }
}
