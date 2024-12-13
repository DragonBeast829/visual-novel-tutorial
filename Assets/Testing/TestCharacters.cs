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
            CharacterModel3D skeleton = CreateCharacter("skeleton") as CharacterModel3D;
            CharacterSprite stickman = CreateCharacter("Stickman") as CharacterSprite;
            CharacterLive2D mao = CreateCharacter("Mao") as CharacterLive2D;

            stickman.SetPosition(Vector2.zero);
            mao.SetPosition(new Vector2(0.5f, 0));
            skeleton.SetPosition(Vector3.right);
            
            yield return new WaitForSeconds(1);
            
            stickman.FaceRight();
            mao.FaceRight();
            skeleton.FaceRight();

            yield return new WaitForSeconds(2f);

            stickman.FaceLeft();
            mao.FaceLeft();
            skeleton.FaceLeft();

            yield return new WaitForSeconds(2f);
        }
    }
}