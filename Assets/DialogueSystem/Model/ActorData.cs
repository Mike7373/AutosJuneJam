using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace DialogueSystem
{
    
[CreateAssetMenu(fileName = "Dialogue Actor", menuName = "Dialogues/Actor", order = 1)]    
public class ActorData : ScriptableObject
{
    [FormerlySerializedAs("icon")] [HorizontalGroup("ActorData", 75)] [PreviewField(75)] [HideLabel]
    public Sprite portrait;

    [VerticalGroup("ActorData/Info")] [LabelWidth(100)]
    public string actorName;
    
/*
 [VerticalGroup("ActorData/Info")] [LabelWidth(100)] [SerializeField]
    private string _actorID;
*/
}

}