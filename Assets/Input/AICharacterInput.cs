
namespace Input
{
    
public class AICharacterInput : CharacterInput
{
    public override InputBinder GetInputBinder(string actionName)
    {
        return new AIInputBinder();
    }
}

}