using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest"), Serializable]
public class Quest : ScriptableObject
{
    public int tipo; //tipo de quest
    public int n1, n2, n3; //número "chave", ou seja, pra tipo "kill" é o número de inimigos, para tipo "talk" é o id do npc, etc
    //somente kill e drop usam n2 e n3, já que precisamos de 3 números, um para cada tipo de inimigo
    public int parent = -1, c1 = -1, c2 = -1; //nó pai e nós filhos

    public override string ToString()
    {
        return "Type="+tipo+"n1="+n1+"n2="+n2+"n3="+n3;
    }
}
