using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CHARACTERS {
    [CreateAssetMenu(fileName = "Character Configuration Asset", menuName = "Dialogue System/Character Configuration Asset")]
    public class CharacterConfigSO : ScriptableObject {
        public CharacterConfigData[] characters;

        public CharacterConfigData GetConfig(string characterName, bool safe = true) {
            characterName = characterName.ToLower();

            for (int i = 0; i < characters.Length; i++) {
                CharacterConfigData data = characters[i];

                if (string.Equals(characterName, data.name.ToLower()) || string.Equals(characterName, data.alias.ToLower())) {
                    return safe ? data.Copy() : data;
                }
            }
            return CharacterConfigData.Default;
        }
    }
}