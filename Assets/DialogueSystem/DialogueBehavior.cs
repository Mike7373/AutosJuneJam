using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace DialogueSystem
{

[Serializable]
public struct ActorIdMapping
{
    [ReadOnly]
    public string actorId;
    public Actor actor;
}


public class DialogueBehavior : MonoBehaviour
{
    [SerializeField]
    TextAsset dialogueJson;
    
    public Dictionary<string,Actor> actorMapping = new();
    
    [NonSerialized]
    public Dialogue dialogue;
    
    [SerializeField]
    List<ActorIdMapping> editorActorMapping;

    void Start()
    {
        actorMapping = editorActorMapping.ToDictionary(v => v.actorId, v => v.actor);
        dialogue     = JsonUtility.FromJson<Dialogue>(dialogueJson.text);
    }

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (dialogueJson != null)
        {
            Dialogue d = JsonUtility.FromJson<Dialogue>(dialogueJson.text);
            ValidateActorList(FindDialogueActors(d));
        }
    }
    void ValidateActorList(HashSet<string> actors)
    {
        foreach(var aId in actors)
        {
            int idx = editorActorMapping.FindIndex(v => v.actorId == aId);
            if (idx < 0)
            {
                editorActorMapping.Add(new ActorIdMapping{actorId = aId});
            }
        }
        editorActorMapping = editorActorMapping
            .Where(a => actors.Contains(a.actorId))
            .OrderBy(a => a.actorId)
            .ToList();
    }
    static HashSet<string> FindDialogueActors(Dialogue dialogue)
    {
        var ret = new HashSet<string>();
        foreach (var s in dialogue.sentences)
        {
            ret.Add(s.actorID);
        }
        return ret;
    }
    #endif
    
}


}