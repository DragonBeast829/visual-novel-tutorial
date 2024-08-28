using System.Collections;
using System.Collections.Generic;
using COMMANDS;
using UnityEngine;

namespace TESTING {
    public class CommandTesting : MonoBehaviour {
        private void Start() {
            StartCoroutine(Running());
        }

        IEnumerator Running() {
            yield return CommandManager.instance.Execute("print");
            yield return CommandManager.instance.Execute("print_1p", "Hello, World!");
            yield return CommandManager.instance.Execute("print_mp", "Foo", "Bar", "Baz");

            yield return CommandManager.instance.Execute("lambda");
            yield return CommandManager.instance.Execute("lambda_1p", "Hello, World! 2");
            yield return CommandManager.instance.Execute("lambda_mp", "Foo2", "Bar2", "Baz2");

            yield return CommandManager.instance.Execute("process");
            yield return CommandManager.instance.Execute("process_1p", "3");
            yield return CommandManager.instance.Execute("process_mp", "Process Line 1", "Process Line 2", "Process Line 3");
        }
    }
}