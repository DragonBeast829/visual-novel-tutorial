using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS {
    public class CharacterModel3D : Character {
        public CharacterModel3D(string name, CharacterConfigData config) : base(name, config) {
            Debug.Log($"Created Model3D Character: '{name}'");
        }
    }
}