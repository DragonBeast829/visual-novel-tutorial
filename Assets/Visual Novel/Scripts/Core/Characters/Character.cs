using DIALOGUE;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

namespace CHARACTERS {
    public abstract class Character {
        public const bool ENABLE_ON_START = false;
        private const float UNHIGHLIGHTED_DARKEN_STRENGTH = 0.65f;
        public const bool DEFAULT_ORIENTATION_IS_FACING_LEFT = true;
        public const string ANIMATION_REFRESH_TRIGGER = "Refresh";

        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public Animator animator;
        public Color color { get; protected set; } = Color.white;
        protected Color displayColor => highlighted ? highlightedColor : unhighlightedColor;
        protected Color highlightedColor => color;
        protected Color unhighlightedColor => new Color(color.r * UNHIGHLIGHTED_DARKEN_STRENGTH, color.g * UNHIGHLIGHTED_DARKEN_STRENGTH, color.b * UNHIGHLIGHTED_DARKEN_STRENGTH, color.a) ;
        public bool highlighted { get; protected set; } = true;
        protected bool facingLeft = DEFAULT_ORIENTATION_IS_FACING_LEFT;
        public int priority { get; protected set; }
        public Vector2 targetPosition { get; private set; }

        public CharacterManager characterManager => CharacterManager.instance;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        // Coroutines
        protected Coroutine co_revealing, co_hiding;
        protected Coroutine co_moving;
        protected Coroutine co_changingColor;
        protected Coroutine co_highlighting;
        protected Coroutine co_flipping;
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public bool isMoving => co_moving != null;
        public bool isChangingColor => co_changingColor != null;
        public bool isHighlighting => highlighted && co_highlighting != null;
        public bool isUnhighlighting => !highlighted && co_highlighting != null;
        public virtual bool isVisible { get; set; }
        public bool isFacingLeft => facingLeft;
        public bool isFacingRight => !facingLeft;
        public bool isFlipping => co_flipping != null;

        public Character(string name, CharacterConfigData config, GameObject prefab) {
            this.name = name;
            displayName = name;
            this.config = config;

            if (prefab != null) {
                RectTransform parentPanel =
                    config.characterType == CharacterType.Live2D
                    ? characterManager.characterPanelLive2D
                    : characterManager.characterPanel;
                GameObject ob = Object.Instantiate(prefab, parentPanel);
                ob.name = characterManager.FormatCharacterPath(characterManager.characterPrefabNameFormat, name);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        public Coroutine Say(List<string> dialogue) {
            dialogueSystem.ShowSpeakerName(displayName);
            dialogueSystem.ApplySpeakerDataToDialogueContainer(config);
            UpdateTextCustomizationsOnScreen();
            return dialogueSystem.Say(dialogue);
        }

        
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void ResetConfigurationData() => config = CharacterManager.instance.GetCharacterConfig(name, getOriginal: true);
        public void UpdateTextCustomizationsOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);
        
        public virtual Coroutine Show(float speedMultipler = 1f) {
            if (isRevealing) characterManager.StopCoroutine(co_revealing);

            if (isHiding) characterManager.StopCoroutine(co_hiding);

            co_revealing = characterManager.StartCoroutine(ShowingOrHiding(true, speedMultipler));

            return co_revealing;
        }

        public virtual Coroutine Hide(float speedMultipler = 1f) {
            if (isHiding) return co_hiding;

            if (isRevealing) characterManager.StopCoroutine(co_hiding);

            co_hiding = characterManager.StartCoroutine(ShowingOrHiding(false, speedMultipler));

            return co_hiding;
        }

        public virtual IEnumerator ShowingOrHiding(bool show, float speedMultipler = 1f) {
            Debug.Log("Show/Hide cannot be called from a base character type.");
            yield return null;
        }

        public virtual void SetPosition(Vector2 position) {
            if (root == null) return;

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);

            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;

            targetPosition = position;
        }

        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false) {
            if (root == null) return null;

            if (isMoving) characterManager.StopCoroutine(co_moving);

            co_moving = characterManager.StartCoroutine(MovingToPosition(position, speed, smooth));

            targetPosition = position;

            return co_moving;
        }

        public IEnumerator MovingToPosition(Vector2 position, float speed, bool smooth = false) {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            Vector2 padding = root.anchorMax - root.anchorMin;

            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget) {
                root.anchorMin = smooth
                                      ? Vector2.Lerp(root.anchorMin, minAnchorTarget, speed * Time.deltaTime)
                                      : Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);

                root.anchorMax = root.anchorMin + padding;

                if (smooth && Vector2.Distance(root.anchorMin, minAnchorTarget) <= 0.001f) {
                    root.anchorMin = minAnchorTarget;
                    root.anchorMax = maxAnchorTarget;
                    break;
                }

                yield return null;
            }

            Debug.Log("Done moving");
            co_moving = null;
        }

        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTargets(Vector2 position) {
            Vector2 padding = root.anchorMax - root.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding;

            return (minAnchorTarget, maxAnchorTarget);
        }

        public virtual void SetColor(Color color) {
            this.color = color;
        }

        public Coroutine TransitionColor(Color color, float speed = 1f) {
            this.color = color;

            if (isChangingColor) characterManager.StopCoroutine(co_changingColor);

            co_changingColor = characterManager.StartCoroutine(ChangingColor(speed));

            return co_changingColor;
        }

        public virtual IEnumerator ChangingColor(float speed) {
            Debug.Log("Color changing is not applicable on this character type!");
            yield return null;
        }

        public Coroutine Highlight(float speed = 1f, bool immediate = false) {
            if (isHighlighting || isUnhighlighting) characterManager.StopCoroutine(co_highlighting);

            highlighted = true;

            co_highlighting = characterManager.StartCoroutine(Highlighting(speed, immediate));

            return co_highlighting;
        }
        
        public Coroutine Unhighlight(float speed = 1f, bool immediate = false) {
            if (isHighlighting || isUnhighlighting) characterManager.StopCoroutine(co_highlighting);

            highlighted = false;

            co_highlighting = characterManager.StartCoroutine(Highlighting(speed, immediate));

            return co_highlighting;
        }

        public virtual IEnumerator Highlighting(float speedMultiplier, bool immediate = false) {
            Debug.Log("Highlighting is not applicable on this character type!");
            yield return null;
        }

        public Coroutine Flip(float speed = 1, bool immediate = false) {
            if (isFacingLeft) return FaceRight(speed, immediate);
            else              return FaceLeft(speed, immediate);
        }

        public Coroutine FaceLeft(float speed = 1, bool immediate = false) {
            if (isFlipping) characterManager.StopCoroutine(co_flipping);

            facingLeft = true;
            co_flipping = characterManager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));

            return co_flipping;
        }

        public Coroutine FaceRight(float speed = 1, bool immediate = false) {
            if (isFlipping) characterManager.StopCoroutine(co_flipping);

            facingLeft = false;
            co_flipping = characterManager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));

            return co_flipping;
        }

        public virtual IEnumerator FaceDirection(bool faceLeft, float speedMultipler, bool immediate) {
                Debug.Log("Cannot flip a character of this type!");
                yield return null;
        }

        public void SetPriority(int priority, bool autoSortCharactersOnUI = true) {
            this.priority = priority;

            if (autoSortCharactersOnUI) characterManager.SortCharacters();
            root.SetSiblingIndex(this.priority);
        }

        public void Animate(string animation) {
            animator.SetTrigger(animation);
            animator.SetTrigger(ANIMATION_REFRESH_TRIGGER); 
            Debug.Log("Animating");
        }

        public void Animate(string animation, bool state) {
            animator.SetBool(animation, state);
            animator.SetTrigger(ANIMATION_REFRESH_TRIGGER);
        }

        public virtual void OnSort(int sortingIndex) {
            return;
        }

        public virtual void OnReceiveCastingExpression(int layer, string expression) {
            return;
        }

        public enum CharacterType {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D,
        }
    }
}