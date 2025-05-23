using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace History {
    public class HistoryNavigation : MonoBehaviour {
        public int progress = 0;

        [SerializeField] private TextMeshProUGUI statusText;
        private HistoryManager manager => HistoryManager.instance;
        private List<HistoryState> history => manager.history;

        // The most recently viewed history state is not yet stored in the HistoryManager, so it needs to be cached
        private HistoryState cachedState = null;
        private bool isOnCachedState = false;

        public bool isViewingHistory = false;

        public bool canNavigate => DialogueSystem.instance.conversationManager.isOnLogicalLine == false;

        public void GoForward() {
            if (!isViewingHistory || !canNavigate) {
                return;
            }
            HistoryState state = null;

            if (progress < history.Count - 1) {
                progress++;
                state = history[progress];
            } else {
                isOnCachedState = true;
                state = cachedState;
            }

            state.Load();

            if (isOnCachedState) {
                isViewingHistory = false;
                DialogueSystem.instance.onUserPrompt_Next -= GoForward;
                statusText.text = "";
                DialogueSystem.instance.OnStopViewingHistory();
            } else {
                UpdateStatusText();
            }
        }

        public void GoBack() {
            if (history.Count == 0 || (progress == 0 && isViewingHistory) || !canNavigate) {
                return;
            }

            progress = isViewingHistory
                ? progress - 1
                : history.Count - 1;
            
            if (!isViewingHistory) {
                isViewingHistory = true;
                isOnCachedState = false;
                cachedState = HistoryState.Capture();

                DialogueSystem.instance.onUserPrompt_Next += GoForward;
                DialogueSystem.instance.OnStartViewingHistory();
            }

            HistoryState state = history[progress];
            state.Load();
            UpdateStatusText();
        }

        private void UpdateStatusText() {
            statusText.text = $"{progress + 1}/{history.Count}";
        }
    }
}

