using Sirenix.OdinInspector;
using UnityEngine;

namespace DialogueSystem
{
    
[CreateAssetMenu(fileName = "Dialogue Actor", menuName = "Dialogues/Actor", order = 1)]    
public class ActorDataV2 : ScriptableObject
{
    [HorizontalGroup("ActorData", 75)] [PreviewField(75)] [HideLabel]
    public Sprite icon;

    [VerticalGroup("ActorData/Info")] [LabelWidth(100)]
    public string actorName;
    
/*
 [VerticalGroup("ActorData/Info")] [LabelWidth(100)] [SerializeField]
    private string _actorID;
*/
}

}