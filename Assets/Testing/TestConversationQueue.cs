using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace TESTING {
    public class TestConversationQueue : MonoBehaviour
    {
        void Start() {
            StartCoroutine(Running());
        }

        IEnumerator Running() {
            List<string> lines = new() {
                "This is line 1 from the original conversation.",
                "This is line 2 from the original conversation.",
                "This is line 3 from the original conversation."
            };

            yield return DialogueSystem.instance.Say(lines);

            DialogueSystem.instance.Hide();
        }

        void Update() {
            List<string> lines = new();
            Conversation conversation = null;

            if (Input.GetKeyDown(KeyCode.Q)) {
                lines = new() {
                    "This is the start of an enqueued conversation.",
                    "We can keep it going!"
                };
                conversation = new(lines);
                DialogueSystem.instance.conversationManager.Enqueue(conversation);
            }

            if (Input.GetKeyDown(KeyCode.W)) {
                lines = new() {
                    "This is an inportant conversation!",
                    "August 26th is international dog day!"
                };
                conversation = new(lines);
                DialogueSystem.instance.conversationManager.EnqueuePriority(conversation);
            }
        }
    }
}
