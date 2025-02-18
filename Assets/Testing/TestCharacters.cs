using System.Collections;
using UnityEngine;
using CHARACTERS;
using TMPro;
using Unity.VisualScripting;

namespace TESTING {
    public class TestCharacters : MonoBehaviour {
        public TMP_FontAsset font;
        private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

        // Start is called before the first frame update
        void Start() {

            StartCoroutine(Test());
        }

        IEnumerator Test() {
            Character Monk = CreateCharacter("Monk as Generic");
            
            yield return Monk.Say("Normal dialogue configuration");

            Monk.SetDialogueColor(Color.red);
            Monk.SetNameColor(Color.blue);

            yield return Monk.Say("Customized dialogue here");

            Monk.ResetConfigurationData();

            yield return Monk.Say("I should be back to normal");
        }
    }
}