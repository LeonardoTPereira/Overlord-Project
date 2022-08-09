using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRunner : MonoBehaviour
{
    public DialogueTree tree;

    void Start()
    {
        tree = tree.Clone();
    }

    void Update()
    {
        tree.Update();
    }
}
