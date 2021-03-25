using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dungeonEntrance : MonoBehaviour
{
    public string nameScene;
    public string fileName;
    private GameManager gameManager;

    public delegate void LoadEnemies(string levelFile, int difficulty);
    public static event LoadEnemies enemies; 

    void Start(){
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            enemies(fileName, 1);
            SceneManager.LoadScene(nameScene);
        }
    }
}
