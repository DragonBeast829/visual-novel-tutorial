using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS {
    public class CharacterLive2D : Character {
        public CharacterLive2D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab) {
            Debug.Log($"Created Live2D Character: '{name}'");
        }
    }
}