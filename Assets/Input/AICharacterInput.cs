using System.Collections.Generic;

namespace Input
{
    
    public class AICharacterInput : CharacterInput
    {
        public Dictionary<string, object> actions = new();

        public override CharacterAction<T> GetAction<T>(string actionName)
        {
            if (actions.TryGetValue(actionName, out var action))
            {
                return (AIInputAction<T>) action;
            }
            else
            {
                var newAction = new AIInputAction<T>();
                actions.Add(actionName, newAction);
                return newAction;
            }
        }
    }
}