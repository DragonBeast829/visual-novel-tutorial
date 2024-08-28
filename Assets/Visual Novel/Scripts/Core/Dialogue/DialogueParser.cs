using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE {
    public class DialogueParser : MonoBehaviour {
        private const string commandRegexPattern = @"[\w\[\]]*[^\s]\(";

        public static DIALOGUE_LINE Parse(string rawLine) {

            (string speaker, string dialogue, string commands) = RipContent(rawLine);
            Debug.Log($"speaker: {speaker}");
            Debug.Log($"dialogue: {dialogue}");
            Debug.Log($"commands: {commands}");

            return new DIALOGUE_LINE(speaker, dialogue, commands);
        }
        private static (string, string, string) RipContent(string rawLine) {
            Debug.Log("In rip content");
            string speaker = "";
            string dialogue = "";
            string commands = "";

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            for (int i = 0; i < rawLine.Length; i++) {
                char current = rawLine[i];
                if (current == '\\') {
                    isEscaped = !isEscaped;
                } else if (current == '\"' && !isEscaped) {
                    if (dialogueStart == -1) {
                        dialogueStart = i;
                    } else if (dialogueEnd == -1) {
                        dialogueEnd = i;
                    }
                } else {
                    isEscaped = false;
                }
            }
            Debug.Log($"Before regex, commands: {commands}");
            Regex commandRegex = new Regex(commandRegexPattern);
            MatchCollection matches = commandRegex.Matches(rawLine);
            int commandStart = -1;
            foreach (Match match in matches) {
                if (match.Index < dialogueStart || match.Index > dialogueEnd) {
                    commandStart = match.Index;
                    break;
                }
            }
            Debug.Log($"After regex, commandStart: {commandStart}, dialogueStart: {dialogueStart}, dialogueEnd: {dialogueEnd}");

            if (commandStart == -1 && (dialogueStart == -1 && dialogueEnd == -1)) {
                return ("", "", rawLine.Trim());
            }

            Debug.Log($"dialogueStart: {dialogueStart}\ndialogueEnd: {dialogueEnd}\ncommandStart: {commandStart}");
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd)) {
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"","\"");
                if (commandStart != -1) {
                    commands = rawLine.Substring(commandStart).Trim();
                }
            } else if (commandStart != -1 && dialogueStart > commandStart) {
                commands = rawLine;
            } else {
                dialogue = rawLine;
            }

            return (speaker, dialogue, commands);
        }
    }
}
