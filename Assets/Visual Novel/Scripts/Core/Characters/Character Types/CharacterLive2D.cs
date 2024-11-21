using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;
using Live2D.Cubism.Framework.Expression;

namespace CHARACTERS {
    public class CharacterLive2D : Character {
        private CubismRenderController renderController;
        private CubismExpressionController expressionController;
        private Animator motionAnimator;
        public CharacterLive2D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab) {
            Debug.Log($"Created Live2D Character: '{name}'");
            motionAnimator = animator.transform.GetChild(0).GetComponentInChildren<Animator>();
            renderController = motionAnimator.GetComponent<CubismRenderController>();
            expressionController = motionAnimator.GetComponent<CubismExpressionController>();
        }

        public void SetMotion(string animationName) {
            motionAnimator.Play(animationName);
        }

        public void SetExpression(int expressionIndex) {
            expressionController.CurrentExpressionIndex = expressionIndex;
        }

        public void SetExpression(string expressionName) {
            expressionController.CurrentExpressionIndex = GetExpressionIndexByName(expressionName);
        }

        private int GetExpressionIndexByName(string expressionName) {
            expressionName = expressionName.ToLower();
            for (int i = 0; i < expressionController.ExpressionsList.CubismExpressionObjects.Length; i++) {
                CubismExpressionData expr = expressionController.ExpressionsList.CubismExpressionObjects[i];
                if (expr.name.Split('.')[0].ToLower() == expressionName) return i;
            }
            return -1;
        }
    }
}