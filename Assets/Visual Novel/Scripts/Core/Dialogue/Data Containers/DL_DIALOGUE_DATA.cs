using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DIALOGUE {
    public class DL_DIALOGUE_DATA {
        public string rawData { get; private set; } = string.Empty;

        public List<DIALOGUE_SEGMENT> segments;
        private const string segmentIdentifierPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";

        public DL_DIALOGUE_DATA(string rawDialogue) {
            this.rawData = rawDialogue;
            segments = RipSegments(rawDialogue);
        }

        private List<DIALOGUE_SEGMENT> RipSegments(string rawDialogue) {
            List<DIALOGUE_SEGMENT> segments = new List<DIALOGUE_SEGMENT>();

            MatchCollection matches = Regex.Matches(rawDialogue, segmentIdentifierPattern);

            int lastIndex = 0;

            DIALOGUE_SEGMENT segment = new DIALOGUE_SEGMENT();

            segment.dialogue = matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index);
            segment.startSignal = DIALOGUE_SEGMENT.StartSignal.NONE;
            segment.signalDelay = 0;
            segments.Add(segment);

            if (matches.Count == 0) {
                return segments;
            } else {
                lastIndex = matches[0].Index;
            }

            for (int i = 0; i < matches.Count; i++) {
                Match match = matches[i];
                segment = new DIALOGUE_SEGMENT();

                string signalMatch = match.Value;
                signalMatch = signalMatch.Substring(1, match.Length - 2);
                string[] signalSplit = signalMatch.Split(' ');

                segment.startSignal = (DIALOGUE_SEGMENT.StartSignal)Enum.Parse(typeof(DIALOGUE_SEGMENT.StartSignal), signalSplit[0].ToUpper());

                if (signalSplit.Length > 1) {
                    float.TryParse(signalSplit[1], out segment.signalDelay);
                }

                int nextIndex = i + 1 < matches.Count ? matches[i + 1].Index : rawDialogue.Length;
                segment.dialogue = rawDialogue.Substring(lastIndex + match.Length, nextIndex - (lastIndex + match.Length));
                lastIndex = nextIndex;

                segments.Add(segment);
            }

            return segments;
        }

        public struct DIALOGUE_SEGMENT {
            public string dialogue;
            public StartSignal startSignal;
            public float signalDelay;

            public enum StartSignal {
                NONE = 0, // No signal
                C = 1, // Clear
                A = 2, // Append
                WC = 3, // Wait, then clear
                WA = 4, // Wait, then append
            }

            public bool appendText => startSignal == StartSignal.A || startSignal == StartSignal.WA;
        }
    }
}