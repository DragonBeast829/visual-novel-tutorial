using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace TESTING {
    public class TestDialogueFiles : MonoBehaviour {

        [SerializeField] private TextAsset fileToRead = null;

        void Start() {
            StartConversation();
        }

        void StartConversation() {
            List<string> lines = FileManager.ReadTextAsset(fileToRead, false);

            DialogueSystem.instance.Say(lines);
        }

    }
}
