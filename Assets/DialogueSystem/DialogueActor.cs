using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(DialogueBrain))]
public class DialogueActor : MonoBehaviour
{
    #region Fields

    [HorizontalGroup("ActorData", 75)] [PreviewField(75)] [HideLabel] [SerializeField]
    private Sprite _icon;

    [VerticalGroup("ActorData/Info")] [LabelWidth(100)] [SerializeField]
    private string _actorName;

    [VerticalGroup("ActorData/Info")] [LabelWidth(100)] [SerializeField]
    private ActorType _actorType;

    [VerticalGroup("ActorData/Info")] [LabelWidth(100)] [SerializeField]
    private string _actorID;

    [VerticalGroup("ActorData/Info")] [LabelWidth(100)] [SerializeField]
    private bool _isPlayer;

    public bool IsPlayer => _isPlayer;
    private static List<DialogueActor> _actorInstances = new();

    #endregion

    #region Properties

    public Sprite Icon => _icon;

    public string ActorName => _actorName;

    public string ActorID => _actorID;

    public static DialogueActor PlayerActor { get; private set; }

    #endregion

    private void Awake()
    {
        _actorInstances.Add(this);
        if (_isPlayer)
        {
            if (PlayerActor == null)
            {
                PlayerActor = this;
            }
            else
            {
                Debug.LogWarning(
                    $"[DialogueActor] -> There are more than one DialogueActor with isPlayer flag setted on true.");
            }
        }
    }

    /// <summary>
    /// Restituisce l'istanza dell'actor a cui appartiene l'id passato come parametro
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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