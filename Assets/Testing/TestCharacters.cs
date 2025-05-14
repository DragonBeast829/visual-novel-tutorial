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
            CharacterSprite Stickman = CreateCharacter("Stickman") as CharacterSprite;

            Stickman.SetPosition(new Vector2(0, 0));
            Stickman.Show();

            yield return Stickman.Say("Blah blah blah");
            yield return Stickman.Say("Blah blah blah");
            Debug.Log("a");
            Stickman.Animate("Shiver");
            yield return Stickman.Say("Blah blah blah");
            yield return Stickman.Say("Blah blah blah");
        }
    }
}