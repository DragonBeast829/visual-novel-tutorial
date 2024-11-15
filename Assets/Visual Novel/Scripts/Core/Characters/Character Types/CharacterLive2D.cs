using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS {
    public class CharacterLive2D : Character {
        private Animator motionAnimator;
        public CharacterLive2D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab) {
            Debug.Log($"Created Live2D Character: '{name}'");
            motionAnimator = animator.transform.GetChild(0).GetComponentInChildren<Animator>();
        }

        public void SetMotion(string animationName) {
            motionAnimator.Play(animationName);
        }
    }
}