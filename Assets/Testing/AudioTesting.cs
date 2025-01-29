using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

namespace TESTING {
    public class AudioTesting : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Running());
        }

        Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

        IEnumerator Running() {
            CharacterSprite Stickman = CreateCharacter("Stickman") as CharacterSprite;
            Stickman.Show();

            yield return new WaitForSeconds(0.5f);

            AudioManager.instance.PlaySoundEffect("Audio/SFX/RadioStatic", loop: true);

            yield return Stickman.Say("I'm going to turn off the radio.");

            AudioManager.instance.StopSoundEffect("RadioStatic");

            Stickman.Say("It's off now!");
        }
    }
}

