using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS {
    public class CharacterSprite : Character {
        private const string SPRITE_RENDERER_PARENT_NAME = "Renderers";
        private const string SPRITESHEET_DEFAULT_SHEET_NAME = "Default";
        private const char SPRITESHEET_TEX_SPRITE_DELIMITER = '-';
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();
        
        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        private string artAssetsDirectory = "";

        public override bool isVisible  {
            get { return isRevealing || rootCG.alpha > 0; }
            set { rootCG.alpha = value ? 1 : 0; }
        }

        public CharacterSprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab) {
            rootCG.alpha = ENABLE_ON_START ? 1 : 0;
            artAssetsDirectory = $"{rootAssetsFolder}/Images";
            GetLayers();
            Debug.Log($"Created Sprite Character: '{name}'");
        }

        private void GetLayers() {
            Transform rendererRoot = animator.transform.Find(SPRITE_RENDERER_PARENT_NAME);

            if (rendererRoot == null) return;

            for (int i = 0; i < rendererRoot.transform.childCount; i++) {
                Transform child = rendererRoot.transform.GetChild(i);

                Image rendererImage = child.GetComponentInChildren<Image>();

                if (rendererImage != null) {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Layer: {i}";
                }
            }
        }

        public void SetSprite(Sprite sprite, int layer = 0) {
            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName) {
            if (config.sprites.Count > 0) {
                if (config.sprites.TryGetValue(spriteName, out Sprite sprite)) {
                    return sprite;
                }
            }
            // If the character doesn't have the sprites listed, check the Resources folder
            if (config.characterType == CharacterType.SpriteSheet) {
                string[] data = spriteName.Split(SPRITESHEET_TEX_SPRITE_DELIMITER);
                
                Sprite[] spriteArray = new Sprite[0];
                if (data.Length == 2) {
                    string textureName = data[0];
                    spriteName = data[1];
                    Debug.Log($"{artAssetsDirectory}/{textureName}");
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{textureName}");
                } else {
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{SPRITESHEET_DEFAULT_SHEET_NAME}");
                }
                
                if (spriteArray.Length == 0) {
                    Debug.LogWarning($"Character '{name}' does not have a default art asset called '{SPRITESHEET_DEFAULT_SHEET_NAME}'");
                }

                return Array.Find(spriteArray, sprite => sprite.name == spriteName);
            } else {
                string path = $"{artAssetsDirectory}/{spriteName}";
                return Resources.Load<Sprite>(path);
            }
        }

        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1) {
            CharacterSpriteLayer spriteLayers = layers[layer];

            return spriteLayers.TransitionSprite(sprite, speed);
        }

        public override IEnumerator ShowingOrHiding(bool show, float speedMultipler = 1f)
        {
            float targetAlpha = show ? 1f : 0;

            CanvasGroup self = rootCG;

            while (self.alpha != targetAlpha) {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime * speedMultipler);
                yield return null;
            }

            co_revealing = null;
            co_hiding = null;
        }

        public override void SetColor(Color color)
        {
            base.SetColor(color);

            color = displayColor;

            foreach (CharacterSpriteLayer layer in layers) {
                layer.StopChangingColor();
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(float speed) {
            foreach (CharacterSpriteLayer layer in layers) {
                layer.TransitionColor(displayColor, speed);
            }

            yield return null;

            while (layers.Any(l => l.isChangingColor)) {
                yield return null;
            }

            co_changingColor = null;
        }

        public override IEnumerator Highlighting(float speedMultiplier, bool immediate = false)
        {
            Color targetColor = displayColor;

            foreach(CharacterSpriteLayer layer in layers) {
                if (immediate) layer.SetColor(displayColor);
                else layer.TransitionColor(targetColor, speedMultiplier);
            }

            yield return null;

            while (layers.Any(l => l.isChangingColor)) {
                yield return null;
            }

            co_highlighting = null;
        }

        public override IEnumerator FaceDirection(bool faceLeft, float speedMultipler, bool immediate)
        {
            foreach (CharacterSpriteLayer layer in layers) {
                if (faceLeft) layer.FaceLeft(speedMultipler, immediate);
                else layer.FaceRight(speedMultipler, immediate);
            }
            yield return null;
            while (layers.Any(l => l.isFlipping)) {
                yield return null;
            }
            co_flipping = null;
        }

        public override void OnReceiveCastingExpression(int layer, string expression)
        {
            Sprite sprite = GetSprite(expression);

            if (sprite == null) {
                Debug.LogWarning($"Sprite '{expression}' could not be found for character '{name}");
                return;
            }

            TransitionSprite(sprite, layer);
        }
    }
}