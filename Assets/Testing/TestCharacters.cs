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
            CharacterModel3D skeleton = CreateCharacter("Skeleton") as CharacterModel3D;
            CharacterModel3D skeleton2 = CreateCharacter("Skeleton2 as Skeleton") as CharacterModel3D;

            yield return skeleton.MoveToPosition(new Vector2(1, 0));
            yield return skeleton2.MoveToPosition(Vector2.zero);

            skeleton.SetMotion("Jump");

            yield return new WaitForSeconds(1);

            skeleton2.SetMotion("Jump");
        }
    }
}