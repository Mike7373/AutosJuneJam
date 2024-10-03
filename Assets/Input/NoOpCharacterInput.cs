using System;

namespace Input
{
    
    
public class NoOpCharacterInput : CharacterInput
{
    public override InputBinder GetInputBinder(string actionName)
    {
        return new NoOpInputBinder();
    }
}

}