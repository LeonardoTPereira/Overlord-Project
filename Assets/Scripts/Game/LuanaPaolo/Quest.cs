using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest"), Serializable]
public class Quest : ScriptableObject
{
    private int tipo; //tipo de quest
    private int n3; //número "chave", ou seja, pra tipo "kill" é o número de inimigos, para tipo "talk" é o id do npc, etc
    //somente kill e drop usam n2 e n3, já que precisamos de 3 números, um para cada tipo de inimigo
    public int parent = -1, c1 = -1, c2 = -1; //nó pai e nós filhos
    private int n1;
    private int n2;

    public int Tipo { get => tipo; set => tipo = value; }
    public int N1 { get => n1; set => n1 = value; }
    public int N2 { get => n2; set => n2 = value; }
    public int N3 { get => n3; set => n3 = value; }

    public override string ToString()
    {
        return "Type="+Tipo+"n1="+N1+"n2="+N2+"n3="+N3;
    }
}
