using DIALOGUE;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS {
    public abstract class Character {
        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;

        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        public Character(string name, CharacterConfigData config) {
            this.name = name;
            displayName = name;
            this.config = config;
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        public Coroutine Say(List<string> dialogue) {
            dialogueSystem.ShowSpeakerName(displayName);
                dialogueSystem.ApplySpeakerDataToDialogueContainer(config);
            return dialogueSystem.Say(dialogue);
        }

        public enum CharacterType {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D,
        }
    }
}