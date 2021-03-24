using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour{
    public Selector sel = new Selector(); //seletor
    
    public List<Quest> graph = new List<Quest>(); //lista de quests
    
    public bool isFinished = true; //verifica se a missão já terminou
    
    public Player_Movement player; //jogador

    void Start(){
        player = FindObjectOfType<Player_Movement>();
    }

    void Update(){
        if(isFinished == true){
            isFinished = false;
            sel.select(this);

            makeBranches();

            for(int i = 0; i < graph.Count; i++) Debug.Log(graph[i].tipo + ", " + graph[i].c1 + ", " + graph[i].c2);

            isFinished = false;
        }

        //if(isFinished == false) mission();
    }

    void makeBranches(){
        int index = 0, b, c1, c2;

        graph[index].parent = -1;

        while(index < graph.Count){
            b = Random.Range(0, 100);

            if(b%2 == 0){
                c1 = Random.Range(index+1, graph.Count);
                if(c1 < graph.Count) graph[c1].parent = index;

                c2 = Random.Range(index+1, graph.Count);
                if(c2 < graph.Count) graph[c2].parent = index;

                graph[index].c1 = c1;
                if(c1 != c2) graph[index].c2 = c2;
            }
            else if(index+1 < graph.Count){
                if(graph[index+1].parent == -1){
                    graph[index+1].parent = index;
                    graph[index].c1 = index + 1;
                }
            }

            index++;
        }
    }
}