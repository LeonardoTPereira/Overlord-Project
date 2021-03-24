using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest")]
public class Quest : ScriptableObject
{
    public int tipo;
    public int n;
    public int node;
    public int parent = -1, c1 = -1, c2 = -1;
}
