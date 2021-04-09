using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quantities : MonoBehaviour
{
    public int numberTreasures = 0, numberEnemies = 0;
    public int totalTreasures, totalEnemies;
    public bool isEnemy;

    public string goodLevel, badLevel;

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Final"){
            if(isEnemy){
                if(totalEnemies <= numberEnemies && goodLevel != null) SceneManager.LoadScene(goodLevel);
                else if(badLevel != null) SceneManager.LoadScene(badLevel);
                else if(badLevel == "null" && goodLevel == "null") Application.Quit();
            }
            else{
                if(totalTreasures <= numberTreasures && goodLevel != null) SceneManager.LoadScene(goodLevel);
                else if(badLevel != null) SceneManager.LoadScene(badLevel);
                else if(badLevel == "null" && goodLevel == "null") Application.Quit();
            } 
        }
    }

}
