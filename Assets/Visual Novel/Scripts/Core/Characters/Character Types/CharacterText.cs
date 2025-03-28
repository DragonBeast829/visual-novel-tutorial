using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS {
    public class CharacterText : Character {
        public CharacterText(string name, CharacterConfigData config) : base(name, config, prefab: null) {
            Debug.Log($"Created Text Character: '{name}'");
        }
    }
}