using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Test.GUI
{
    public class ChoiceBox : MonoBehaviour
    {
        ChoiceButton prefabB;

        public Action<Choice> choice;
        public UnityEvent<Choice> choice2 = new UnityEvent<Choice>();

        public void Initialize(List<Choice> choices)
        {
            Choice c = new Choice();
            this.choice.Invoke(c);
            this.choice2.Invoke(c);
            
            
            ChoiceButton b = Instantiate(prefabB);
            b.OnChoiceButton += c => this.choice.Invoke(c);
            
        }

    }
}