using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour{
    public Selector sel = new Selector(); //seletor
    
    //public List<Quest> graph = new List<Quest>(); //lista de quests
    
    public List<int> chain = new List<int>();
    public List<int> type = new List<int>();
    
    public bool isFinished = true; //verifica se a missão já terminou
    
    public Dialog_Box box;
    public Mission_Log log;
    
    public Player_Movement player; //jogador
    
    private int index1 = 0;
    private int index2 = 0;
    
    public Text txt;
    public Canvas canvas;
    
    public string[] sentences = new string[10];
    public string[] npcs = new string[3];
    public string[] foes = new string[3];
    public string[] rewards = new string[3];

    void Start(){
        player = FindObjectOfType<Player_Movement>();
        canvas.enabled = true;

        npcs[0] = "Bowser";
        npcs[1] = "Sidon";
        npcs[2] = "Ganon";

        foes[0] = "Lagartos gigantes";
        foes[1] = "Morcegos vampiro";
        foes[2] = "Lobisomens";

        rewards[0] = "rupees";
        rewards[1] = "dentes de dragão";
        rewards[2] = "escamas de lagarto";

        sentences[0] = "Vá falar com um dos aldeões, ele precisa de ajuda. Seu nome é ";
        sentences[1] = "Pegue o tesouro dentro do baú.";
        sentences[2] = "Explore a sala secreta de id ";
        sentences[3] = "Mate os inimigos que estão assustando a vila. São ";
        sentences[4] = "Colete ";
        sentences[5] = "Explore a sala de id ";
        sentences[6] = "Mate os inimigos para conseguir ";
        sentences[7] = "Vá falar com um dos moradores da vila para iniciar a missão. Seu nome é ";
        sentences[8] = "Vá ao painel de missões para iniciar a missão.";
        sentences[9] = "Missão Completa.";

        index1 = log.index1;
        index2 = log.index2;
        isFinished = log.isFinished;
        type = log.type;
    }

    void Update(){
        if(isFinished == true){
            isFinished = false;
            sel.select(this);
            
            if(chain.Count >= 2 && chain[0] == 0) type.Add(7);
            else if(chain.Count >= 2 && chain[0] != 0) type.Add(8);

            for(int i = 0; i < chain.Count; i++){
                type.Add(chain[i]);
            };

            type.Add(9);

            index1 = 0;
            index2 = -1;

            //for(int i = 0; i < type.Count; i++) Debug.Log(type[i]);
        }

        if(isFinished == false) mission();
    }

    void mission(){
        if(index1 == type.Count){
            isFinished = true;
            type.Clear();
            chain.Clear();
        } 

        else if(index1 != index2){
            index2 = index1;
            canvas.enabled = true;
            //Debug.Log(box.sentences[type[index1]]);
            box.dialog = sentences[type[index1]];
            if(type[index1] == 2 || type[index1] == 5){
                box.value = Random.Range(0, 1000);
                box.dialog += box.value;
            }
            if(type[index1] == 3 || type[index1] == 4){
                box.value = Random.Range(0, 10);
                box.dialog += box.value;
            }
            if(type[index1] == 0 || type[index1] == 7){
                box.name = npcs[Random.Range(0, 3)];
                box.dialog += box.name;
            }
            if(type[index1] == 4){
                box.name = rewards[Random.Range(0, 3)];
                box.dialog += box.name;
            }

            txt.text = box.dialog;
        }

        if(type.Count != 0){
            if((type[index1] == 0 || type[index1] == 7) && player.col == 0 && Input.GetKeyDown("space")) index1++;
            else if(type[index1] != 0 && type[index1] != 7 && player.col == 1 && Input.GetKeyDown("space")){
                if(type[index1] != 3 && type[index1] != 6) index1++;
                if(type[index1] == 3 || type[index1] == 6){
                    index1++;
                    log.index1 = index1;
                    log.index2 = index2;
                    log.isFinished = isFinished;
                    log.type = type;
                    SceneManager.LoadScene("MainBreno");
                    index1 = log.index1;
                    index2 = log.index2;
                    isFinished = log.isFinished;
                    type = log.type;
                }
            }
        }
    }
}