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
            CharacterLive2D rice = CreateCharacter("Rice") as CharacterLive2D;
            CharacterLive2D mao = CreateCharacter("Mao") as CharacterLive2D;
            CharacterLive2D natori = CreateCharacter("Natori") as CharacterLive2D;
            CharacterLive2D koharu = CreateCharacter("Koharu") as CharacterLive2D;
            
            rice.SetPosition(new Vector2(0.3f, 0));
            mao.SetPosition(new Vector2(0.4f, 0));
            natori.SetPosition(new Vector2(0.5f, 0));
            koharu.SetPosition(new Vector2(0.6f, 0));

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters(new string[] { "Koharu", "Natori", "Mao", "Rice" });

            yield return new WaitForSeconds(1);

            rice.SetPriority(5);
        }
    }
}