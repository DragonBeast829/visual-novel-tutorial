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
            // CharacterSprite guard = CreateCharacter("Guard as Generic") as CharacterSprite;
            // CharacterSprite guardRed = CreateCharacter("Guard Red as Generic") as CharacterSprite;
            CharacterSprite raelin = CreateCharacter("Raelin") as CharacterSprite;
            CharacterSprite stickman = CreateCharacter("Stickman") as CharacterSprite;
            raelin.SetPosition(new Vector2(0, 0));
            stickman.SetPosition(new Vector2(1, 0));

            raelin.Animate("Shiver", true);
            yield return raelin.Say("i'm cold");

            stickman.Animate("Hop");
            yield return stickman.Say("no");

            raelin.Animate("Shiver", false);
            yield return raelin.Say("oh ok");
        }
        
        // Update is called once per frame
        void Update() {

        }
    }
}