using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING {
    public class ChoicePanelTesting : MonoBehaviour
    {
        ChoicePanel panel;

        void Start() {
            panel = ChoicePanel.instance;
            StartCoroutine(Running());
        }

        IEnumerator Running() {
            string[] choices = new string[] {
                "This is a choice!",
                "I'm also a choice!",
                "I'm a very long choice. Nice weather we're having, huh? Blah blah blah blah.",
            };

            panel.Show("This is the title!", choices);

            while (panel.isWaitingOnUserChoice) {
                yield return null;
            }

            var decision = panel.lastDecision;

            Debug.Log($"Made choice {decision.answerIndex} '{decision.choices[decision.answerIndex]}'");
        }
    }
}
