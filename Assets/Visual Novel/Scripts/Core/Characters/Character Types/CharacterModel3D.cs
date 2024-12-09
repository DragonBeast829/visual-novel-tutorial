using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS {
    public class CharacterModel3D : Character {
        public const string CHARACTER_RENDER_GROUP_PREFAB_NAME_FORMAT = "RenderGroup - [{0}]";
        public const string CHARACTER_RENDER_TEXTURE_NAME_FORMAT = "RenderTexture";
        public const int CHARACTER_STACKING_DEPTH = 15;

        private GameObject renderGroup;
        private Camera camera;
        private Transform modelContainer, model;
        private Animator modelAnimator;
        private SkinnedMeshRenderer modelExpressionController;
        private RawImage renderer;

        public CharacterModel3D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab) {
            Debug.Log($"Created Model3D Character: '{name}'");

            GameObject renderGroupPrefab = Resources.Load<GameObject>(rootAssetsFolder + '/' + string.Format(CHARACTER_RENDER_GROUP_PREFAB_NAME_FORMAT, config.name));
            renderGroup = Object.Instantiate(renderGroupPrefab, characterManager.characterPanelModel3D);
            renderGroup.name = string.Format(CHARACTER_RENDER_GROUP_PREFAB_NAME_FORMAT, name);
            renderGroup.SetActive(true);
            camera = renderGroup.GetComponentInChildren<Camera>();
            modelContainer = camera.transform.GetChild(0);
            model = modelContainer.GetChild(0);
            modelAnimator = model.GetComponent<Animator>();
            modelExpressionController = model.GetComponentsInChildren<SkinnedMeshRenderer>().FirstOrDefault(sm => sm.sharedMesh.blendShapeCount > 0);

            renderer = animator.GetComponentInChildren<RawImage>();
            RenderTexture renderTex = Resources.Load<RenderTexture>(rootAssetsFolder + '/' + CHARACTER_RENDER_TEXTURE_NAME_FORMAT);
            RenderTexture newTex = new RenderTexture(renderTex);
            renderer.texture = newTex;
            camera.targetTexture = newTex;

            int modelsInScene = characterManager.GetCharacterCountFromCharacterType(CharacterType.Model3D);
            renderGroup.transform.position += Vector3.down * (CHARACTER_STACKING_DEPTH * modelsInScene);
        }

        public void SetMotion(string motionName) {
            modelAnimator.Play(motionName);
        }
    }
}