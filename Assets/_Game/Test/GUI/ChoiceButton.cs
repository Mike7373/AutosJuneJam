using System;
using UnityEngine;
using UnityEngine.UI;

namespace Test.GUI
{
    public class ChoiceButton : MonoBehaviour
    {
        public Action<Choice> OnChoiceButton;
        public Button button;

        Choice c;

        public void Start()
        {
            button.onClick.AddListener(() => OnChoiceButton.Invoke(c));
            button.onClick.AddListener(() => Debug.Log("blabla"));
            

        }

        public void Initialize(Choice c)
        {
            
        }

    }
}