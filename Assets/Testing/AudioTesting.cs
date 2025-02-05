using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

namespace TESTING {
    public class AudioTesting : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Running());
        }

        Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

        IEnumerator Running() {
            yield return new WaitForSeconds(1);

            CharacterSprite Stickman = CreateCharacter("Stickman") as CharacterSprite;
            Stickman.Show();

            yield return Stickman.Say("Let's go to the beach.");

            GraphicPanelManager.instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/BG Images/BG Beach");
            AudioManager.instance.PlayTrack("Audio/Music/Calm", volumeCap: 0.5f);

            Stickman.SetSprite(Stickman.GetSprite("2"), 0);
            Stickman.SetSprite(Stickman.GetSprite("Happy 2"), 1);
            Stickman.MoveToPosition(new Vector2(0.7f, 0), speed: 0.5f);
            yield return Stickman.Say("We're here!");

            GraphicPanelManager.instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/BG Images/Beach");
            AudioManager.instance.PlayTrack("Audio/Music/Calm2", volumeCap: 0.5f);
            yield return Stickman.Say("More beach!");

            GraphicPanelManager.instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/BG Images/villagenight");
            AudioManager.instance.PlayTrack("Audio/Ambience/RainyMood", 0);
            AudioManager.instance.PlayTrack("Audio/Music/Calm", 1, pitch: 0.7f);
            Stickman.SetSprite(Stickman.GetSprite("Sad 2"), 1);
            yield return Stickman.Say("I think we strayed too far from the beach.");

            AudioManager.instance.StopTrack(1);
            
            yield return null;
        }
    }
}

