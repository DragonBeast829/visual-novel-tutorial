using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

namespace TESTING {
    public class InputPanelTesting : MonoBehaviour
    {
        public InputPanel inputPanel;

        void Start() {
            StartCoroutine(Running());
        }

        IEnumerator Running() {
            Character Stickman = CharacterManager.instance.CreateCharacter("Stickman", revealAfterCreation: true);

            yield return Stickman.Say("Hi! What's your name?");

            inputPanel.Show("What is your name?");

            while (inputPanel.isWaitingOnUserInput) {
                yield return null;
            }

            string characterName = inputPanel.lastInput;

            yield return Stickman.Say($"It's very nice to meet you, {characterName}!");
        }
    }
}
