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
            CharacterLive2D natori = CreateCharacter("Natori") as CharacterLive2D;
            // CharacterSprite stickman = CreateCharacter("Stickman") as CharacterSprite;
            CharacterLive2D mao = CreateCharacter("Mao") as CharacterLive2D;

            natori.SetPosition(Vector2.zero);
            mao.SetPosition(new Vector2(1, 0));

            yield return new WaitForSeconds(1);
            mao.SetExpression(5);
            mao.SetMotion("Bounce");
            natori.SetExpression(2);
            natori.SetMotion("Glasses Push");
        }
        
        // Update is called once per frame
        void Update() {

        }
    }
}