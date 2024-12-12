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

            yield return skeleton.MoveToPosition(new Vector2(0.5f, 0));

            skeleton.SetExpression("Angry", 100);

            yield return new WaitForSeconds(3);

            skeleton.SetExpression("Angry", 0, immediate: true);
            skeleton.SetExpression("Sad", 100, immediate: true);
        }
    }
}