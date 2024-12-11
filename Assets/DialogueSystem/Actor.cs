using UnityEngine;
using UnityEngine.Serialization;

namespace DialogueSystem
{
    
public class Actor : MonoBehaviour
{
    [FormerlySerializedAs("actorDataV2")] public ActorData actorData;
    
    public bool _isPlayer;
}

}