using System.Collections;
using System.Collections.Generic;
using DIALOGUE.LogicalLines;
using UnityEngine;

using static DIALOGUE.LogicalLines.LogicalLineUtilities.Encapsulation;
using static DIALOGUE.LogicalLines.LogicalLineUtilities.Conditions;

namespace DIALOGUE.LogicalLines {
    public class LL_Condition : ILogicalLine
    {
        public string keyword => "if";
        private const string ELSE = "else";
        private readonly string[] CONTAINERS = new string[] { "(", ")" };

        public IEnumerator Execute(DIALOGUE_LINE line)
        {
            string rawCondition = ExtractCondition(line.rawData.Trim());
            bool conditionResult = EvaluateCondition(rawCondition);
            
            Conversation currentConversation = DialogueSystem.instance.conversationManager.conversation;
            int currentProgress = DialogueSystem.instance.conversationManager.conversationProgress;

            EncapsulatedData ifData = RipEncapsulatedData(currentConversation, currentProgress, false);
            EncapsulatedData elseData = new EncapsulatedData();

            if (ifData.endingIndex + 1 < currentConversation.Count) {
                // There are lines after the if data
                string nextLine = currentConversation.GetLines()[ifData.endingIndex + 1].Trim();
                if (nextLine == ELSE) {
                    elseData = RipEncapsulatedData(currentConversation, ifData.endingIndex + 1, false);

                    ifData.endingIndex = elseData.endingIndex;
                }
            }
            
            currentConversation.SetProgress(ifData.endingIndex);

            EncapsulatedData selectedData = conditionResult ? ifData : elseData;
            if (selectedData.lines.Count > 0) {
                Conversation newConversation = new Conversation(selectedData.lines);

                // Set the conversation progress to wherever it needs to go to after the condition
                DialogueSystem.instance.conversationManager.conversation.SetProgress(selectedData.endingIndex);

                DialogueSystem.instance.conversationManager.EnqueuePriority(newConversation);
            }

            yield return null;
        }

        public bool Matches(DIALOGUE_LINE line)
        {
            return line.rawData.Trim().StartsWith(keyword);
        }

        private string ExtractCondition(string line) {
            int startIndex = line.IndexOf(CONTAINERS[0]) + 1;
            int endIndex   = line.IndexOf(CONTAINERS[1]);

            return line.Substring(startIndex, endIndex - startIndex).Trim();
        }
    }
}
