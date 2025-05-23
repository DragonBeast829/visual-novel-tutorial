using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public class VNManager : MonoBehaviour
{
    public static VNManager instance { get; private set; }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void LoadFile(string filePath)
    {
        List<string> lines = new List<string>();

        TextAsset file = Resources.Load<TextAsset>(filePath);

        try
        {
            lines = FileManager.ReadTextAsset(file);
        }
        catch
        {
            Debug.LogError($"Dialogue file at path Resources/{filePath} does not exist!");
            return;
        }

        DialogueSystem.instance.Say(lines, filePath);
    }
}
