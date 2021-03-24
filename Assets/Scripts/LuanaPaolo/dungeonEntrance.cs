using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dungeonEntrance : MonoBehaviour
{
    public string nameScene;
    public TextAsset mapFile;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            FindObjectOfType<GameManager>().mapFile = mapFile;
            SceneManager.LoadScene(nameScene);
        }
    }
}
