using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using TMPro;

namespace TESTING {
    public class TestCharacters : MonoBehaviour {
        public TMP_FontAsset font;

        // Start is called before the first frame update
        void Start() {
            StartCoroutine(Test());
        }

        IEnumerator Test() {

            Character Elen = CharacterManager.instance.CreateCharacter("Elen");
            Character Adam = CharacterManager.instance.CreateCharacter("Adam");
            Character Ben = CharacterManager.instance.CreateCharacter("Benjamin");

            List<string> lines = new List<string>() {
                "Hi, there!",
                "My name is Elen.",
                "What's your name?",
                "Oh,{wa 1} that's very nice."
            };

            yield return Elen.Say(lines);

            Elen.SetNameColor(Color.red);
            Elen.SetDialogueColor(Color.green);
            Elen.SetNameFont(font);
            Elen.SetDialogueFont(font);

            yield return Elen.Say(lines);

            Elen.ResetConfigurationData();

            yield return Elen.Say(lines);

            lines = new List<string>() {
                "I am Adam.",
                "More lines{c}Here."
            };

            yield return Adam.Say(lines);

            yield return Ben.Say("This is a line that I want to say.{a} It is a simple line.");

            Debug.Log("Finished");
        }

        IEnumerator XTest() {
            Debug.Log("In test characters");

            Character Foo = CharacterManager.instance.CreateCharacter("Foo");
            Character Bar = CharacterManager.instance.CreateCharacter("Bar");
            Character Placeholder = CharacterManager.instance.CreateCharacter("Placeholder");

            List<string> lines = new List<string>() {
                "Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue",
                "Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue",
                "Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue Dialogue",
                "Dialogue Dialogue Dialogue",
            };
            yield return Foo.Say(lines);

            lines = new List<string>() {
                "Also Dialogue",
                "Dialogue!!!!!",
            };

            yield return Bar.Say(lines);

            yield return Placeholder.Say("I am talking.{a} I am still talking.{wc 1}I am talking");
        }

        // Update is called once per frame
        void Update() {

        }
    }
}