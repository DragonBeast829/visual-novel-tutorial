using System;
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
    
        public static class Expressions {
            public static HashSet<string> OPERATORS = new() {
                "-",
                "-=",
                "+",
                "-=",
                "*",
                "*=",
                "/",
                "/=",
                "=",
            };
            public static readonly string REGEX_ARITHMATIC = @"([-+*/=]=?)";
            public static readonly string REGEX_OPERATOR_LINE = @"^\$\w+\s*(=|\+=|-=|\*=|/=|)\s*";

            public static object CalculateValue(string[] expressionParts) {
                List<string> operandStrings = new();
                List<string> operatorStrings = new();
                List<object> operands = new();

                for (int i = 0; i < expressionParts.Length; i++) {
                    string part = expressionParts[i].Trim();

                    if (part == string.Empty) {
                        continue;
                    }

                    if (OPERATORS.Contains(part)) {
                        operatorStrings.Add(part);
                    } else {
                        operandStrings.Add(part);
                    }
                }

                foreach (string operandString in operandStrings) {
                    operands.Add(ExtractValue(operandString));
                }

                CalculateValue_DivisionAndMultiplication(operatorStrings, operands);

                CalculateValue_AdditionAndSubtraction(operatorStrings, operands);

                return operands[0];
            }

            private static void CalculateValue_DivisionAndMultiplication(List<string> operatorStrings, List<object> operands) {
                for (int i = 0; i < operatorStrings.Count; i++) {
                    string operatorString = operatorStrings[i];

                    if (operatorString == "*" || operatorString == "/") {
                        double leftOperand = Convert.ToDouble(operands[i]);
                        double rightOperand = Convert.ToDouble(operands[i + 1]);
                        if (operatorString == "*") {
                            operands[i] = leftOperand * rightOperand;
                        } else {
                            if (rightOperand == 0) {
                                Debug.LogError("Cannot divide by zero!");
                                return;
                            }
                            operands[i] = leftOperand / rightOperand;
                        }

                        // Remove the processed operands and operators
                        operands.RemoveAt(i + 1);
                        operatorStrings.RemoveAt(i);
                        i--;
                    }
                }
            }

            private static void CalculateValue_AdditionAndSubtraction(List<string> operatorStrings, List<object> operands) {
                for (int i = 0; i < operatorStrings.Count; i++) {
                    string operatorString = operatorStrings[i];

                    if (operatorString == "+" || operatorString == "-") {
                        double leftOperand = Convert.ToDouble(operands[i]);
                        double rightOperand = Convert.ToDouble(operands[i + 1]);

                        if (operatorString == "+") {
                            operands[i] = leftOperand + rightOperand;
                        } else {
                            operands[i] = leftOperand - rightOperand;
                        }
                        
                        // Remove the processed operands and operators
                        operands.RemoveAt(i + 1);
                        operatorStrings.RemoveAt(i);
                        i--;
                    }
                }
            }

            private static object ExtractValue(string value) {
                bool negate = false;

                if (value.StartsWith('!')) {
                    negate = true;
                    value = value.Substring(1);
                }

                if (value.StartsWith(VariableStore.VARIABLE_ID)) {
                    string variableName = value.TrimStart(VariableStore.VARIABLE_ID);
                    if (!VariableStore.HasVariable(variableName)) {
                        Debug.LogError($"Variable {variableName} does not exist!");
                        return null;
                    }

                    VariableStore.TryGetValue(variableName, out object val);

                    if (val is bool boolValue && negate) {
                        return !boolValue;
                    }
                    
                    return val;
                } else if (value.StartsWith('\"') && value.EndsWith('\"')) {
                    value = TagManager.Inject(value, injectTags: true, injectVariables: true);
                    return value.Trim('"');
                } else {
                    if (int.TryParse(value, out int intValue)) {
                        return intValue;
                    } else if (float.TryParse(value, out float floatValue)) {
                        return floatValue;
                    } else if (bool.TryParse(value, out bool boolValue)) {
                        return negate ? !boolValue : boolValue;
                    } else {
                        return value;
                    }
                }
            }
        }
    }
}

