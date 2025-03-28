using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE.LogicalLines {
    public static class LogicalLineUtilities
    {
        public static class Encapsulation {
            public struct EncapsulatedData {
                public List<string> lines;
                public int startingIndex;
                public int endingIndex;
            }

            private const char ENCAPSULATION_START = '{';
            private const char ENCAPSULATION_END = '}';

            public static EncapsulatedData RipEncapsulatedData(Conversation conversation, int startingIndex, bool ripHeadersAndEncapsulators = false) {
                // Depth is measured to ensure it doesn't prematurely end ripping the choice data.
                int encapsulationDepth = 0;

                EncapsulatedData data = new() {
                    lines = new(),
                    startingIndex = startingIndex,
                    endingIndex = 0
                };

                for (int i = startingIndex; i < conversation.Count; i++) {
                    string line = conversation.GetLines()[i];

                    if (ripHeadersAndEncapsulators
                        || (encapsulationDepth > 0 && !IsEncapsulationEnd(line))) {
                        data.lines.Add(line);
                    }
                    

                    if (IsEncapsulationStart(line)) {
                        encapsulationDepth++;
                        continue;
                    }

                    if (IsEncapsulationEnd(line)) {
                        encapsulationDepth--;
                        if (encapsulationDepth == 0) {
                            data.endingIndex = i;
                            break;
                        }
                    }
                }

                return data;
            }
            
            public static bool IsEncapsulationStart(string line) => line.Trim().StartsWith(ENCAPSULATION_START);
            public static bool IsEncapsulationEnd(string line) => line.Trim().StartsWith(ENCAPSULATION_END);
        }
    }
}

