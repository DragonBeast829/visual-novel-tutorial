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
            // CharacterSprite stickman = CreateCharacter("Stickman") as CharacterSprite;
            CharacterLive2D mao = CreateCharacter("Mao") as CharacterLive2D;

            raelin.SetPosition(Vector2.zero);
            mao.SetPosition(new Vector2(1, 0));

            yield return new WaitForSeconds(1);

            mao.SetMotion("Magic Heart Success");
        }
        
        // Update is called once per frame
        void Update() {

        }
    }
}