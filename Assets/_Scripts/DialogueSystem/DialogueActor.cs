using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(DialogueBrain))]
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
    [SerializeField]
    private string _actorID;
    private static List<DialogueActor> _actorInstances = new();

    private void Awake()
    {
        _actorInstances.Add(this);
    }

    public Sprite GetIcon() { return _icon; }
    public string GetActorName() { return _actorName; }

    public static DialogueActor FindActorByID(string id)
    {
        foreach (DialogueActor actor in _actorInstances)
        {
            if (actor._actorID == id)
            {
                return actor;
            }
        }
        Debug.LogWarning($"[DialogueActor] FindActorByID is returning a null value! ID:{id} can't be found.");
        return null;
    }

    
}
