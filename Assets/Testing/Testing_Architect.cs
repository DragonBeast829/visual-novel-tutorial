using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING {
    public class Testing_Architect : MonoBehaviour {
        DialogueSystem ds;
        TextArchitect architect;

        string[] lines = new string[1]
        {
            "blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah"
        };

        public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.Instant;

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.Fade;
            architect.speed = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            if (bm != architect.buildMethod) { 
                architect.buildMethod = bm;
                architect.Stop();
            }

            if (Input.GetKeyUp(KeyCode.S)) {
                architect.Stop();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architect.isBuilding)
                {
                    if (!architect.hurryUp)
                    {
                        architect.hurryUp = true;
                    }
                    else
                    {
                        architect.ForceComplete();
                    }
                }
                else
                {
                    architect.Build(lines[Random.Range(0, lines.Length)]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                architect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}