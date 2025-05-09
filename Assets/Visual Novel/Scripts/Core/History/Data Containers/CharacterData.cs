using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

namespace History {
    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public string displayName;
        public bool enabled;
        public Color color;
        public int priority;
        public bool isHighlighted;
        public bool isFacingLeft;
        public Vector2 position;
        public CharacterConfigCache characterConfig;

        public string dataJSON;

        [System.Serializable]
        public class CharacterConfigCache {
            public string name;
            public string alias;

            public Character.CharacterType characterType;

            public Color nameColor;
            public Color dialogueColor;

            public string nameFont;
            public string dialogueFont;

            public float nameFontScale     = 1f;
            public float dialogueFontScale = 1f;

            public CharacterConfigCache(CharacterConfigData reference) {
                name          = reference.name;
                alias         = reference.alias;
                characterType = reference.characterType;
                
                nameColor     = reference.nameColor;
                dialogueColor = reference.dialogueColor;

                nameFont     = FilePaths.resources_fonts + reference.nameFont.name;
                dialogueFont = FilePaths.resources_fonts + reference.dialogueFont.name; 

                nameFontScale     = reference.nameFontScale;
                dialogueFontScale = reference.dialogueFontScale;
            }
        
         
        }
    
        public static List<CharacterData> Capture() {
            List<CharacterData> characters = new List<CharacterData>();

            foreach (var character in CharacterManager.instance.allCharacters) {
                // Only care about visible characters
                if (!character.isVisible) {
                    continue;
                }

                CharacterData entry = new CharacterData();
                entry.characterName   = character.name;
                entry.displayName     = character.displayName;
                entry.enabled         = character.isVisible;
                entry.color           = character.color;
                entry.priority        = character.priority;
                entry.isHighlighted   = character.highlighted;
                entry.position        = character.targetPosition;
                entry.characterConfig = new CharacterConfigCache(character.config);

                switch (character.config.characterType) {
                    case Character.CharacterType.Sprite:
                    case Character.CharacterType.SpriteSheet:
                        SpriteData sData = new SpriteData();
                        sData.layers     = new List<SpriteData.LayerData>();

                        CharacterSprite sc = character as CharacterSprite;

                        foreach (var layer in sc.layers) {
                            var layerData        = new SpriteData.LayerData();
                            layerData.color      = layer.renderer.color;
                            layerData.spriteName = layer.renderer.sprite.name;
                            sData.layers.Add(layerData);
                        }
                        entry.dataJSON = JsonUtility.ToJson(sData);
                        break;
                    case Character.CharacterType.Live2D:
                        Live2DData l2Data  = new Live2DData();
                        CharacterLive2D lc = character as CharacterLive2D;

                        l2Data.expression = lc.activeExpression;
                        l2Data.motion     = lc.activeMotion;

                        entry.dataJSON = JsonUtility.ToJson(l2Data);
                        break;
                    case Character.CharacterType.Model3D:
                        Model3DData m3Data = new Model3DData();
                        CharacterModel3D mc = character as CharacterModel3D;

                        m3Data.position = mc.model.position;
                        m3Data.rotation = mc.model.rotation;

                        entry.dataJSON = JsonUtility.ToJson(m3Data);
                        break;
                }

                characters.Add(entry);
            }

            return characters;
        }

        public static void Apply(List<CharacterData> data) {
            List<string> cache = new List<string>();

            foreach (CharacterData characterData in data) {
                Character character = CharacterManager.instance.GetCharacter(characterData.characterName, createIfDoesNotExist: true);
                character.displayName = characterData.displayName;
                character.SetColor(characterData.color);

                if (characterData.isHighlighted) {
                    character.Highlight(immediate: true);
                } else {
                    character.Unhighlight(immediate: true);
                }

                character.SetPriority(characterData.priority);

                if (characterData.isFacingLeft) {
                    character.FaceLeft(immediate: true);
                } else {
                    character.FaceRight(immediate: false);
                }

                character.SetPosition(characterData.position);

                character.isVisible = characterData.enabled;

                switch (character.config.characterType) {
                    case Character.CharacterType.Sprite:
                    case Character.CharacterType.SpriteSheet:
                        SpriteData sData = JsonUtility.FromJson<SpriteData>(characterData.dataJSON);
                        CharacterSprite sc = character as CharacterSprite;

                        for (int i = 0; i < sData.layers.Count; i++) {
                            var layer = sData.layers[i];
                            if (sc.layers[i].renderer.sprite != null && sc.layers[i].renderer.sprite.name != layer.spriteName) {
                                Sprite sprite = sc.GetSprite(layer.spriteName);
                                if (sprite != null) {
                                    sc.SetSprite(sprite, i);
                                } else {
                                    Debug.LogWarning($"History State could not load sprite '{layer.spriteName}'");
                                }
                            }
                        }
                        break;
                    case Character.CharacterType.Live2D:
                        Live2DData l2Data = JsonUtility.FromJson<Live2DData>(characterData.dataJSON);
                        CharacterLive2D lc = character as CharacterLive2D;
                        if (lc.activeExpression != l2Data.expression) {
                            lc.SetExpression(l2Data.expression);
                        }
                        if (lc.activeMotion != l2Data.motion) {
                            lc.SetMotion(l2Data.motion);
                        }
                        break;
                    case Character.CharacterType.Model3D:
                        Model3DData m3Data = JsonUtility.FromJson<Model3DData>(characterData.dataJSON);
                        CharacterModel3D mc = character as CharacterModel3D;
                        mc.model.position = m3Data.position;
                        mc.model.rotation = m3Data.rotation;
                        break;
                }

                cache.Add(character.name);
            }
            foreach (Character character in CharacterManager.instance.allCharacters) {
                if (!cache.Contains(character.name)) {
                    character.isVisible = false;
                }
            }
        }

        [System.Serializable]
        public class SpriteData {
            public List<LayerData> layers;

            [System.Serializable]
            public class LayerData {
                public string spriteName;
                public Color color;
            }
        }

        [System.Serializable]
        public class Live2DData {
            public string expression;
            public string motion;
        }

        [System.Serializable]
        public class Model3DData {
            public Vector3 position;
            public Quaternion rotation;
        }
    }
}
