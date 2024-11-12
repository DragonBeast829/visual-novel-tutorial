using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS {
    public class CharacterModel3D : Character {
        public CharacterModel3D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab) {
            Debug.Log($"Created Model3D Character: '{name}'");
        }
    }
}