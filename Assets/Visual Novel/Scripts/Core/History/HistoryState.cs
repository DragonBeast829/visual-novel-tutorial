using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace History {
    [System.Serializable]
    public class HistoryState
    {
        public DialogueData dialogue;
        public List<CharacterData> characters;
        public List<AudioData> audios;
        public List<GraphicData> graphics;

        public static HistoryState Capture() {
            HistoryState state = new HistoryState {
                dialogue   = DialogueData.Capture(),
                characters = CharacterData.Capture(),
                audios     = AudioData.Capture(),
                graphics   = GraphicData.Capture()
            };
            return state;
        }

        public void Load() {
            DialogueData.Apply(dialogue);
            CharacterData.Apply(characters);
            AudioData.Apply(audios);
            GraphicData.Apply(graphics);
        }
    }
}
